using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSkillMenu : MonoBehaviour
{
    SkillTreeManager skillTreeManager;
    // Start is called before the first frame update
    private void Awake()
    {
        skillTreeManager = GameObject.Find("SkillTreeManager").GetComponent<SkillTreeManager>();
    }

    public void OnIncreaseBubbleRadius()
    {
        skillTreeManager.unlockSkill("IncBubRad");
        EffectsManager.instance.replacePoolEffects("bubbleShield", 1);
        EffectsManager.instance.replacePoolEffects("earthShield", 1);
        print("Bubble rad has been changed");
    }

    public void onIncreaseCombatAuraRad()
    {
        skillTreeManager.unlockSkill("IncComAuraRad");
        EffectsManager.instance.replacePoolEffects("caPool", 1);
        EffectsManager.instance.replacePoolEffects("faPool", 1);
        print("combat aura rad has been changed");
    }
}
