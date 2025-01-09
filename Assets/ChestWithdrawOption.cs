using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestWithdrawOption : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public TMP_Text itemName;
    [SerializeField] public RawImage itemTexture;
    [SerializeField] public TMP_Text itemAmount;
    [SerializeField] public TMP_Text amountInChest;
    public int chestAmount;
    public int amountToTake;
    public int itemIndex;
    public CraftMaterial item;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeFromChest()
    {
        var scrollManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
        
        if(amountToTake > 0 ) scrollManager.AddToMaterialsInventory(item, amountToTake);
        //scrollManager.RemoveFromTotalMaterialsInventory(attachedMaterial, materialDepositCount);
        GameObject.Find("MenuManager").GetComponent<MenuManager>().updateChestMenuMaterials();
        chestAmount -= amountToTake;
        if(chestAmount <= 0) this.gameObject.SetActive(false);
        else
        {
            itemAmount.text = "x" + chestAmount.ToString();
            amountInChest.text = "0";
        }
        GameObject.Find("MenuManager").GetComponent<MenuManager>().UpdateChest(itemIndex, amountToTake);
        amountToTake = 0;


    }

    public void OnPlusButton()
    {
        //int val = 0;
        //int.TryParse(itemAmount.text, out val);
        if (amountToTake + 1 <= chestAmount) amountToTake++;
        amountInChest.text = amountToTake.ToString();

    }

    public void OnMinusButton()
    {
        if (amountToTake - 1 >= 0) amountToTake--;
        amountInChest.text = amountToTake.ToString();
    }
}
