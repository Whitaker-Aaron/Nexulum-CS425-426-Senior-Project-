using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionChecks : ScriptableObject
{
    public bool hasVisitedDungeon = false;
    public bool hasPickedUpMaterial = false;

    private void Awake()
    {
        
    }

    public bool getHasVisitedDungeon()
    {
        return hasVisitedDungeon;
    }

    public void setHasVisitedDungeon(bool value)
    {
        hasVisitedDungeon = value;
    }

    public bool getHasPickedUpMaterial()
    {
        return hasPickedUpMaterial;
    }

    public void setHasPickedUpMaterial(bool value)
    {
        hasPickedUpMaterial = value;
    }

}
