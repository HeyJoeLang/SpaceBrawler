using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using heyjoelang;

public class EnemyShipTests
{
    GameObject enemyShipPrefab;

    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void EnemyShipTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    [UnityTest]
    public IEnumerator EnemyShipTestsWithEnumeratorPasses()
    {
        var shipObject = new GameObject("TestShipMoveTarget");
        shipObject.AddComponent<EnemyShip>();
        yield return null;
    }
}
