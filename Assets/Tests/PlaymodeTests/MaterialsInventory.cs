using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MaterialsInventoryTest
{

 
    [UnityTest]
    public IEnumerator AddCraftMaterialToMaterialsInventory()
    {
        //Initialize craft material to unit test.
        CraftMaterial craftMaterialToAdd = ScriptableObject.CreateInstance<CraftMaterial>();
        craftMaterialToAdd.materialName = "Unit Test Craft Material";

        //Load scene where the inventory is created.
        SceneManager.LoadScene("TitleScreen");
        yield return new WaitForSeconds(1);

        //Get reference to the inventory and assert it exists.
        var materialsInventory = GameObject.Find("MaterialsInventory").GetComponent<MaterialsInventory>();
        Assert.AreNotEqual(null, materialsInventory);

        //Add initialized material to inventory.
        materialsInventory.AddToInventory(craftMaterialToAdd, 3);

        //Get reference to inventory array and assert its not null and not empty.
        var inventory = materialsInventory.GetInventory();
        Assert.AreNotEqual(null, inventory);
        Assert.NotZero(materialsInventory.GetCurrentInventorySize());

        //Boolean to assert if material added is found in inventory.
        bool inInventory = false;
        
        for(int i =0; i < materialsInventory.GetCurrentInventorySize(); i++)
        {
            var material = inventory[i];
            if(material.materialName == craftMaterialToAdd.materialName)
            {
                inInventory = true;
                break;
            }
        }

        //Assert that material is in inventory
        Assert.AreEqual(true, inInventory);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RemoveCraftMaterialFromMaterialsInventory()
    {
        //Initialize craft material to unit test.
        CraftMaterial craftMaterialToAdd = ScriptableObject.CreateInstance<CraftMaterial>();
        craftMaterialToAdd.materialName = "Unit Test Craft Material";

        //Load scene where the inventory is created.
        SceneManager.LoadScene("TitleScreen");
        yield return new WaitForSeconds(1);

        //Get reference to the inventory and assert it exists.
        var materialsInventory = GameObject.Find("MaterialsInventory").GetComponent<MaterialsInventory>();
        Assert.AreNotEqual(null, materialsInventory);

        //Add initialized material to inventory.
        materialsInventory.AddToInventory(craftMaterialToAdd, 3);

        //Remove material from inventory
        materialsInventory.RemoveFromInventory(craftMaterialToAdd, 3);

        //Get reference to inventory array and assert its not null and not empty.
        var inventory = materialsInventory.GetInventory();


        //Boolean to assert if material added is not found in inventory.
        bool notInInventory = true;

        for (int i = 0; i < materialsInventory.GetCurrentInventorySize(); i++)
        {
            var material = inventory[i];
            if (material.materialName == craftMaterialToAdd.materialName)
            {
                notInInventory = false;
                break;
            }
        }
        Assert.AreEqual(true, notInInventory);

        yield return null;
    }
}
