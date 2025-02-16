using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject materialRequirementObject;
    [SerializeField] public CraftRecipe craftRecipe;
    [SerializeField] public WeaponBase weaponBase;
    [SerializeField] public PlayerItem playerItem;
    [SerializeField] GameObject materialContainer;
    [SerializeField] public GameObject storeItemName;
    [SerializeField] public GameObject storeItemNameShadow;
    [SerializeField] public GameObject florentineRequiredAmount;
    [SerializeField] public GameObject purchaseButton;
    [SerializeField] public GameObject viewButton;
    [SerializeField] public GameObject disabledPanel;
    [SerializeField] public GameObject itemQuantity;
    [SerializeField] public GameObject requiredMaterials;
    [SerializeField] public GameObject description;
    public StoreItemType storeType;
    MenuManager menuManager;

    List<GameObject> currentMaterialObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        if (storeType != StoreItemType.Item) itemQuantity.SetActive(false);
        if (storeType != StoreItemType.Recipe)
        {
            requiredMaterials.SetActive(false);
            viewButton.SetActive(false);
        }
        if (storeType == StoreItemType.Recipe && craftRecipe != null)
        {
            description.SetActive(false);
            populateMaterialsList();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void populateMaterialsList()
    {
        foreach (var item in craftRecipe.requiredMaterials)
        {
            var matManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
            var wrapComponent = materialRequirementObject.GetComponent<materialRequirementsWrapper>();

            int curAmount = matManager.GetMaterialAmount(item.Key);
            wrapComponent.materialDescription.GetComponent<TMP_Text>().text = item.Key.materialName;
            wrapComponent.amountHas.GetComponent<TMP_Text>().text = "x" + item.Value.ToString();
            wrapComponent.materialTexture.GetComponent<RawImage>().texture = item.Key.materialTexture;
            Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            wrapComponent.amountHas.GetComponent<TMP_Text>().color = white;

            var matRef = Instantiate(materialRequirementObject);
            currentMaterialObjects.Add(matRef);
            matRef.transform.SetParent(materialContainer.transform);
            matRef.transform.localScale = Vector3.one;


        }

    }

    public void onPurchase()
    {
        switch (storeType)
        {
            case StoreItemType.Recipe:
                Debug.Log("Purchasing recipe");
                menuManager.AddToAvailableCraftRecipes(craftRecipe);
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().RemoveFlorentine(craftRecipe.shopCost);
                menuManager.RemoveShopItemFromShop(this.gameObject);
                menuManager.updateShopCount(craftRecipe.shopCost);
                menuManager.resetShopSelection();
                break;
            case StoreItemType.Weapon:
                Debug.Log("Purchasing weapon");
                GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().AddToInventory(weaponBase);
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().RemoveFlorentine(weaponBase.shopCost);
                menuManager.RemoveShopItemFromShop(this.gameObject);
                menuManager.updateShopCount(weaponBase.shopCost);
                menuManager.resetShopSelection();
                break;
            case StoreItemType.Item:
                Debug.Log("Purchasing item");
                GameObject.Find("ItemManager").GetComponent<ItemManager>().AddToInventory(playerItem, 1);
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().RemoveFlorentine(playerItem.shopCost);
                menuManager.updateShopCount(playerItem.shopCost);
                menuManager.resetShopSelection();
                
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
