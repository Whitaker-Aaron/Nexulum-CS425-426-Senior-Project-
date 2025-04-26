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
    AudioManager audioManager;
    void Awake()
    {
        DepositNumber.text = materialDepositCount.ToString();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnIncreaseDepositCount()
    {
        
        if (!((materialDepositCount + 1) > currentMaterialCount))
        {
            audioManager.PlaySFX("UIConfirm");
            materialDepositCount += 1;
            UpdateDepositCount();
        }
        else
        {
            audioManager.PlaySFX("UIBack");
        }
    }

    public void OnDecreaseDepositCount()
    {
        if (!(materialDepositCount - 1 < 0))
        {
            audioManager.PlaySFX("UIConfirm");
            materialDepositCount -= 1;
            UpdateDepositCount();
        }
        else
        {
            audioManager.PlaySFX("UIBack");
        }
    }

    public void OnAll()
    {
        audioManager.PlaySFX("UIConfirm");
        materialDepositCount = currentMaterialCount;
        UpdateDepositCount();
    }

    public void UpdateDepositCount()
    {
        DepositNumber.text = materialDepositCount.ToString();
    }

    public void DepositToTotalInventory()
    {
        audioManager.PlaySFX("UIConfirm");
        if (materialDepositCount <= 0) return;
        var scrollManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
        scrollManager.AddToTotalMaterialsInventory(attachedMaterial, materialDepositCount);
        scrollManager.RemoveFromMaterialsInventory(attachedMaterial, materialDepositCount);
        GameObject.Find("MenuManager").GetComponent<MenuManager>().updateBaseInventoryMaterials();
    }

    public void WithdrawFromTotalInventory()
    {
        audioManager.PlaySFX("UIConfirm");
        if (materialDepositCount <= 0) return;
        var scrollManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
        scrollManager.AddToMaterialsInventory(attachedMaterial, materialDepositCount);
        scrollManager.RemoveFromTotalMaterialsInventory(attachedMaterial, materialDepositCount);
        GameObject.Find("MenuManager").GetComponent<MenuManager>().updateBaseInventoryMaterials();
    }
}
