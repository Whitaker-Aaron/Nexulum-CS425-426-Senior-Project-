using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runeIntController : MonoBehaviour, RuneInt
{

    //private RuneInventory runeInventory;
    Rune equippedRuneOne, equippedRuneTwo;
    WeaponBase weapon;
    //private WeaponsInventory weaponsInventory;
    [SerializeField] public WeaponBase.weaponClassTypes currentClass;
    //[SerializeField] public RuneManager runeManager;
    //[SerializeField] public WeaponsManager weaponsManager;
    public CharacterBase character;
    string runeName, runeNameTwo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        //runeInventory = runeManager.GetComponent<RuneInventory>();
        if (character.equippedRunes.Length >= 1)
        {
            equippedRuneOne = character.equippedRunes[0];
            runeName = equippedRuneOne.runeName;
        }
        if (character.equippedRunes.Length >= 2)
        {
            equippedRuneTwo = character.equippedRunes[1];
            runeNameTwo = equippedRuneTwo.runeName;
        }
        //weaponsInventory = weaponsManager.GetComponent<WeaponsInventory>();
        weapon = character.equippedWeapon;

    }

    public void apply()
    {
        //LOGIC FOR THE RUNES GOES HERE
        if(runeName == "Fire Rune" || runeNameTwo == "Fire Rune")
        {
            if(currentClass == WeaponBase.weaponClassTypes.Knight)
            {
                weapon.weaponMesh.GetComponent<swordCombat>().activateFire(true);
            }
            else if(currentClass == WeaponBase.weaponClassTypes.Gunner)
            {

            }
            else
            {
                Debug.Log("Error, no playertype found, cant apply rune");
            }
        }
    }

    public void remove()
    {
        if (runeName == "Fire Rune")
        {
            if (currentClass == WeaponBase.weaponClassTypes.Knight)
            {
                weapon.weaponMesh.GetComponent<swordCombat>().activateFire(false);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
