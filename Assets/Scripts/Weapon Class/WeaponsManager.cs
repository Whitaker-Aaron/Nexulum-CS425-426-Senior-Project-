using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    // Start is called before the first frame update
    WeaponsInventory weaponInventory;
    GameObject weaponPrefab;
    CharacterBase characterReference;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        //weaponInventory = GetComponent<WeaponsInventory>();
        weaponPrefab = characterReference.equippedWeapon.weaponMesh;
        Instantiate(weaponPrefab, characterReference.hand);
    }

    private void Awake()
    {
        
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
