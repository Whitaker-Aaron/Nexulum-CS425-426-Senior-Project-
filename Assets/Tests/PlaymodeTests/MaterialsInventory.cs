using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
        
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
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

        yield return null;
    }
}
