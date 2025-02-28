using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour, SaveSystemInterface
    
{
    [SerializeField] GameObject materialsMenuReference;
    [SerializeField] GameObject totalMaterialMenuReference;
    [SerializeField] GameObject baseShopMenuReference;
    [SerializeField] GameObject terminalMenuReference;
    [SerializeField] GameObject craftMenuReference;
    [SerializeField] GameObject itemsMenuReference;
    [SerializeField] GameObject equipMenuReference;
    [SerializeField] GameObject chestMenuReference;

    [SerializeField] GameObject scrollContent;
    [SerializeField] GameObject DepositScrollContent;
    [SerializeField] GameObject WithdrawScrollContent;
    [SerializeField] GameObject ChestWithdrawObject;
    [SerializeField] GameObject shopOptionObject;

    [SerializeField] GameObject craftListsReference;
    [SerializeField] GameObject shopOptionsReference;
    [SerializeField] GameObject pauseMenuReference;

    List<GameObject> currentMaterials = new List<GameObject>();
    List<GameObject> currentBaseMaterials = new List<GameObject>();

    GameObject currentMenuObject;
    GameObject canvas;
    GameObject materialManager;
    masterInput inputManager;
    CharacterBase character;
    Chest currentChest;

    public bool menuActive = false;
    public bool chestMenuActive = false;
    bool pauseMenuActive = false;
    public bool menusPaused = false;
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

    public void SaveData(ref SaveData data)
    {
        var craftLists = GameObject.Find("CraftLists");
        var weaponLists = craftLists.transform.Find("WeaponCraftList").GetComponent<WeaponCraftList>();
        var runeLists = craftLists.transform.Find("RunesCraftList").GetComponent<RunesCraftList>();
        var itemLists = craftLists.transform.Find("ItemsCraftList").GetComponent<ItemsCraftList>();
        int accRecipeIndex = 0;
        int allRecipeIndex = 0;

        for(int i =0; i < itemLists.allRecipes.Count;  i++)
        {
            var recipe = new CraftRecipeSaveData();
            recipe.recipeType = "Item";
            recipe.recipeName = itemLists.allRecipes[i].recipeName;
            recipe.hasCrafted = itemLists.allRecipes[i].hasCrafted;
            recipe.hasPurchased = itemLists.allRecipes[i].hasPurchased;
            data.allRecipes[allRecipeIndex] = recipe;
            allRecipeIndex++;
        }
        for (int i = 0; i < weaponLists.allRecipes.Count; i++)
        {
            var recipe = new CraftRecipeSaveData();
            recipe.recipeType = "Weapon";
            recipe.recipeName = weaponLists.allRecipes[i].recipeName;
            recipe.hasCrafted = weaponLists.allRecipes[i].hasCrafted;
            recipe.hasPurchased = weaponLists.allRecipes[i].hasPurchased;
            data.allRecipes[allRecipeIndex] = recipe;
            allRecipeIndex++;
        }
        for (int i = 0; i < runeLists.allRecipes.Count; i++)
        {
            var recipe = new CraftRecipeSaveData();
            recipe.recipeType = "Rune";
            recipe.recipeName = runeLists.allRecipes[i].recipeName;
            recipe.hasCrafted = runeLists.allRecipes[i].hasCrafted;
            recipe.hasPurchased = runeLists.allRecipes[i].hasPurchased;
            data.allRecipes[allRecipeIndex] = recipe;
            allRecipeIndex++;
        }
        for (int i = 0; i < itemLists.accessibleRecipes.Count; i++)
        {
            var recipe = new CraftRecipeSaveData();
            recipe.recipeType = "Item";
            recipe.recipeName = itemLists.accessibleRecipes[i].recipeName;
            recipe.hasCrafted = itemLists.accessibleRecipes[i].hasCrafted;
            recipe.hasPurchased = itemLists.accessibleRecipes[i].hasPurchased;
            data.accessibleRecipes[accRecipeIndex] = recipe;
            accRecipeIndex++;
        }
        for (int i = 0; i < weaponLists.accessibleRecipes.Count; i++)
        {
            var recipe = new CraftRecipeSaveData();
            recipe.recipeType = "Weapon";
            recipe.recipeName = weaponLists.accessibleRecipes[i].recipeName;
            recipe.hasCrafted = weaponLists.accessibleRecipes[i].hasCrafted;
            recipe.hasPurchased = weaponLists.accessibleRecipes[i].hasPurchased;
            data.accessibleRecipes[accRecipeIndex] = recipe;
            accRecipeIndex++;
        }
        for (int i = 0; i < runeLists.accessibleRecipes.Count; i++)
        {
            var recipe = new CraftRecipeSaveData();
            recipe.recipeType = "Rune";
            recipe.recipeName = runeLists.accessibleRecipes[i].recipeName;
            recipe.hasCrafted = runeLists.accessibleRecipes[i].hasCrafted;
            recipe.hasPurchased = runeLists.accessibleRecipes[i].hasPurchased;
            data.accessibleRecipes[accRecipeIndex] = recipe;
            accRecipeIndex++;
        }
    }

    public void LoadData(SaveData data)
    {
        var craftLists = GameObject.Find("CraftLists");
        var weaponLists = craftLists.transform.Find("WeaponCraftList").GetComponent<WeaponCraftList>();
        var runeLists = craftLists.transform.Find("RunesCraftList").GetComponent<RunesCraftList>();
        var itemLists = craftLists.transform.Find("ItemsCraftList").GetComponent<ItemsCraftList>();
        if (data.isNewFile)
        {
            for (int i = 0; i < itemLists.allRecipes.Count; i++)
            {
                itemLists.allRecipes[i].hasCrafted = false;
                itemLists.allRecipes[i].hasPurchased = false;
            }
            for (int i = 0; i < weaponLists.allRecipes.Count; i++)
            {
                weaponLists.allRecipes[i].hasCrafted = false;
                weaponLists.allRecipes[i].hasPurchased = false;
            }
            for (int i = 0; i < runeLists.allRecipes.Count; i++)
            {
                runeLists.allRecipes[i].hasCrafted = false;
                runeLists.allRecipes[i].hasPurchased = false;
            }
            for(int i = 0; i < data.accessibleRecipes.Length; i++)
            {
                if (data.accessibleRecipes[i] == null) continue;
                switch (data.accessibleRecipes[i].recipeType)
                {
                    case "Weapon":
                        weaponLists.addToAccessibleRecipes(GameObject.Find("WeaponsList").
                            GetComponent<WeaponsList>().ReturnWeapon(data.accessibleRecipes[i].recipeName).weaponRecipe);
                        break;
                    case "Rune":
                        runeLists.addToAccessibleRecipes(GameObject.Find("RunesList").
                            GetComponent<RuneList>().ReturnRune(data.accessibleRecipes[i].recipeName).runeRecipe);
                        break;
                    case "Item":
                        itemLists.addToAccessibleRecipes(GameObject.Find("ItemsList").
                            GetComponent<ItemsList>().ReturnItem(data.accessibleRecipes[i].recipeName).itemRecipe);
                        break;
                }
            }
        }
        else
        {
            weaponLists.accessibleRecipes.Clear();
            runeLists.accessibleRecipes.Clear();
            itemLists.accessibleRecipes.Clear();
            for (int i = 0; i < data.accessibleRecipes.Length; i++)
            {
                if (data.accessibleRecipes[i] == null) continue;
                Debug.Log(data.accessibleRecipes[i].recipeName);
                switch (data.accessibleRecipes[i].recipeType)
                {
                    case "Weapon":
                        var weaponRecipe = GameObject.Find("WeaponsList").
                            GetComponent<WeaponsList>().ReturnWeapon(data.accessibleRecipes[i].recipeName).weaponRecipe;
                        weaponRecipe.hasCrafted = data.accessibleRecipes[i].hasCrafted;
                        weaponRecipe.hasPurchased = data.accessibleRecipes[i].hasPurchased;
                        weaponLists.addToAccessibleRecipes(weaponRecipe);
                        break;
                    case "Rune":
                        var runeRecipe = GameObject.Find("RunesList").
                            GetComponent<RuneList>().ReturnRune(data.accessibleRecipes[i].recipeName).runeRecipe;
                        Debug.Log(runeRecipe);
                        runeRecipe.hasCrafted = data.accessibleRecipes[i].hasCrafted;
                        runeRecipe.hasPurchased = data.accessibleRecipes[i].hasPurchased;
                        runeLists.addToAccessibleRecipes(runeRecipe);
                        break;
                    case "Item":
                        var itemRecipe = GameObject.Find("ItemsList").
                            GetComponent<ItemsList>().ReturnItem(data.accessibleRecipes[i].recipeName).itemRecipe;
                        itemRecipe.hasCrafted = data.accessibleRecipes[i].hasCrafted;
                        itemRecipe.hasPurchased = data.accessibleRecipes[i].hasPurchased;
                        itemLists.addToAccessibleRecipes(itemRecipe);
                        break;
                }
            }
        }
    }

        public GameObject GetCurrentMenu()
    {
        return currentMenuObject;
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

    public void openChestMenu(Chest chestRef)
    {
        if (menuActive)
        {
            Destroy(currentMenuObject);
        }
        currentChest = chestRef;
        var chestDeposit = chestMenuReference.GetComponent<ChestMaterialDeposit>();
        for(int i =0; i < chestDeposit.itemDisplays.Count; i++)
        {
            if (i < chestRef.itemsToAdd.Count && chestRef.itemsToAdd[i] != null)
            {
                chestDeposit.itemDisplays[i].SetActive(true);
                var chestItem = chestDeposit.itemDisplays[i].GetComponent<ChestWithdrawOption>();
                chestItem.itemName.text = chestRef.itemsToAdd[i].material.materialName;
                chestItem.itemTexture.texture = chestRef.itemsToAdd[i].material.materialTexture;
                chestItem.itemAmount.text = "x" + chestRef.itemsToAdd[i].amount.ToString();
                chestItem.chestAmount = chestRef.itemsToAdd[i].amount;
                chestItem.item = chestRef.itemsToAdd[i].material;
                chestItem.itemIndex = i;
            }
            else chestDeposit.itemDisplays[i].SetActive(false);
        }
        currentMenuObject = Instantiate(chestMenuReference);
        currentMenuObject.transform.SetParent(canvas.transform, false);

        inputManager.pausePlayerInput();
        chestMenuActive = true;
        
        populateInventoryMaterials();
    }

    public void UpdateChest(int index, int amountRemoved)
    {
        if (chestMenuActive && currentChest != null) {
            currentChest.UpdateChestItem(index, amountRemoved);
            UpdateChestIndex();
        }
    }

    public void UpdateChestIndex()
    {
        if(chestMenuActive && currentChest != null)
        {
            var curItems = currentMenuObject.GetComponent<ChestMaterialDeposit>().itemDisplays;
            int counter = 0;
            for (int i = 0; i < curItems.Count; i++)
            {
                if (curItems[i].GetComponent<ChestWithdrawOption>().chestAmount > 0 && curItems[i].activeSelf) // && 
                {
                    curItems[i].GetComponent<ChestWithdrawOption>().itemIndex = counter;
                    counter++;
                }
            }
        }
    }

    public void resetShopSelection()
    {
        if(currentMenuObject != null && currentMenuObject.GetComponent<BaseShopMenu>() != null)
        {
            currentMenuObject.GetComponent<BaseShopMenu>().resetSelection();
            currentMenuObject.GetComponent<BaseShopMenu>().updateShopListings();
        }
    }

    public void updateShopCount(float amount)
    {
        if (currentMenuObject != null && currentMenuObject.GetComponent<BaseShopMenu>() != null)
        {
            currentMenuObject.GetComponent<BaseShopMenu>().updateFlorentineCount(amount);
        }
    }

    public void closeChestMenu()
    {
        if (chestMenuActive)
        {
            chestMenuActive = false;
            Destroy(currentMenuObject);
            inputManager.resumePlayerInput();
            if (currentChest != null)
            {
                currentChest.OnMenuClosed();
            }
            currentChest = null;

        }
    }


    public void openPauseMenu(InputAction.CallbackContext context)
    {


        if (!pauseMenuActive && !character.transitioningRoom && !menusPaused && context.performed)
        {

            if (GameObject.FindGameObjectWithTag("CraftLists") != null)
            {
                //Destroy(GameObject.FindGameObjectWithTag("CraftLists"));
            }
            if(GameObject.FindGameObjectWithTag("ShopOptions") != null)
            {
                //Destroy(GameObject.FindGameObjectWithTag("ShopOptions"));
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
            character.usingTerminal = false;


        }

    }

        public void openMenu(InputAction.CallbackContext context)
    {
        if (character.usingTerminal) return;
        if(!menuActive && !pauseMenuActive && !character.inEvent && !character.transitioningRoom && !character.inDialogueBox && !menusPaused && context.performed) {
            if (chestMenuActive && currentMenuObject != null) closeChestMenu();
            currentMenuObject = Instantiate(materialsMenuReference);
            populateInventoryMaterials();
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            currentMenuObject.GetComponent<RectTransform>().localPosition = new Vector3(currentMenuObject.GetComponent<RectTransform>().localPosition.x + 75, currentMenuObject.GetComponent<RectTransform>().localPosition.y + 90, currentMenuObject.GetComponent<RectTransform>().localPosition.z);
            Debug.Log("activating main menu");
            currentMenuObject.transform.SetParent(canvas.transform, false);

            
            inputManager.pausePlayerInput();
            menuActive = true;
        }
        else if(menuActive && !pauseMenuActive && !character.transitioningRoom && context.performed)
        {
            //currentMenuObject.SetActive(false);
            CloseMenu();
            
        }

    }

    public void CloseMenu()
    {
        if (GameObject.FindGameObjectWithTag("CraftLists") != null)
        {
            //Destroy(GameObject.FindGameObjectWithTag("CraftLists"));
        }
        if (GameObject.FindGameObjectWithTag("ShopOptions") != null)
        {
            //Destroy(GameObject.FindGameObjectWithTag("ShopOptions"));
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
        if (GameObject.FindGameObjectWithTag("ItemMenu") != null)
        {

            Destroy(GameObject.FindGameObjectWithTag("ItemMenu"));
        }
        inputManager.resumePlayerInput();
        character.usingTerminal = false;
        menuActive = false;
    }

    public List<CraftRecipe> returnWeaponsCraftList()
    {
        return GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<WeaponCraftList>().accessibleRecipes;
        
    }

    public void addToWeaponsCraftList(CraftRecipe recipeToAdd)
    {
        GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<WeaponCraftList>().addToAccessibleRecipes(recipeToAdd);
    }

    public void addToItemsCraftList(CraftRecipe recipeToAdd)
    {
        GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<ItemsCraftList>().addToAccessibleRecipes(recipeToAdd);
    }

    public void addToRunesCraftList(CraftRecipe recipeToAdd)
    {
        GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<RunesCraftList>().addToAccessibleRecipes(recipeToAdd);
    }

    public List<CraftRecipe> returnItemsCraftList()
    {
        return GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<ItemsCraftList>().accessibleRecipes;
    }

    public List<CraftRecipe> returnRunesCraftList()
    {
        return GameObject.FindGameObjectWithTag("CraftLists").GetComponentInChildren<RunesCraftList>().accessibleRecipes;
    }

    public List<CraftRecipe> returnRecipesInShop()
    {
        return GameObject.FindGameObjectWithTag("ShopOptions").GetComponentInChildren<ShopCraftRecipes>().getRecipes();
    }

    public List<WeaponBase> returnWeaponsInShop()
    {
        return GameObject.FindGameObjectWithTag("ShopOptions").GetComponentInChildren<ShopWeapons>().getWeapons();
    }
    public List<PlayerItem> returnItemsInShop()
    {
        return GameObject.FindGameObjectWithTag("ShopOptions").GetComponentInChildren<ShopPlayerItems>().getItems();
    }
    public void removeShopCraftRecipe(CraftRecipe recipeToRemove)
    {
        //GameObject.FindGameObjectWithTag("ShopOptions").GetComponentInChildren<ShopCraftRecipes>().removeRecipe(recipeToRemove);
    }

    public void navigateToMaterialMenu()
    {
        if(menuActive) {
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(materialsMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform, false);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            currentMenuObject.GetComponent<RectTransform>().localPosition = new Vector3(currentMenuObject.GetComponent<RectTransform>().localPosition.x + 75, currentMenuObject.GetComponent<RectTransform>().localPosition.y + 90, currentMenuObject.GetComponent<RectTransform>().localPosition.z);
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

    public void AddToAvailableCraftRecipes(CraftRecipe recipeToAdd)
    {
        switch(recipeToAdd.type)
        {
            case CraftRecipe.CraftTypes.Weapon:
                addToWeaponsCraftList(recipeToAdd);
                break;
            case CraftRecipe.CraftTypes.Rune:
                addToRunesCraftList(recipeToAdd);
                break;
            case CraftRecipe.CraftTypes.Item:
                addToItemsCraftList(recipeToAdd);
                break;
        }
    }

    public void navigateToBaseShopMenu()
    {
        Debug.Log("Navigating to Base Shop Menu");
        Destroy(currentMenuObject);
        currentMenuObject = Instantiate(baseShopMenuReference);
        currentMenuObject.transform.SetParent(canvas.transform, false);
        //populateBaseShopOptions(StoreItem.StoreItemType.Recipe);
        //currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    public void navigateToCraftMenu()
    {
        
        if (menuActive)
        {
            Debug.Log("Navigating to Craft Menu");
            //Instantiate(craftListsReference);
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(craftMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform, false);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            currentMenuObject.GetComponent<RectTransform>().localPosition = new Vector3(currentMenuObject.GetComponent<RectTransform>().localPosition.x + 75, currentMenuObject.GetComponent<RectTransform>().localPosition.y + 90, currentMenuObject.GetComponent<RectTransform>().localPosition.z);
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
            currentMenuObject.GetComponent<RectTransform>().localPosition = new Vector3(currentMenuObject.GetComponent<RectTransform>().localPosition.x + 75, currentMenuObject.GetComponent<RectTransform>().localPosition.y + 90, currentMenuObject.GetComponent<RectTransform>().localPosition.z);
        }
    }


    public void navigateToEquipMenu()
    {
        if (menuActive)
        {
            Debug.Log("Navigating to Equip Menu");
            //Instantiate(craftListsReference);
            Destroy(currentMenuObject);
            currentMenuObject = Instantiate(equipMenuReference);
            currentMenuObject.transform.SetParent(canvas.transform, false);
            currentMenuObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            currentMenuObject.GetComponent<RectTransform>().localPosition = new Vector3(currentMenuObject.GetComponent<RectTransform>().localPosition.x + 75, currentMenuObject.GetComponent<RectTransform>().localPosition.y + 90, currentMenuObject.GetComponent<RectTransform>().localPosition.z);
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
        int matCounter = 0;
        if (currentMenuObject !=  null) {
            var container = currentMenuObject.GetComponentInChildren<GridLayoutGroup>();
            for (int i = 0; i < matInventory.Length; i++) {

                if (matInventory[i] == null) {
                    break;
                }
                matCounter++;
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
        GameObject.Find("CurrentMaterialAmount").GetComponent<TMP_Text>().text = matCounter.ToString() + "/20";
    }

    public void updateChestMenuMaterials()
    {
        for (int i = 0; i < currentMaterials.Count; i++)
        {
            Destroy(currentMaterials[i]);
            currentMaterials.RemoveAt(i);
            i--;
        }
        populateInventoryMaterials();
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

    public void populateBaseShopOptions(StoreItem.StoreItemType type)
    {
        var curFlorentine = character.GetFlorentine();
        if (currentMenuObject != null)
        {
            var container = GameObject.Find("StoreScrollContent").GetComponent<VerticalLayoutGroup>();
            switch (type)
            {
                case StoreItem.StoreItemType.Recipe:
                    var recipes = returnRecipesInShop();
                    if (recipes == null) break;
                    for (int i = 0; i < recipes.Count; i++)
                    {
                        if (recipes[i].hasPurchased) continue;
                        var shopItem = shopOptionObject.GetComponent<StoreItem>();
                        shopItem.craftRecipe = recipes[i];
                        shopItem.storeItemName.GetComponent<TMP_Text>().text = recipes[i].recipeName + " Recipe";
                        shopItem.storeItemNameShadow.GetComponent<TMP_Text>().text = recipes[i].recipeName + " Recipe";
                        shopItem.florentineRequiredAmount.GetComponent<Text>().text = "x" + recipes[i].shopCost.ToString();
                        shopItem.storeType = type;
                        Color32 red = new Color32(0xE4, 0x3c, 0x54, 0xFF);
                        Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
                        if (curFlorentine >= recipes[i].shopCost)
                        {
                            shopItem.purchaseButton.GetComponent<Button>().interactable = true;
                            shopItem.disabledPanel.SetActive(false);
                            shopItem.florentineRequiredAmount.GetComponent<Text>().color = white;
                        }
                        else
                        {
                            shopItem.purchaseButton.GetComponent<Button>().interactable = false;
                            shopItem.disabledPanel.SetActive(true);
                            shopItem.florentineRequiredAmount.GetComponent<Text>().color = red;
                        }
                        var curShop = Instantiate(shopOptionObject);
                        curShop.transform.SetParent(container.transform, false);
                        currentMenuObject.GetComponent<BaseShopMenu>().addToCurShopListings(curShop);
                    }
                    break;
                case StoreItem.StoreItemType.Weapon:
                    var weapons = returnWeaponsInShop();
                    if (weapons == null) break;
                    for (int i = 0; i < weapons.Count; i++)
                    {
                        var shopItem = shopOptionObject.GetComponent<StoreItem>();
                        shopItem.weaponBase = weapons[i];
                        shopItem.storeItemName.GetComponent<TMP_Text>().text = weapons[i].weaponName;
                        shopItem.storeItemNameShadow.GetComponent<TMP_Text>().text = weapons[i].weaponName;
                        shopItem.florentineRequiredAmount.GetComponent<Text>().text = "x" + weapons[i].shopCost.ToString();
                        shopItem.storeType = type;
                        Color32 red = new Color32(0xE4, 0x3c, 0x54, 0xFF);
                        Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
                        if (curFlorentine >= weapons[i].shopCost)
                        {
                            shopItem.purchaseButton.GetComponent<Button>().interactable = true;
                            shopItem.disabledPanel.SetActive(false);
                            shopItem.florentineRequiredAmount.GetComponent<Text>().color = white;
                        }
                        else
                        {
                            shopItem.purchaseButton.GetComponent<Button>().interactable = false;
                            shopItem.disabledPanel.SetActive(true);
                            shopItem.florentineRequiredAmount.GetComponent<Text>().color = red;
                        }
                        var curShop = Instantiate(shopOptionObject);
                        curShop.transform.SetParent(container.transform, false);
                        currentMenuObject.GetComponent<BaseShopMenu>().addToCurShopListings(curShop);
                    }
                    break;
                case StoreItem.StoreItemType.Item:
                    var items = returnItemsInShop();
                    if (items == null) break;
                    for (int i = 0; i < items.Count; i++)
                    {
                        var shopItem = shopOptionObject.GetComponent<StoreItem>();
                        shopItem.playerItem = items[i];
                        shopItem.storeItemName.GetComponent<TMP_Text>().text = items[i].itemName;
                        shopItem.storeItemNameShadow.GetComponent<TMP_Text>().text = items[i].itemName;
                        shopItem.florentineRequiredAmount.GetComponent<Text>().text = "x" + items[i].shopCost.ToString();
                        shopItem.itemQuantity.GetComponent<TMP_Text>().text = "Have: " + items[i].itemAmount.ToString() + "/" + items[i].maxItemAmount.ToString(); ;
                        shopItem.description.GetComponent<TMP_Text>().text = items[i].itemDescription;
                        shopItem.storeType = type;
                        Color32 red = new Color32(0xE4, 0x3c, 0x54, 0xFF);
                        Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
                        if (curFlorentine >= items[i].shopCost)
                        {
                            shopItem.purchaseButton.GetComponent<Button>().interactable = true;
                            shopItem.disabledPanel.SetActive(false);
                            shopItem.florentineRequiredAmount.GetComponent<Text>().color = white;
                        }
                        else
                        {
                            shopItem.purchaseButton.GetComponent<Button>().interactable = false;
                            shopItem.disabledPanel.SetActive(true);
                            shopItem.florentineRequiredAmount.GetComponent<Text>().color = red;
                        }
                        var curShop = Instantiate(shopOptionObject);
                        curShop.transform.SetParent(container.transform, false);
                        currentMenuObject.GetComponent<BaseShopMenu>().addToCurShopListings(curShop);
                    }
                    break;
            }
            
        }
    }

    public void RemoveShopItemFromShop(GameObject shopItem)
    {
        switch (shopItem.GetComponent<StoreItem>().storeType)
        {
            case StoreItem.StoreItemType.Recipe:
                removeShopCraftRecipe(shopItem.GetComponent<StoreItem>().craftRecipe);
                break;
        }
        
        Destroy(shopItem);
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
            int matCounter = 0;
            int baseMatCounter = 0;
            for (int i = 0; i < matInventory.Length; i++)
            {

                if (matInventory[i] == null)
                {
                    break;
                }
                matCounter++;
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
            GameObject.Find("CurrentMaterialAmount").GetComponent<TMP_Text>().text = matCounter.ToString() + "/20";

            for (int i = 0; i < totalMatInventory.Length; i++)
            {

                if (totalMatInventory[i] == null)
                {
                    break;
                }
                baseMatCounter++;
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
            GameObject.Find("CurrentBaseMaterialAmount").GetComponent<TMP_Text>().text = baseMatCounter.ToString() + "/150";
        }
    }
    
    
}
