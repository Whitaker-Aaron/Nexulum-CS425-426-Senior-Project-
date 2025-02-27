using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class LOSTest
{
    private GameObject mockEnemy;
    private GameObject mockPlayerJimothy;
    private GameObject mockPlayerTimothy;
    private EnemyLOS enemyLOS;

    [SetUp]
    public void SetUp()
    {
        mockEnemy = new GameObject();
        mockPlayerJimothy = new GameObject();
        mockPlayerTimothy = new GameObject();
        enemyLOS = mockEnemy.AddComponent<EnemyLOS>();

        mockPlayerJimothy.tag = "Player";
        mockPlayerJimothy.name = "Jimothy";

        mockPlayerTimothy.tag = "Player";
        mockPlayerTimothy.name = "Timothy";
    }

    // Test debugging tests

    [Test]
    public void MockEnemyNotNull()
    {
        Assert.IsNotNull(mockEnemy);
    }

    [Test]
    public void MockPlayersNotNull()
    {
        Assert.IsNotNull(mockPlayerJimothy);
        Assert.IsNotNull(mockPlayerTimothy);
    }

    [Test]
    public void EnemyLOSNotNull()
    {
        Assert.IsNotNull(enemyLOS);
    }

    [Test]
    public void MockEnemyHasComponent()
    {
        bool hasComponent;

        if (mockEnemy.GetComponent<EnemyLOS>() == null)
        {
            hasComponent = false;
        }
        else
        {
            hasComponent = true;
        }

        Assert.IsTrue(hasComponent);
    }

    // Actual EnemyLOS tests

    [Test]
    public void TestChangeTarget()
    {
        enemyLOS.ChangeTarget(mockPlayerJimothy);
        GameObject observedIniitalTarget = enemyLOS.currentTarget;
        string observedInitialTargetName = observedIniitalTarget.name;

        enemyLOS.ChangeTarget(mockPlayerTimothy);
        GameObject observedFinalTarget = enemyLOS.currentTarget;
        string observedFinalTargetName = observedFinalTarget.name;

        Assert.AreNotEqual(observedInitialTargetName, observedFinalTargetName);
    }

    [Test]
    public void TestGetDistanceToTarget()
    {
        enemyLOS.ChangeTarget(mockPlayerJimothy);
        float distance = enemyLOS.GetDistanceToTarget();
        Assert.AreEqual(0, distance);
    }
}