using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BarrierRequirementUI : MonoBehaviour
{
    [SerializedDictionary("Material", "Amount")]
    public SerializedDictionary<CraftMaterial, int> requiredMaterials;
    [SerializeField] List<GameObject> materialWrappers;
    [SerializeField] List<bool> requiredMaterialCheck;
    [SerializeField] List<int> curAmountCheck;
    [SerializeField] Barrier barrier;
    MaterialScrollManager matManager;
    // Start is called before the first frame update
    void Start()
    {
        matManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
        populateFields();
        populateFields();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            int i = 0;
            bool changeDetected = false;
            foreach (var item in requiredMaterials)
            {
                int curAmount = matManager.GetMaterialAmount(item.Key);
                if(curAmount != curAmountCheck[i])
                {
                    changeDetected = true;
                }
            }
            if(changeDetected) populateFields();
        }
    }

    public void removeMatFromInventory()
    {
        foreach (var item in requiredMaterials)
        {
            var matManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
            matManager.RemoveFromMaterialsInventory(item.Key, item.Value);
        }
    }

    public void populateFields()
    {
        int i = 0;
        bool canCraft = true;
        foreach (var item in requiredMaterials)
        {

            
            var wrapComponent = materialWrappers[i].GetComponent<materialRequirementsWrapper>();



            int curAmount = matManager.GetMaterialAmount(item.Key);
            curAmountCheck[i] = curAmount;
            wrapComponent.materialDescription.GetComponent<TMP_Text>().text = item.Key.materialName;
            //wrapComponent.requiredAmount.GetComponent<TMP_Text>().text = item.Value.ToString();
            wrapComponent.amountHas.GetComponent<TMP_Text>().text = curAmount.ToString() + "/" + item.Value.ToString();
            wrapComponent.materialTexture.GetComponent<RawImage>().texture = item.Key.materialTexture;
            Color32 red = new Color32(0xE4, 0x3c, 0x54, 0xFF);
            Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            if (curAmount < item.Value)
            {
                canCraft = false;
                wrapComponent.amountHas.GetComponent<TMP_Text>().color = red;
            }
            else wrapComponent.amountHas.GetComponent<TMP_Text>().color = white;
            i++;
        }
        if (!canCraft)
        {
            barrier.SetCanTrigger(false);
        }
        else barrier.SetCanTrigger(true);
    }

}
