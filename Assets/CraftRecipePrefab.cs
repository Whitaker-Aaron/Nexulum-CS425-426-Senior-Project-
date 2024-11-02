using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
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
    List<GameObject> currentMaterialObjects = new List<GameObject>();


    

    void Start()
    {
        
    }

    private void Awake()
    {
        craftButton.GetComponent<Button>().interactable = false;
        craftRecipeName.GetComponent<TMP_Text>().text = craftRecipe.recipeName;
        craftRecipeShadow.GetComponent<TMP_Text>().text = craftRecipe.recipeName;
        populateMaterialsList();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            wrapComponent.requiredAmount.GetComponent<TMP_Text>().text = item.Value.ToString();
            wrapComponent.amountHas.GetComponent<TMP_Text>().text = curAmount.ToString();
            wrapComponent.materialTexture.GetComponent<RawImage>().texture = item.Key.materialTexture;

            

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
            craftButton.GetComponent<Button>().interactable = true;

        }

    }
    public void AddToInventory()
    {
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
            craftButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            craftButton.GetComponent<Button>().interactable = false;
        }
        

        switch (craftRecipe.type)
        {
            case CraftRecipe.CraftTypes.Weapon:
                AddToWeaponsInventory();
                craftRecipe.hasCrafted = true;
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().DestroyRecipe(craftRecipe, craftRecipe.type);
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().ResetWeaponCraftSelection();

                break;
            case CraftRecipe.CraftTypes.Rune:
                AddToRunesInventory();
                craftRecipe.hasCrafted = true;
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().DestroyRecipe(craftRecipe, craftRecipe.type);
                GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenuTransition>().ResetRuneCraftSelection();
                break;
            case CraftRecipe.CraftTypes.Item:
                AddToItemsInventory();
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
        currentMaterialObjects[index].GetComponent<materialRequirementsWrapper>().amountHas.GetComponent<TMP_Text>().text =  curAmount.ToString();
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
