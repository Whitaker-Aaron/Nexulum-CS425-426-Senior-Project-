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
        print("Bubble rad has been changed");
    }
}
