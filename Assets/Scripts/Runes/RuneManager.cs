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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {

        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        runesInventory = GameObject.Find("RuneInventory").GetComponent<RuneInventory>();
        //characterReference.equippedRunes
        for (int i = 0; i < characterReference.equippedRunes.Length; i++)
        {
            if (characterReference.equippedRunes[i] != null)
            {
                Debug.Log(characterReference.equippedRunes[i].runeName);
                AddToInventory(characterReference.equippedRunes[i]);
            }
            else
            {
                break;
            }

        }
    }

    public void AddToInventory(Rune runeToAdd)
    {
        runesInventory.GetComponent<RuneInventory>().AddToInventory(runeToAdd);
    }

    public bool FindRuneAndAdd(string runeName)
    {
        var runes = GameObject.Find("runeEquipList").GetComponent<RuneEquipList>().allRunes;
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
        
        characterReference.UpdateRunes(runeToEquip, position);
    }

    public Rune[] GetRuneInventory()
    {
        return runesInventory.GetComponent<RuneInventory>().GetInventory();
    }
}
