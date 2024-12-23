using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runeIntController : MonoBehaviour, RuneInt
{

    WeaponBase weapon;
    public WeaponBase.weaponClassTypes currentClass;
    public CharacterBase character;
    masterInput input;
    classAbilties abilities;

    [SerializedDictionary("buffName", "buffVal(int)")]
    public SerializedDictionary<string, int> intBuffVals;

    // Start is called before the first frame update
    void Start()
    {
        //ResetRunes();
        abilities = classAbilties.instance;
    }

    void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        input = GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>();
        
        //runeInventory = runeManager.GetComponent<RuneInventory>();

        //weaponsInventory = weaponsManager.GetComponent<WeaponsInventory>();
        //weapon = character.equippedWeapon;
        ResetRunes();
        foreach(var rune in character.equippedRunes)
        {
            print("Equipped Runes: " + rune);
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

    public void ResetRunes()
    {
        Remove();
        Apply();
    }

    public void ChangeClass(WeaponBase.weaponClassTypes newClass)
    {
        Debug.Log("Rune class before change: " + currentClass);
        currentClass = newClass;
        ResetRunes();
    }

    public void Apply()
    {
        //LOGIC FOR THE RUNES GOES HERE
        for(int i = 0; i < character.equippedRunes.Length; i++)
        {
            if (character.equippedRunes[i] != null)
            {
                switch (character.equippedRunes[i].runeType)
                {
                    case Rune.RuneType.Buff:

                        applyBuffRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Class:
                        print("class rune detected");
                        applyClassRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Spell:

                        applySpellRunes(character.equippedRunes[i]);
                        break;

                }
            }
            else
            {
                break;
            }
        }

    }

    public void applyBuffRunes(Rune rune)
    {
        if(rune.runeName == "Defense")
        {
            character.changeDefenseStat(getIntVal("defenseRuneBuff"));
        }
        if (rune.runeName == "Regen")
        {
            Debug.Log("Applying Regen");
            StartCoroutine(ApplyRegen());
        }
    }
    public void applyDefenseRunes(Rune rune)
    {

    }
    public void applyClassRunes(Rune rune)
    {
        if(rune.runeName == "Fire")
        {
            Debug.Log(abilities);
            abilities.activateFireRune(true);
        }
        if(rune.runeName == "Ice")
        {
            abilities.activateIceRune(true);
        }
        if (rune.runeName == "Earth")
        {
            abilities.activateEarthRune(true);
        }
    }

    public void applySpellRunes(Rune rune)
    {

    }


    public void removeBuffRunes(Rune rune)
    {
        if (rune.runeName == "Defense")
        {
            character.changeDefenseStat(-getIntVal("defenseRuneBuff"));
        }
    }

    public void removeDefenseRunes(Rune rune)
    {

    }

    public void removeClassRunes(Rune rune)
    {
        if(rune.name == "Fire")
        {
            abilities.activateFireRune(false);
        }
        if (rune.runeName == "Ice")
        {
            abilities.activateIceRune(false);
        }
        if (rune.runeName == "Earth")
        {
            abilities.activateEarthRune(false);
        }
    }

    public void removeSpellRunes(Rune rune)
    {

    }

    public void removeHealthRunes(Rune rune)
    {
        if (rune.runeName == "Regen")
        {
            StopCoroutine(ApplyRegen());
        }
    }


    public void Remove()
    {
        //LOGIC FOR THE RUNES GOES HERE
        for (int i = 0; i < character.equippedRunes.Length; i++)
        {
            if (character.equippedRunes[i] != null)
            {
                switch (character.equippedRunes[i].runeType)
                {
                    case Rune.RuneType.Buff:

                        removeBuffRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Class:

                        removeClassRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Spell:

                        removeSpellRunes(character.equippedRunes[i]);
                        break;

                }
            }
            else
            {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //RUNE LOGIC FOR RUNES THAT APPLY PER FRAME OR ON TIMERS.
    }

    IEnumerator ApplyRegen()
    {
        while (true)
        {
            character.restoreHealth(5);
            yield return new WaitForSeconds(5f);
        }
        
    }
}
