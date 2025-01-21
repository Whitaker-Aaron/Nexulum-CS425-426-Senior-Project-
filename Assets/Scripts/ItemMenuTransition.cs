using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMenuTransition : MonoBehaviour
{
    [SerializeField] GameObject itemOptionPrefab;
    public GameObject itemsScrollContent;
    public GameObject backButton;
    List<GameObject> currentItemScrollObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        itemsScrollContent = GameObject.Find("ItemsScrollContent");
        populateItemsScroll();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    // Update is called once per frame
    void Update()
    {
        
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
