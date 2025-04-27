using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerItemPrefab : MonoBehaviour
{
    public PlayerItem item;
    AudioManager audioManager;

    [SerializeField] public GameObject playerItemName;
    [SerializeField] public GameObject playerOptionDescription;
    [SerializeField] public GameObject itemUseButton;
    [SerializeField] public GameObject itemAmount;
    [SerializeField] public GameObject disabledPanel;

    // Start is called before the first frame update
    void Start()
    {
        playerItemName.GetComponent<TMP_Text>().text = item.itemName;
        playerOptionDescription.GetComponent<TMP_Text>().text = item.itemDescription;
        itemAmount.GetComponent<TMP_Text>().text = "x" + (item.itemAmount).ToString();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (!item.canUseFromMenu)
        {
            itemUseButton.GetComponent<Button>().interactable = false;
            disabledPanel.SetActive(true);

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use()
    {
        audioManager.PlaySFX("UIConfirm");
        GameObject.Find("ItemManager").GetComponent<ItemManager>().ExecuteItemLogic(item);

    }

}
