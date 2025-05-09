using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using static StoreItem;
using JetBrains.Annotations;
using System;
//using static UnityEditor.Progress;

public class CraftRecipePrefab : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject materialRequirementObject;
    [SerializeField] public CraftRecipe craftRecipe;
    [SerializeField] GameObject materialContainer;
    [SerializeField] GameObject craftRecipeName;
    [SerializeField] GameObject craftRecipeShadow;
    [SerializeField] public GameObject craftButton;
    [SerializeField] GameObject disabledPanel;
    List<GameObject> currentMaterialObjects = new List<GameObject>();
    AudioManager audioManager;

    
    public static ResetDelegateTemplate.ResetDelegate reset;




    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Awake()
    {
        //craftButton.GetComponent<Button>().interactable = false;
        craftButton.SetActive(false);
        disabledPanel.SetActive(true);
        craftRecipeName.GetComponent<TMP_Text>().text = craftRecipe.recipeName;
        craftRecipeShadow.GetComponent<TMP_Text>().text = craftRecipe.recipeName;
        populateMaterialsList();
    }

    public void ResetSelection()
    {
        GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().ResetSelection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onView()
    {
        audioManager.PlaySFX("UIConfirm");
        reset = ResetSelection;
        if (craftRecipe.type == CraftRecipe.CraftTypes.Weapon)
        {
            var weaponToView = GameObject.Find("WeaponsList").GetComponent<WeaponsList>().ReturnWeapon(craftRecipe.recipeName);
            GameObject.Find("UIManager")
                .GetComponent<UIManager>().DisplayViewItem(ViewItemPrefab.ViewType.Weapon,reset, weaponToView);
        }

        else if (craftRecipe.type == CraftRecipe.CraftTypes.Rune)
        {
            var runeToView = GameObject.Find("RunesList").GetComponent<RuneList>().ReturnRune(craftRecipe.recipeName);
            GameObject.Find("UIManager")
                .GetComponent<UIManager>().DisplayViewItem(ViewItemPrefab.ViewType.Rune, reset, null, runeToView);
        }

        else if (craftRecipe.type == CraftRecipe.CraftTypes.Item)
        {
            var itemToView = GameObject.Find("ItemsList").GetComponent<ItemsList>().ReturnItem(craftRecipe.recipeName);
            GameObject.Find("UIManager")
                .GetComponent<UIManager>().DisplayViewItem(ViewItemPrefab.ViewType.Item, reset, null, null, itemToView);
        }
    }

    void populateMaterialsList()
    {
        bool isCraftable = true;
        foreach (var item in craftRecipe.requiredMaterials)
        {
            var matManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
            var wrapComponent = materialRequirementObject.GetComponent<materialRequirementsWrapper>();
            


            int curAmount = matManager.GetMaterialAmount(item.Key);
            wrapComponent.materialDescription.GetComponent<TMP_Text>().text = item.Key.materialName;
            //wrapComponent.requiredAmount.GetComponent<TMP_Text>().text = item.Value.ToString();
            wrapComponent.amountHas.GetComponent<TMP_Text>().text = curAmount.ToString() + "/" + item.Value.ToString();
            wrapComponent.materialTexture.GetComponent<RawImage>().texture = item.Key.materialTexture;
            Color32 red = new Color32(0xE4, 0x3c, 0x54, 0xFF);
            Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            if (curAmount < item.Value) wrapComponent.amountHas.GetComponent<TMP_Text>().color = red;
            else wrapComponent.amountHas.GetComponent<TMP_Text>().color = white;

            

            if (curAmount < item.Value)
            {
                isCraftable = false;
            }


            var matRef = Instantiate(materialRequirementObject);
            currentMaterialObjects.Add(matRef);
            matRef.transform.SetParent(materialContainer.transform);
            matRef.transform.localScale = Vector3.one;

            
        }
        if (isCraftable)
        {
            //craftButton.GetComponent<Button>().interactable = true;
            craftButton.SetActive(true);
            disabledPanel.SetActive(false);

        }

    }
    public void AddToInventory()
    {
        audioManager.PlaySFX("UIConfirm");
        int index = 0;
        bool hasEnoughMat = true;
        foreach (var item in craftRecipe.requiredMaterials)
        {
            var matManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
            matManager.RemoveFromMaterialsInventory(item.Key, item.Value);
            if(UpdateMaterialsList(item.Key, item.Value, index) == false)
            {
                hasEnoughMat = false;
            }
            index++;
        }
        if(hasEnoughMat)
        {
            //craftButton.GetComponent<Button>().interactable = true;
            craftButton.SetActive(true);
            disabledPanel.SetActive(false);
        }
        else
        {
            //craftButton.GetComponent<Button>().interactable = false;
            craftButton.SetActive(false);
            disabledPanel.SetActive(true);
        }

        

        switch (craftRecipe.type)
        {
            case CraftRecipe.CraftTypes.Weapon:
                AddToWeaponsInventory();
                craftRecipe.hasCrafted = true;
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().DestroyRecipe(craftRecipe, craftRecipe.type);
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().ResetSelection();

                break;
            case CraftRecipe.CraftTypes.Rune:
                AddToRunesInventory();
                craftRecipe.hasCrafted = true;
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().DestroyRecipe(craftRecipe, craftRecipe.type);
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().ResetSelection();
                break;
            case CraftRecipe.CraftTypes.Item:
                AddToItemsInventory();
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().ResetSelection();
                break;
        }
    }

    public bool UpdateMaterialsList(CraftMaterial key, int requiredAmount, int index)
    {
        bool hasEnough = true;
        var matManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
        int curAmount = matManager.GetMaterialAmount(key);
        if(curAmount < requiredAmount)
        {
            hasEnough = false;
        }
        //currentMaterialObjects[index].GetComponent<materialRequirementsWrapper>().amountHas.GetComponent<TMP_Text>().text =  curAmount.ToString();
        currentMaterialObjects[index].GetComponent<materialRequirementsWrapper>().amountHas.GetComponent<TMP_Text>().text = curAmount.ToString() + "/" + requiredAmount.ToString();
        Color32 red = new Color32(0xE4, 0x3c, 0x54, 0xFF);
        Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        if (!hasEnough) currentMaterialObjects[index].GetComponent<materialRequirementsWrapper>().amountHas.GetComponent<TMP_Text>().color = red;
        else currentMaterialObjects[index].GetComponent<materialRequirementsWrapper>().amountHas.GetComponent<TMP_Text>().color = white;
        return hasEnough;
    }

    public void AddToWeaponsInventory()
    {
        Debug.Log("Adding to weapons inventory");
        GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().FindWeaponAndAdd(craftRecipeName.GetComponent<TMP_Text>().text);
    }

    public void AddToItemsInventory()
    {
        Debug.Log("Adding to items inventory");
        GameObject.Find("ItemManager").GetComponent<ItemManager>().FindItemAndAdd(craftRecipeName.GetComponent<TMP_Text>().text);
    }

    public void AddToRunesInventory()
    {
        Debug.Log("Adding to runes inventory");
        GameObject.Find("RuneManager").GetComponent<RuneManager>().FindRuneAndAdd(craftRecipeName.GetComponent<TMP_Text>().text);
    }
}
