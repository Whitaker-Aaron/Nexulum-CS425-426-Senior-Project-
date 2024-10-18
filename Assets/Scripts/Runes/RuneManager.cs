using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour, SaveSystemInterface
{
    RuneInventory runesInventory;
    CharacterBase characterReference;
    RuneList runeList;
    // Start is called before the first frame update
    void Awake()
    {
        runeList = GameObject.Find("RunesList").GetComponent<RuneList>();
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        runesInventory = GameObject.Find("RuneInventory").GetComponent<RuneInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {

        
        
        //characterReference.equippedRunes
    }

    public void SaveData(ref SaveData data)
    {
        var currentRunes = characterReference.equippedRunes;
        for(int index = 0; index < currentRunes.Length; index++)
        {
            if (currentRunes[index] != null){
                data.equippedRunes[index] = currentRunes[index].runeName;
            }
            
        }
    }
    public void LoadData(SaveData data)
    {
        Debug.Log(characterReference);
        var currentRunes = characterReference.equippedRunes;
        for (int index = 0; index < currentRunes.Length; index++)
        {
            if (data.equippedRunes[index] != null && data.equippedRunes[index] != "")
            {
                currentRunes[index] = runeList.ReturnRune(data.equippedRunes[index]);
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

    public void ChangeRunes(Rune runeToEquip)
    {

        var position = 0;
        var charRunes = characterReference.equippedRunes;
        for (int index = 0; index < charRunes.Length; index++)
        {
            if (charRunes[index] != null)
            {
                position++;
            }
        }
        if(position >= 2)
        {
            position = 0;
        }
        characterReference.UpdateRunes(runeToEquip, position);
    }

    public Rune[] GetRuneInventory()
    {
        return runesInventory.GetComponent<RuneInventory>().GetInventory();
    }
}
