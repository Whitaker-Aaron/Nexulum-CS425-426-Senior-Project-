using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    // Start is called before the first frame update
    WeaponsInventory weaponInventory;
    CharacterBase characterReference;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeWeapon(WeaponBase newWeapon)
    {
        characterReference.GetWeaponClass().currentWeapon = newWeapon;
    }
}
