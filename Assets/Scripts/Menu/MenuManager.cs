using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
    
{
    [SerializeField] GameObject materialsMenuReference;
    [SerializeField] GameObject scrollContent;
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
            currentMenuObject.SetActive(false);
            menuActive = false;
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
