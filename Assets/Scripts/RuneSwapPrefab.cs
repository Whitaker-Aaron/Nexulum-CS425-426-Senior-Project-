using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSwapPrefab : MonoBehaviour
{
    public Rune rune;
    public Rune runeToEquip;
    public int index;

    [SerializeField] public GameObject swapName;
    [SerializeField] public GameObject swapText;
    [SerializeField] public GameObject swapButton;
    [SerializeField] public GameObject swapEffect;



    public void Swap()
    {
        UnequipRune();
        EquipRune();
    }

    public void EquipRune()
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

        GameObject.Find("RuneManager").GetComponent<RuneManager>().ChangeRunes(runeToEquip, index);
        GameObject.FindGameObjectWithTag("EquipMenu").GetComponent<EquipMenuTransition>().ResetMenu();
        

    }

    public void UnequipRune()
    {
        GameObject.Find("RuneManager").GetComponent<RuneManager>().UnequipRune(rune);
    }

}
