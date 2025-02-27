using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMenuTransition : MonoBehaviour
{
    [SerializeField] GameObject itemOptionPrefab;
    public GameObject itemsScrollContent;
    ScrollRect itemsScrollRect;
    public GameObject backButton;
    List<GameObject> currentItemScrollObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        itemsScrollContent = GameObject.Find("ItemsScrollContent");
        var itemsScroll = GameObject.Find("ItemsScroll");
        itemsScrollRect = itemsScroll.GetComponent<ScrollRect>();
        populateItemsScroll();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name == "ItemOption(Clone)")
        {
            var selectedItem = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent;
            RectTransform selectedItemRect = selectedItem.GetComponent<RectTransform>();


            var itemContentPanel = itemsScrollContent.GetComponent<RectTransform>();
            Vector2 newItemPos = (Vector2)itemsScrollRect.transform.InverseTransformPoint(itemContentPanel.position)
            - (Vector2)itemsScrollRect.transform.InverseTransformPoint(selectedItemRect.position);
            float newItemPosY = (float)newItemPos.y;
            itemContentPanel.anchoredPosition = new Vector2(itemContentPanel.anchoredPosition.x, newItemPosY - 150f);
            
        }
    }

    public void NavigateToMaterialMenu()
    {
        Debug.Log("Back Button pressed");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToMaterialMenu();
    }

    public void populateItemsScroll()
    {
        var itemsInventory = GameObject.Find("ItemManager").GetComponent<ItemManager>().GetInventory();
        foreach (var item in itemsInventory)
        {
            if(item != null)
            {
                itemOptionPrefab.GetComponent<PlayerItemPrefab>().item = item;
                var itemRef = Instantiate(itemOptionPrefab);
                itemRef.transform.SetParent(itemsScrollContent.transform, false);
            }
            
        }
    }
}
