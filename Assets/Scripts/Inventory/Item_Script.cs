using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Item_Script : MonoBehaviour
{
    public Item itemObject;
    public Item heldProperties;

    public TMP_Text itemName;
    public RawImage itemTexture;
    public void SetCurrentItem(Item item)
    {
        if (heldProperties == null)
        {
            SetHeldProperties(item);
        }

        itemName.text = heldProperties.name;
        itemTexture.texture = heldProperties.itemTexture;
    }

    public void SetHeldProperties(Item item)
    {
        heldProperties = ScriptableObject.CreateInstance<Item>();
        heldProperties.itemName = item.itemName;
        heldProperties.itemTexture = item.itemTexture;
    }

}
