using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    RuneInventory runesInventory;
    CharacterBase characterReference;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(Rune runeToAdd)
    {
        runesInventory.GetComponent<RuneInventory>().AddToInventory(runeToAdd);
    }

    public bool FindRuneAndAdd(string runeName)
    {
        var runes = GameObject.Find("itemsEquipList").GetComponent<RuneEquipList>().allRunes;
        if (runes != null && runes.Count > 0)
        {
            Debug.Log("Inside Weapons Manager if statement");
            foreach (var rune in runes)
            {
                if (runeName == rune.runeName)
                {
                    AddToInventory(rune);
                    return true;
                }
            }
        }
        return false;

    }

    public void ChangeRunes(Rune runeToEquip, int position)
    {
        characterReference.equippedRunes[position] = runeToEquip;
        characterReference.ApplyRuneLogicToWeapon();
    }
}
