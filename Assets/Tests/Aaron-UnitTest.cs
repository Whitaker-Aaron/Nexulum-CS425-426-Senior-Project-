using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class AaronUnitTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void AaronUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions

        MaterialsInventory materialsInventory;
        //materialsInventory = GameObject.AddComponent<MaterialsInventory>();
        CraftMaterial craftMaterialToAdd = ScriptableObject.CreateInstance<CraftMaterial>();

        //Asserts that the scroll manager is successfully able to be located within the game project.
        //Assert.AreNotEqual(null, materialsInventory);

        //materialsInventory.AddToInventory(craftMaterialToAdd, 3);

        
        

    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator AaronUnitTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        var menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        Assert.AreNotEqual(null, menuManager);

        yield return null;
    }
}
