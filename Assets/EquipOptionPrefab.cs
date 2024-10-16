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
    [SerializeField] public GameObject equipOptionEquipText;
    [SerializeField] public GameObject equipOptionDescription;
    [SerializeField] public GameObject equipOptionButton;
    [SerializeField] public GameObject equipOptionEffect;

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
        GameObject.FindGameObjectWithTag("EquipMenu").GetComponent<EquipMenuTransition>().ResetMenu();
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
        GameObject.Find("RuneManager").GetComponent<RuneManager>().ChangeRunes(rune);
    }

    public enum EquipTypes
    {
        Class,
        Weapon,
        Rune
    }
}
