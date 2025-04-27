using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipOptionPrefab : MonoBehaviour
{
    public EquipTypes type;
    public WeaponBase weapon;
    public WeaponClass weaponClass;
    public Rune rune;
    AudioManager audioManager;

    [SerializeField] public GameObject equipOptionName;
    [SerializeField] public GameObject image;
    [SerializeField] public GameObject equipOptionEquipText;
    [SerializeField] public GameObject equipOptionDescription;
    [SerializeField] public GameObject equipOptionButton;
    [SerializeField] public GameObject unequipOptionButton;
    [SerializeField] public GameObject equipOptionEffect;
    [SerializeField] public GameObject equipOptionDamage;
    [SerializeField] public GameObject equipOptionClassUI;
    [SerializeField] public GameObject equipOptionRuneUI;
    [SerializeField] public GameObject disabledPanel;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Equip()
    {
        audioManager.PlaySFX("UIConfirm");
        switch (type)
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



    public void Unequip()
    {
        audioManager.PlaySFX("UIBack");
        switch (type){
            case EquipTypes.Rune:
                UnequipRune();
                break;
        }
        GameObject.FindGameObjectWithTag("EquipMenu").GetComponent<EquipMenuTransition>().ResetMenu();

    }

    public void EquipWeapon()
    {
        Debug.Log("Equipping: " + weapon.weaponName);
        GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().ChangeWeapon(weapon);
        GameObject.FindGameObjectWithTag("EquipMenu").GetComponent<EquipMenuTransition>().ResetMenu();
    }

    public void EquipClass()
    {

    }

    public void EquipRune()
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        var equippedRunes = character.equippedRunes;
        int validCounter = 0;
        for(int i = 0; i < equippedRunes.Length; i++)
        {
            if (equippedRunes[i] != null) validCounter++;
            else break;
        }
        if (validCounter > 2)
        {
            DisplayRuneSwapMenu();
        }
        else
        {
            GameObject.Find("RuneManager").GetComponent<RuneManager>().ChangeRunes(rune, validCounter);
            GameObject.FindGameObjectWithTag("EquipMenu").GetComponent<EquipMenuTransition>().ResetMenu();
        }
        
    }

    public void DisplayRuneSwapMenu()
    {
        GameObject.FindGameObjectWithTag("EquipMenu").GetComponent<EquipMenuTransition>().navigateToRuneSwapMenu(rune);
    }

    public void UnequipRune()
    {
        GameObject.Find("RuneManager").GetComponent<RuneManager>().UnequipRune(rune);
    }

    public enum EquipTypes
    {
        Class,
        Weapon,
        Rune
    }
}
