using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestMaterialDeposit : MonoBehaviour
{
    MenuManager menuManager;
    [SerializeField] public List<GameObject> itemDisplays;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackButton()
    {
        menuManager.closeChestMenu();
    }

    public void OnTakeAll()
    {
        int items = itemDisplays.Count;
        for(int i =0; i < items; i++)
        {
            if (itemDisplays[i] != null && itemDisplays[i].activeSelf)
            {
                Debug.Log("Item found at index " +  i);
                var item = itemDisplays[i].GetComponent<ChestWithdrawOption>();
                if (item.chestAmount > 0)
                {
                    item.amountToTake = item.chestAmount;
                    item.TakeFromChest();
                }

            }
            
        }
    }
}
