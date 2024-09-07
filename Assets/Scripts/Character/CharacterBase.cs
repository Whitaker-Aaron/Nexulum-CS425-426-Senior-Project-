using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterBase : MonoBehaviour
{
    //Will remove serialize field later. Here for testing purposes. Will have to be handled by lifetime 
    //managers.

    [SerializeField] Rune[] equippedRunes;
    [SerializeField] WeaponClass weaponClass;
    //[SerializeField] MaterialsInventory materialInventory;
    //[SerializeField] RunesInventory runesInventory;
    //[SerializeField] WeaponsInventory weaponsInventory;
    //[SerializeField] ItemsInventory itemsInventory;



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

    
}
