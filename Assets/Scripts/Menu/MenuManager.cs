using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject terminalScrollContent;
    [SerializeField] GameObject craftListsReference;
    [SerializeField] GameObject pauseMenuReference;

    List<GameObject> currentMaterials = new List<GameObject>();

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
        if (!menuActive && !pauseMenuActive)
        {
            currentMenuObject = Instantiate(materialsMenuReference);
            Debug.Log("activating main menu");
            currentMenuObject.transform.SetParent(canvas.transform, false);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            populateInventoryMaterials();
            inputManager.pausePlayerInput();
            menuActive = true;
        }
        else if (menuActive && !pauseMenuActive && !character.transitioningRoom)
        {
            //currentMenuObject.SetActive(false);
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
            inputManager.resumePlayerInput();
        }
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
            inputManager.resumePlayerInput();
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
                newScrollMaterial.transform.SetParent(container.transform, false);
                currentMaterials.Add(newScrollMaterial);
            }
        }
    }

    public void populateBaseInventoryMaterials()
    {
        CraftMaterial[] matInventory = materialManager.GetComponent<MaterialScrollManager>().GetMaterialInventory();

        if (currentMenuObject != null)
        {
            var container = currentMenuObject.GetComponentInChildren<GridLayoutGroup>();
            for (int i = 0; i < matInventory.Length; i++)
            {

                if (matInventory[i] == null)
                {
                    break;
                }

                var depositMaterial = terminalScrollContent.transform.Find("Material Scroll Object").GetComponent<MaterialScrollObject>();
                depositMaterial.description.text = matInventory[i].materialName;
                depositMaterial.imageRef.texture = matInventory[i].materialTexture;
                depositMaterial.quantityInt = matInventory[i].currentAmount;
                depositMaterial.quantity.text = "x" + depositMaterial.quantityInt.ToString();

                var newScrollMaterial = Instantiate(terminalScrollContent);
                newScrollMaterial.transform.SetParent(container.transform, false);
                

                //var depositMaterial = terminalScrollContent.GetComponentInChildren<MaterialScrollObject>();
                
                
                //currentMaterials.Add(newScrollMaterial);
            }
        }
    }
    
    
}
