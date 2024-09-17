using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftRecipePrefab : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject materialRequirementObject;
    [SerializeField] public CraftRecipe craftRecipe;
    [SerializeField] GameObject materialContainer;
    [SerializeField] GameObject craftRecipeName;
    [SerializeField] GameObject craftButton;
    

    void Start()
    {
        craftButton.GetComponent<Button>().interactable = false;
        craftRecipeName.GetComponent<Text>().text = craftRecipe.recipeName;
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
            wrapComponent.materialDescription.GetComponent<Text>().text = item.Key.materialName;
            wrapComponent.requiredAmount.GetComponent<Text>().text = "Need: x" + item.Value;
            wrapComponent.amountHas.GetComponent<Text>().text = "Has: x" + curAmount;
            wrapComponent.materialTexture.GetComponent<RawImage>().texture = item.Key.materialTexture;

            if (curAmount < item.Value)
            {
                isCraftable = false;
            }


            var matRef = Instantiate(materialRequirementObject);
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
        switch (craftRecipe.type)
        {
            case CraftRecipe.CraftTypes.Weapon:
                AddToWeaponsInventory();
                break;
            case CraftRecipe.CraftTypes.Rune:
                AddToRunesInventory();
                break;
            case CraftRecipe.CraftTypes.Item:
                AddToItemsInventory();
                break;
        }
    }

    public void AddToWeaponsInventory()
    {
        Debug.Log("Adding to weapons inventory");
        GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().FindWeaponAndAdd(craftRecipeName.GetComponent<Text>().text);
    }

    public void AddToItemsInventory()
    {
        Debug.Log("Adding to items inventory");
    }

    public void AddToRunesInventory()
    {
        Debug.Log("Adding to runes inventory");
    }
}
