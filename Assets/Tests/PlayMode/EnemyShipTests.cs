using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using heyjoelang;

public class EnemyShipTests
{
    GameObject enemyShipPrefab;
    GameObject enemyShip;
    EnemyShip enemyShipComponent;
    GameObject vrRig;

    [SetUp]
    public void Setup()
    {
        // Setup a new GameObject and attach the EnemyShip component
        enemyShip = Resources.Load<GameObject>("Prefabs/UFO");
        Assert.IsNotNull(enemyShip, "UFO prefab could not be loaded. Ensure the prefab is placed in 'Assets/Base/Resources/Prefabs'.");
        enemyShip = GameObject.Instantiate(enemyShip);
        enemyShipComponent = enemyShip.GetComponent<EnemyShip>();

        vrRig = Resources.Load<GameObject>("Prefabs/OVRCameraRig");
        Assert.IsNotNull(vrRig, "OVRCameraRig prefab could not be loaded. Ensure the prefab is placed in 'Assets/Base/Resources/Prefabs'.");
        vrRig = GameObject.Instantiate(vrRig);

        // Load and instantiate the GameplayScoreboard prefab
        var scoreboardPrefab = Resources.Load<GameObject>("Prefabs/GameplayScoreboard");
        Assert.IsNotNull(scoreboardPrefab, "GameplayScoreboard prefab could not be loaded. Ensure the prefab is placed in 'Assets/Base/Resources/Prefabs'.");
        GameObject.Instantiate(scoreboardPrefab);

        // Ensure the target is set before running tests
        enemyShipComponent.SetTarget(vrRig.transform);
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up after each test
        GameObject.DestroyImmediate(enemyShip);
        GameObject.DestroyImmediate(vrRig);
        if (GameplayScoreboard.Instance != null)
        {
            GameObject.DestroyImmediate(GameplayScoreboard.Instance.gameObject);
        }
    }

    [Test]
    public void _01_EnemyShip_InitializesCorrectly()
    {
        // Check if the enemy ship initializes correctly
        Assert.IsNotNull(enemyShipComponent);
        Assert.AreEqual(EnemyShip.State_EnemyShip.FlyingToTarget, enemyShipComponent.GetEnemyShipState());
        Assert.AreEqual(vrRig.transform, enemyShipComponent.GetTarget());
    }

    [UnityTest]
    public IEnumerator _02_EnemyShip_FliesToTarget()
    {
        // Test if the enemy ship flies towards the target
        enemyShipComponent.SetTarget(vrRig.transform);
        enemyShipComponent.SetEnemyShipState(EnemyShip.State_EnemyShip.FlyingToTarget);

        Vector3 initialPosition = enemyShip.transform.position;
        yield return new WaitForSeconds(1.0f);

        Assert.AreNotEqual(initialPosition, enemyShip.transform.position);
    }

    [UnityTest]
    public IEnumerator _03_EnemyShip_WandersAroundTarget()
    {
        // Test if the enemy ship wanders around the target
        enemyShipComponent.SetTarget(vrRig.transform);
        enemyShipComponent.SetEnemyShipState(EnemyShip.State_EnemyShip.Wandering);

        Vector3 initialPosition = enemyShip.transform.position;
        yield return new WaitForSeconds(1.0f);

        Assert.AreNotEqual(initialPosition, enemyShip.transform.position);
    }

    [UnityTest]
    public IEnumerator _04_EnemyShip_ExplodesCorrectly()
    {
        // Test if the enemy ship explodes correctly
        enemyShipComponent.SetTarget(vrRig.transform);
        enemyShipComponent.Explode();

        // Immediately check the state of MeshRenderer and MeshCollider
        Assert.IsFalse(enemyShip.GetComponent<MeshRenderer>().enabled, "MeshRenderer should be disabled immediately after Explode() is called.");
        Assert.IsFalse(enemyShip.GetComponent<MeshCollider>().enabled, "MeshCollider should be disabled immediately after Explode() is called.");

        // Give some time for the explosion logic to fully complete
        yield return new WaitForSeconds(0.5f);

        // Check the state again after some time
        Assert.IsFalse(enemyShip.GetComponent<MeshRenderer>().enabled, "MeshRenderer should remain disabled after 0.5 seconds.");
        Assert.IsFalse(enemyShip.GetComponent<MeshCollider>().enabled, "MeshCollider should remain disabled after 0.5 seconds.");
    }

    [UnityTest]
    public IEnumerator _05_EnemyShip_EndGameExplosionCorrectly()
    {
        // Test if the enemy ship triggers the end game explosion correctly
        enemyShipComponent.SetTarget(vrRig.transform);
        enemyShipComponent.EndGameExplode();

        yield return new WaitForSeconds(enemyShipComponent.GetStallDestroyTime() + .01f);

        // Immediately check the state of MeshRenderer and MeshCollider
        Assert.IsFalse(enemyShip.GetComponent<MeshRenderer>().enabled, "MeshRenderer should be disabled after EndGameExplode() is called.");
        Assert.IsFalse(enemyShip.GetComponent<MeshCollider>().enabled, "MeshCollider should be disabled after EndGameExplode() is called.");

        yield return new WaitForSeconds(1);
        Assert.IsTrue(enemyShip == null, "EnemyShip should be destroyed after the end game explosion.");
    }
}
