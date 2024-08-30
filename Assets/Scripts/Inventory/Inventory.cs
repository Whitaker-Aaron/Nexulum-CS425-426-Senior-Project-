using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Makes the class STATIC
    //This class will need to interact with almost every aspect of the game,
    //so it needs to be static
    public static Inventory instance { get; set; }
    public GameObject[] inventoryItems;
    public InventoryManager inventoryMan;
    private void Awake()
    {
        instance = this;
    }

    public void AddItem(GameObject item, int amount)
    {
        //if(GetItemCount(item.GetComponent<Item_Script>().obj) > 0)
        {

        }

    }
    public void GetItemCount()
    {

    }
}
