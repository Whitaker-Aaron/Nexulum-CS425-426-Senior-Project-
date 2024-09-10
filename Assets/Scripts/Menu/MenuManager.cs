using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
    
{
    [SerializeField] GameObject materialsMenuReference;
    [SerializeField] GameObject scrollContent;

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
            }
        }



    }
}
