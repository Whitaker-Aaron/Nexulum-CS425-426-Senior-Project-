using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerItemPrefab : MonoBehaviour
{
    public PlayerItem item;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use()
    {
        GameObject.Find("ItemManager").GetComponent<ItemManager>().ExecuteItemLogic(item);

    }

}
