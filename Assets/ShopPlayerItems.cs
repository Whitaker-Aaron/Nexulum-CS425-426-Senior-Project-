using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlayerItems : MonoBehaviour
{
    [SerializeField] List<PlayerItem> itemsInStore = new List<PlayerItem>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<PlayerItem> getItems()
    {
        return itemsInStore;
    }

    public void removeItem(PlayerItem itemToRemove)
    {
        for (int i = 0; i < itemsInStore.Count; i++)
        {
            if (itemsInStore[i].itemName == itemToRemove.itemName)
            {
                itemsInStore.RemoveAt(i);
                break;
            }
        }
    }
}
