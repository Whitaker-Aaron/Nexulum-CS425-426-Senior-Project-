using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipOptionPrefab : MonoBehaviour
{
    public EquipTypes type;
    public WeaponBase weapon;
    public WeaponClass weaponClass;
    public Rune rune;

    [SerializeField] public GameObject equipOptionName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Equip()
    {
        switch(type)
        {
            case EquipTypes.Weapon:
                EquipWeapon();
                break;
            case EquipTypes.Class:
                EquipClass();
                break;
            case EquipTypes.Rune:
                EquipRune();
                break;

        }
    }

    public void EquipWeapon()
    {
        Debug.Log("Equipping: " + weapon.weaponName);
        GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().ChangeWeapon(weapon);
    }

    public void EquipClass()
    {

    }

    public void EquipRune()
    {

    }

    public enum EquipTypes
    {
        Class,
        Weapon,
        Rune
    }
}
