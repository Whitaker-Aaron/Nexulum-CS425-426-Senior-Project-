using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
    
{
    [SerializeField] GameObject materialsMenuReference;
    [SerializeField] GameObject craftMenuReference;
    [SerializeField] GameObject itemsMenuReference;
    [SerializeField] GameObject equipMenuReference;
    [SerializeField] GameObject scrollContent;
    [SerializeField] GameObject craftListsReference;

    List<GameObject> currentMaterials = new List<GameObject>();

    GameObject currentMenuObject;
    GameObject canvas;
    GameObject materialManager;

    bool menuActive = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("UI");
        materialManager = GameObject.FindGameObjectWithTag("ScrollManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openMenu(InputAction.CallbackContext context)
    {
        if(!menuActive) {
            currentMenuObject = Instantiate(materialsMenuReference);
            Debug.Log("activating main menu");
            currentMenuObject.transform.SetParent(canvas.transform);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            populateInventoryMaterials();
            menuActive = true;
        }
        else
        {
            //currentMenuObject.SetActive(false);
            if(GameObject.FindGameObjectWithTag("CraftLists") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("CraftLists"));
            }
            if (GameObject.FindGameObjectWithTag("MainMenu") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("MainMenu"));
                
            }
            if (GameObject.FindGameObjectWithTag("EquipMenu") != null)
            {
   
                Destroy(GameObject.FindGameObjectWithTag("EquipMenu"));
            }
            if (GameObject.FindGameObjectWithTag("CraftMenu") != null)
            {

                Destroy(GameObject.FindGameObjectWithTag("CraftMenu"));
            }

            menuActive = false;
        }

    }

    public List<CraftRecipe> returnWeaponsCraftList()
    {
        return GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<WeaponCraftList>().allRecipes;
        
    }

    public List<CraftRecipe> returnItemsCraftList()
    {
        return GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<ItemsCraftList>().allRecipes;
    }

    public List<CraftRecipe> returnRunesCraftList()
    {
        return GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<RunesCraftList>().allRecipes;
    }

    public void navigateToMaterialMenu()
    {
        if(menuActive) {
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(materialsMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            populateInventoryMaterials();
        }
    }

    public void navigateToCraftMenu()
    {
        
        if (menuActive)
        {
            Debug.Log("Navigating to Craft Menu");
            Instantiate(craftListsReference);
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(craftMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }

    public void navigateToItemsMenu()
    {
        if (menuActive)
        {
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(itemsMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }


    public void navigateToEquipMenu()
    {
        if (menuActive)
        {
            Debug.Log("Navigating to Equip Menu");
            Instantiate(craftListsReference);
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(equipMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }




    public void AddToCurrentInventory(CraftMaterial materialToAdd)
    {
        var scrollObject = scrollContent.GetComponent<MaterialScrollObject>();
        int existingIndex = -1;
        bool exists = false;

        scrollObject.description.text = materialToAdd.materialName;
        scrollObject.imageRef.texture = materialToAdd.materialTexture;
        scrollObject.quantityInt = 1;
        scrollObject.quantity.text = "x" + scrollObject.quantityInt.ToString();

        for (int i = 0; i < currentMaterials.Count; i++)
        {
            if (currentMaterials[i].GetComponent<MaterialScrollObject>().description.text == materialToAdd.materialName)
            {
                exists = true;
                existingIndex = i;
            }
        }

        if (exists)
        {
            Debug.Log("Current material found");
            var updateMat = currentMaterials[existingIndex].GetComponent<MaterialScrollObject>();
            updateMat.quantityInt += 1;
            updateMat.quantity.text = "x" + updateMat.quantityInt.ToString();
        }
        else
        {
            var matManager = materialManager.GetComponent<MaterialScrollManager>();
            if (matManager.GetMaterialInventorySize() < matManager.GetMaterialInventoryMaxSize())
            {
                var container = currentMenuObject.GetComponentInChildren<GridLayoutGroup>();
                GameObject newScrollMaterial = Instantiate(scrollContent);
                newScrollMaterial.transform.SetParent(container.transform);
                currentMaterials.Add(newScrollMaterial);
            }
            
        }

    }

    public void populateInventoryMaterials()
    {
        CraftMaterial[] matInventory = materialManager.GetComponent<MaterialScrollManager>().GetMaterialInventory();
        
        if (currentMenuObject !=  null) {
            var container = currentMenuObject.GetComponentInChildren<GridLayoutGroup>();
            for (int i = 0; i < matInventory.Length; i++) {

                if (matInventory[i] == null) {
                    break;
                }

                var scrollObject = scrollContent.GetComponent<MaterialScrollObject>();
                scrollObject.description.text = matInventory[i].materialName;
                scrollObject.imageRef.texture = matInventory[i].materialTexture;
                scrollObject.quantityInt = matInventory[i].currentAmount;
                scrollObject.quantity.text = "x" + scrollObject.quantityInt.ToString();
                GameObject newScrollMaterial = Instantiate(scrollContent);
                newScrollMaterial.transform.SetParent(container.transform);
                currentMaterials.Add(newScrollMaterial);
            }
        }



    }
}
