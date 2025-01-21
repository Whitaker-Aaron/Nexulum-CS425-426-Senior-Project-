using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BaseMaterialTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject backButton;
    [SerializeField] TMP_Text materialInventoryAmount;
    [SerializeField] TMP_Text baseMaterialInventoryAmount;
    MenuManager menuManager;
    MaterialScrollManager scrollManager;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        scrollManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ReturnToTerminal()
    {
        GameObject.Find("MenuManager").GetComponent<MenuManager>().openTerminalMenu();
    }

    public void ResetSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void OnTakeAll()
    {
        /*int items = itemDisplays.Count;
        for (int i = 0; i < items; i++)
        {
            if (itemDisplays[i] != null && itemDisplays[i].activeSelf)
            {
                Debug.Log("Item found at index " + i);
                var item = itemDisplays[i].GetComponent<ChestWithdrawOption>();
                if (item.chestAmount > 0)
                {
                    item.amountToTake = item.chestAmount;
                    item.TakeFromChest();
                }

            }

        }*/
        var materialInventory = scrollManager.GetMaterialInventory();
        var index = scrollManager.GetMaterialInventoryMaxSize();
        for (int i =0; i < index; i++)
        {
            if (materialInventory[i] == null) continue;
            scrollManager.AddToTotalMaterialsInventory(materialInventory[i], materialInventory[i].currentAmount);
        }
        for (int i = 0; i < index; i++)
        {
            if (materialInventory[i] == null) continue;
            scrollManager.RemoveFromMaterialsInventory(materialInventory[i], materialInventory[i].currentAmount);
            i--;
        }
        GameObject.Find("MenuManager").GetComponent<MenuManager>().updateBaseInventoryMaterials();


    }
}
