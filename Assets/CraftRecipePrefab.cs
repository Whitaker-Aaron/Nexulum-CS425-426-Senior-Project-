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
        craftRecipeName.GetComponent<Text>().text = craftRecipe.recipeName;
        populateMaterialsList();
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
            wrapComponent.materialDescription.GetComponent<Text>().text = item.Key.materialName;
            wrapComponent.requiredAmount.GetComponent<Text>().text = "Need: x" + item.Value;
            wrapComponent.amountHas.GetComponent<Text>().text = "Has: x" + curAmount;
            wrapComponent.materialTexture.GetComponent<RawImage>().texture = item.Key.materialTexture;


            var matRef = Instantiate(materialRequirementObject);
            matRef.transform.SetParent(materialContainer.transform);
            matRef.transform.localScale = Vector3.one;

            if (curAmount < item.Value)
            {
                craftButton.GetComponent<Button>().interactable = false;
            }
        }
    }
}
