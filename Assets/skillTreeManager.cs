using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillTreeManager : MonoBehaviour
{
    public static skillTreeManager instance;
    private List<skillTreeNode> skillTree;
    classAbilties abilities;

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
        skillTree.Add(new skillTreeNode("IncBubRad", () => abilities.modifyBubbleRad(1f)));

        foreach (var s in skillTree)
        {
            Debug.Log("SkillTree Skill Name: " + s.skillName);
        }
    }

    private void Awake()
    {
        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object
            InitializeSkillTree();
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
