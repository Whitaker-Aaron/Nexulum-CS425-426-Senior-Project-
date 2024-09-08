using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterBase : MonoBehaviour
{
    //Will remove serialize field later. Here for testing purposes. Will have to be handled by lifetime 
    //managers.

    [SerializeField] public Rune[] equippedRunes;
     WeaponClass weaponClass;
     MaterialsInventory materialInventory;
     RuneInventory runesInventory;
     WeaponsInventory weaponsInventory;
     ItemsInventory itemsInventory;
     [SerializeField] GameObject masterInput;



    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < equippedRunes.Length; i++)
        {
            Debug.Log("Currently equipped with " + equippedRunes[i].name + " rune.");
        }
    
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public WeaponClass GetWeaponClass()
    {
        return weaponClass;
    }

    

    public void ApplyRuneLogicToWeapon()
    {
        for(int i = 0;  i < equippedRunes.Length; i++)
        {
            if (equippedRunes[i].runeType == Rune.RuneType.Weapon)
            {
                //Implement logic to apply rune logic to currently equipped weapon.
            }

        }

    }

    public GameObject GetMasterInput()
    {
        return masterInput;
    }

    
}
