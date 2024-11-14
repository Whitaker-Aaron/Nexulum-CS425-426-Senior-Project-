using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellCastManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateSpellCast(Rune rune)
    {

        switch(rune.runeName)
        {
            case "lightningSpell":
                activateLightning();
                break;
        }



    }


    void activateLightning()
    {

    }
}