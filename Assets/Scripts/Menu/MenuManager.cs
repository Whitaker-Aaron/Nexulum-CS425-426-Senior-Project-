using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
    
{
    [SerializeField] GameObject materialsMenuReference;
    [SerializeField] GameObject totalMaterialMenuReference;
    [SerializeField] GameObject terminalMenuReference;
    [SerializeField] GameObject craftMenuReference;
    [SerializeField] GameObject itemsMenuReference;
    [SerializeField] GameObject equipMenuReference;

    [SerializeField] GameObject scrollContent;
    [SerializeField] GameObject DepositScrollContent;
    [SerializeField] GameObject WithdrawScrollContent;

    [SerializeField] GameObject craftListsReference;
    [SerializeField] GameObject pauseMenuReference;

    List<GameObject> currentMaterials = new List<GameObject>();
    List<GameObject> currentBaseMaterials = new List<GameObject>();

    GameObject currentMenuObject;
    GameObject canvas;
    GameObject materialManager;
    masterInput inputManager;
    CharacterBase character;

    bool menuActive = false;
    bool pauseMenuActive = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("UI");
        materialManager = GameObject.FindGameObjectWithTag("ScrollManager");
        inputManager = GameObject.Find("InputandAnimationManager").GetComponent<masterInput>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openTerminalMenu()
    {
        if (menuActive)
        {
            Destroy(currentMenuObject);  
        }
        currentMenuObject = Instantiate(terminalMenuReference);
        currentMenuObject.transform.SetParent(canvas.transform, false);

        inputManager.pausePlayerInput();
        menuActive = true;
    }


    public void openPauseMenu(InputAction.CallbackContext context)
    {


        if (!pauseMenuActive && !character.transitioningRoom && context.performed)
        {

            if (GameObject.FindGameObjectWithTag("CraftLists") != null)
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

            currentMenuObject = Instantiate(pauseMenuReference);
            pauseMenuActive = true;
            Time.timeScale = 0;
            inputManager.pausePlayerInput();
        }
        else if(pauseMenuActive && context.performed)
        {
            closePauseMenu();
        }

        

    }

    

    public void closePauseMenu()
    {
        if(pauseMenuActive)
        {
            pauseMenuActive = false;
            Time.timeScale = 1.0f;
            Destroy(currentMenuObject);
            if (!character.transitioningRoom)
            {
                inputManager.resumePlayerInput();
            }
            

        }

    }

        public void openMenu(InputAction.CallbackContext context)
    {
        
        if(!menuActive && !pauseMenuActive && !character.transitioningRoom && context.performed) {
            if (!character.inRangeOfTerminal)
            {
                currentMenuObject = Instantiate(materialsMenuReference);
                populateInventoryMaterials();
                currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            }
            else
            {
                currentMenuObject = Instantiate(terminalMenuReference);
            }
            
            Debug.Log("activating main menu");
            currentMenuObject.transform.SetParent(canvas.transform, false);
            
            inputManager.pausePlayerInput();
            menuActive = true;
        }
        else if(menuActive && !pauseMenuActive && !character.transitioningRoom && context.performed)
        {
            //currentMenuObject.SetActive(false);
            CloseMenu();
            inputManager.resumePlayerInput();
        }

    }

    public void CloseMenu()
    {
        if (GameObject.FindGameObjectWithTag("CraftLists") != null)
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
            currentMenuObject.transform.SetParent(canvas.transform, false);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            populateInventoryMaterials();
        }
    }


    public void navigateToBaseMaterialsMenu()
    {
        Debug.Log("Navigating to Base Material Menu");
        //Instantiate();
        Destroy(currentMenuObject);
        currentMenuObject = Instantiate(totalMaterialMenuReference);
        currentMenuObject.transform.SetParent(canvas.transform, false);
        populateBaseInventoryMaterials();
        //currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    public void navigateToCraftMenu()
    {
        
        if (menuActive)
        {
            Debug.Log("Navigating to Craft Menu");
            Instantiate(craftListsReference);
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(craftMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform, false);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }

    public void navigateToItemsMenu()
    {
        if (menuActive)
        {
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(itemsMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform, false);
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
            currentMenuObject.transform.SetParent(canvas.transform, false);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }




    public void AddToCurrentInventory(CraftMaterial materialToAdd)
    {
        var scrollObject = scrollContent.GetComponent<MaterialScrollObject>();
        int existingIndex = -1;
        bool exists = false;

        scrollObject.descriptionMain.text = materialToAdd.materialName;
        scrollObject.descriptionSub.text = materialToAdd.materialName;
        scrollObject.imageRef.texture = materialToAdd.materialTexture;
        scrollObject.quantityInt = 1;
        scrollObject.quantityMain.text = "x" + scrollObject.quantityInt.ToString();
        scrollObject.quantitySub.text = "x" + scrollObject.quantityInt.ToString();

        for (int i = 0; i < currentMaterials.Count; i++)
        {
            if (currentMaterials[i].GetComponent<MaterialScrollObject>().descriptionMain.text == materialToAdd.materialName)
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
            updateMat.quantityMain.text = "x" + updateMat.quantityInt.ToString();
            updateMat.quantitySub.text = "x" + updateMat.quantityInt.ToString();
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
                scrollObject.descriptionMain.text = matInventory[i].materialName;
                scrollObject.descriptionSub.text = matInventory[i].materialName;
                scrollObject.imageRef.texture = matInventory[i].materialTexture;
                scrollObject.quantityInt = matInventory[i].currentAmount;
                scrollObject.quantityMain.text = "x" + scrollObject.quantityInt.ToString();
                scrollObject.quantitySub.text = "x" + scrollObject.quantityInt.ToString();
                GameObject newScrollMaterial = Instantiate(scrollContent);
                newScrollMaterial.transform.SetParent(container.transform, false);
                currentMaterials.Add(newScrollMaterial);
            }
        }
    }

    public void updateBaseInventoryMaterials()
    {
        if (currentMenuObject != null)
        {
            currentMenuObject.GetComponent<BaseMaterialTransition>().ResetSelection();
            for(int i =0; i < currentMaterials.Count; i++)
            {
                Destroy(currentMaterials[i]);
                currentMaterials.RemoveAt(i);
                i--;
            }
            populateBaseInventoryMaterials();
        }
    }

    public void populateBaseInventoryMaterials()
    {
        CraftMaterial[] matInventory = materialManager.GetComponent<MaterialScrollManager>().GetMaterialInventory();
        CraftMaterial[] totalMatInventory = materialManager.GetComponent<MaterialScrollManager>().GetTotalMaterialInventory();

        if (currentMenuObject != null)
        {
            Debug.Log(currentMenuObject);
            //var container = currentMenuObject.transform.Find("MaterialsLayout");
            var container = GameObject.Find("MaterialsLayout").GetComponent<GridLayoutGroup>();
            var totalContainer = GameObject.Find("MaterialsLayout2").GetComponent<GridLayoutGroup>();

            for (int i = 0; i < matInventory.Length; i++)
            {

                if (matInventory[i] == null)
                {
                    break;
                }

                var depositMaterial = DepositScrollContent.transform.Find("Material Scroll Object").GetComponent<MaterialScrollObject>();
                depositMaterial.descriptionMain.text = matInventory[i].materialName;
                depositMaterial.descriptionSub.text = matInventory[i].materialName;
                depositMaterial.imageRef.texture = matInventory[i].materialTexture;
                depositMaterial.quantityInt = matInventory[i].currentAmount;
                depositMaterial.quantityMain.text = "x" + depositMaterial.quantityInt.ToString();
                depositMaterial.quantitySub.text = "x" + depositMaterial.quantityInt.ToString();

                var newScrollMaterial = Instantiate(DepositScrollContent);
                newScrollMaterial.GetComponent<MaterialDepositObject>().currentMaterialCount = matInventory[i].currentAmount;
                newScrollMaterial.GetComponent<MaterialDepositObject>().attachedMaterial = matInventory[i];
                newScrollMaterial.transform.SetParent(container.transform, false);
                currentMaterials.Add(newScrollMaterial);

            }

            for (int i = 0; i < totalMatInventory.Length; i++)
            {

                if (totalMatInventory[i] == null)
                {
                    break;
                }
                Debug.Log("Inside total mat inventory for menu manager");
                var depositMaterial = WithdrawScrollContent.transform.Find("Material Scroll Object").GetComponent<MaterialScrollObject>();
                depositMaterial.descriptionMain.text = totalMatInventory[i].materialName;
                depositMaterial.descriptionSub.text = totalMatInventory[i].materialName;
                depositMaterial.imageRef.texture = totalMatInventory[i].materialTexture;
                depositMaterial.quantityInt = totalMatInventory[i].currentTotalAmount;
                depositMaterial.quantityMain.text = "x" + depositMaterial.quantityInt.ToString();
                depositMaterial.quantitySub.text = "x" + depositMaterial.quantityInt.ToString();

                var newScrollMaterial = Instantiate(WithdrawScrollContent);
                newScrollMaterial.GetComponent<MaterialDepositObject>().currentMaterialCount = totalMatInventory[i].currentTotalAmount;
                newScrollMaterial.GetComponent<MaterialDepositObject>().attachedMaterial = totalMatInventory[i];
                newScrollMaterial.transform.SetParent(totalContainer.transform, false);
                currentMaterials.Add(newScrollMaterial);

            }
        }
    }
    
    
}
