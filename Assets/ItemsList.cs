using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsList : MonoBehaviour
{
        [SerializedDictionary("ItemName", "Item")]
        public SerializedDictionary<string, PlayerItem> itemLookup;

        public PlayerItem ReturnItem(string itemName)
        {
            return itemLookup[itemName];
        }

    
}
