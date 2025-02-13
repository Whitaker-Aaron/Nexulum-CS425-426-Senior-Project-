using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject materialRequirementObject;
    [SerializeField] public CraftRecipe craftRecipe;
    [SerializeField] GameObject materialContainer;
    [SerializeField] public GameObject storeItemName;
    [SerializeField] public GameObject storeItemNameShadow;
    [SerializeField] public GameObject florentineRequiredAmount;
    [SerializeField] public GameObject purchaseButton;
    [SerializeField] public GameObject disabledPanel;
    public StoreItemType storeType;
    MenuManager menuManager;

    List<GameObject> currentMaterialObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPurchase()
    {
        switch (storeType)
        {
            case StoreItemType.Recipe:
                Debug.Log("Purchasing recipe");
                menuManager.AddToAvailableCraftRecipes(craftRecipe);
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().RemoveFlorentine(craftRecipe.shopCost);
                break;
        }
    }

    public enum StoreItemType
    {
        Recipe,
        Item,
        Weapon
    }
}
