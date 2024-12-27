using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
public class LockedDoorUI : MonoBehaviour
{
    [SerializeField] TMP_Text AmountOfSmallKeyText;
    CharacterBase playerRef;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateKeyAmount(int amount)
    {
        AmountOfSmallKeyText.text = "Have: Small Key x" + amount.ToString();
    }
}
