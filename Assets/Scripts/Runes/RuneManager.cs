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

    public void ChangeRunes(Rune runeToEquip, int position)
    {
        characterReference.equippedRunes[position] = runeToEquip;
        characterReference.ApplyRuneLogicToWeapon();
    }
}
