using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillTreeNode
{
    public string skillName;
    public bool isUnlocked;
    public Action onUnlock;

    public skillTreeNode(string name, Action unlockAction)
    {
        skillName = name;
        isUnlocked = false;
        onUnlock = unlockAction;
    }

    public void unlockSkill()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            onUnlock.Invoke();
        }
    }
}
