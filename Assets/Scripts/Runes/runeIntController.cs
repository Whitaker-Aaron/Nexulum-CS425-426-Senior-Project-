using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runeIntController : MonoBehaviour, RuneInt
{

    WeaponBase weapon;
    public WeaponBase.weaponClassTypes currentClass;
    public CharacterBase character;

    // Start is called before the first frame update
    void Start()
    {
        //ResetRunes();
    }

    void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        //runeInventory = runeManager.GetComponent<RuneInventory>();

        //weaponsInventory = weaponsManager.GetComponent<WeaponsInventory>();
        //weapon = character.equippedWeapon;

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

                    case Rune.RuneType.Defense:

                        applyDefenseRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Health:

                        applyHealthRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Projectile:

                        applyProjectileRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Weapon:

                        applyWeaponRunes(character.equippedRunes[i]);
                        break;

                }
            }
            else
            {
                break;
            }
        }

    }

    public void applyWeaponRunes(Rune rune)
    {
        if (rune.runeName == "Fire")
        {
            if (currentClass == WeaponBase.weaponClassTypes.Knight)
            {
                character.equippedWeapon.weaponMesh.GetComponent<swordCombat>().activateFire(true);
            }
            else if (currentClass == WeaponBase.weaponClassTypes.Gunner)
            {

            }
            else
            {
                Debug.Log("Error, no playertype found, cant apply rune");
            }
        }
    }
    public void applyHealthRunes(Rune rune)
    {
        if (rune.runeName == "Regen")
        {
            Debug.Log("Applying Regen");
            StartCoroutine(ApplyRegen());
        }
    }
    public void applyBuffRunes(Rune rune)
    {
        

    }
    public void applyDefenseRunes(Rune rune)
    {

    }
    public void applyProjectileRunes(Rune rune)
    {

    }

    public void removeWeaponRunes(Rune rune)
    {
        if (rune.runeName == "Fire")
        {
            if (currentClass == WeaponBase.weaponClassTypes.Knight)
            {
                //weapon.weaponMesh.GetComponent<swordCombat>().activateFire(false);
            }
            else if (currentClass == WeaponBase.weaponClassTypes.Gunner)
            {

            }
            else
            {
                Debug.Log("Error, no playertype found, cant apply rune");
            }
        }
    }

    public void removeBuffRunes(Rune rune)
    {

    }

    public void removeDefenseRunes(Rune rune)
    {

    }

    public void removeProjectileRunes(Rune rune)
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

                    case Rune.RuneType.Defense:

                        removeDefenseRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Health:

                        removeHealthRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Projectile:

                        removeProjectileRunes(character.equippedRunes[i]);
                        break;

                    case Rune.RuneType.Weapon:

                        removeWeaponRunes(character.equippedRunes[i]);
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
