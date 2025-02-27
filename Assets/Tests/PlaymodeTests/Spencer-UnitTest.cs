using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SpencerUnitTest
{

    private class TestObject
    {
        public float damageDropOffDistance = 10f;
        public float shootingRange;

        public void updateDistance(float modifier)
        {
            shootingRange = damageDropOffDistance * modifier;
        }
    }

    /*
    [Test]
    public void testCreateProjectilePool()
    {
        bool test = false;
        GameObject obj = new GameObject();

        projectileManager manager = obj.AddComponent<projectileManager>();
        //obj.tag = "TestTag";

        manager.createNewPool("testingPool", obj, 10);

        obj.tag = "none";
        GameObject temp = manager.getProjectile("testingPool", Vector3.zero, Quaternion.identity);
        if (temp != null)
            test = true;
        Assert.IsTrue(test);
        Assert.AreEqual(10, manager.GetPool("poolList").Count);
    }
    */

    [UnityTest]
    public IEnumerator testDistanceUpdate()
    {
        SceneManager.LoadScene("TitleScreen");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "TitleScreen");//.LoadScene("TitleScreen"));
        yield return new WaitForSeconds(.3f);

        // Ensure masterInput is available
        masterInput input = masterInput.instance;
        Assert.IsNotNull(input, "masterInput instance is null!");

        // Get initial distance
        float initialDistance = input.shootingRange;

        // Apply modifier
        float modifier = 1.5f;
        float expectedRange = input.damageDropOffDistance * modifier;

        input.updateDistance(modifier); // Call the real method

        // Verify if the updateDistance method applied correctly
        Assert.AreEqual(expectedRange, input.shootingRange, 0.001f, "Shooting range update failed!");

        yield return null;
    }
}
