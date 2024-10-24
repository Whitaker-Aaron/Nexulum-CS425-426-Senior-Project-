using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialDepositObject : MonoBehaviour
{
    [SerializeField] Text DepositNumber;
    private int materialDepositCount = 0;
    public int currentMaterialCount;
    public CraftMaterial attachedMaterial;
    void Awake()
    {
        DepositNumber.text = materialDepositCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnIncreaseDepositCount()
    {
        
        if (!((materialDepositCount + 1) > currentMaterialCount))
        {
            
            materialDepositCount += 1;
            UpdateDepositCount();
        }
    }

    public void OnDecreaseDepositCount()
    {
        if (!(materialDepositCount - 1 < 0))
        {
            materialDepositCount -= 1;
            UpdateDepositCount();
        }
    }

    public void UpdateDepositCount()
    {
        DepositNumber.text = materialDepositCount.ToString();
    }

    public void DepositToTotalInventory()
    {
        var scrollManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
        scrollManager.AddToTotalMaterialsInventory(attachedMaterial, materialDepositCount);
        scrollManager.RemoveFromMaterialsInventory(attachedMaterial, materialDepositCount);
        GameObject.Find("MenuManager").GetComponent<MenuManager>().updateBaseInventoryMaterials();
    }

    public void WithdrawFromTotalInventory()
    {
        var scrollManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
        scrollManager.AddToMaterialsInventory(attachedMaterial, materialDepositCount);
        scrollManager.RemoveFromTotalMaterialsInventory(attachedMaterial, materialDepositCount);
        GameObject.Find("MenuManager").GetComponent<MenuManager>().updateBaseInventoryMaterials();
    }
}
