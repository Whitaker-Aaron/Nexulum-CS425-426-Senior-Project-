using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;
    private List<skillTreeNode> skillTree;
    classAbilties abilities;

    [SerializedDictionary("skillName", "buffValue(float)")]
    public SerializedDictionary<string, float> floatBuffVals;

    [SerializedDictionary("skillName", "buffValue(int)")]
    public SerializedDictionary<string, int> intBuffVals;

    private float getFloatVal(string name)
    {
        //print("string name is: " + name);
        if (floatBuffVals.ContainsKey(name))
        {
            return floatBuffVals[name];
        }
        else
        {
            Debug.LogWarning($"Prefab with name {name} not found in the dictionary.");
            return 0f;
        }
    }

    private int getIntVal(string name)
    {
        //print("string name is: " + name);
        if (intBuffVals.ContainsKey(name))
        {
            return intBuffVals[name];
        }
        else
        {
            Debug.LogWarning($"Prefab with name {name} not found in the dictionary.");
            return 0;
        }
    }

    public void unlockSkill(string skillName)
    {
        Debug.Log("Skill name passed to UnlockSkill: " + skillName);
        if (skillTree == null || skillTree.Count == 0)
        {
            Debug.LogError("Skill tree is not initialized or empty.");
            return;
        }

        skillTreeNode skill = skillTree.Find(s => s.skillName.Trim().Equals(skillName.Trim(), StringComparison.OrdinalIgnoreCase));
        if (skill != null)
        {
            skill.unlockSkill();
        }
        else
            Debug.Log("Skill not found for: " + skillName);
    }

    public void Initialize()
    {
        //initialize list
        skillTree = new List<skillTreeNode>();

        abilities = GameObject.FindGameObjectWithTag("inputManager").GetComponent<classAbilties>();

        //where all skills will go

        //knight
        skillTree.Add(new skillTreeNode("IncBubRad", () => abilities.modifyBubbleRad(getFloatVal("bubbleRadUpgrade"))));
        skillTree.Add(new skillTreeNode("IncComAuraRad", () => abilities.modifyCombatAuraRad(getFloatVal("combatAuraRadUpgrade"))));

        //Gunner
        skillTree.Add(new skillTreeNode("IncGrenadeRad", () => abilities.modifyGrenadeRad(getFloatVal("grenadeUpgrade"))));


        //Engineer

        foreach (var s in skillTree)
        {
            Debug.Log("SkillTree Skill Name: " + s.skillName);
        }
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            unlockSkill("IncGrenadeRad");
        }
    }
}
