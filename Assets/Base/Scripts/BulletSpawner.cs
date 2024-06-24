using Bytesized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    /*[SerializeField]
    private Transform trackingspace;

    [SerializeField]
    private GameObject controllerTransform;

    [SerializeField]
    private OVRInput.Controller controller;
    */
    [SerializeField]
    private OVRInput.RawButton shootBallAction;
    /*
    [SerializeField]
    private float addedForce = 0.25f;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private GameObject ballPrefab;

    private GameObject ball;
    */
    public LayerMask ShardLayers;
    public float ExplosionRadius;
    public float ExplosionForce;

    private void Update()
    {
        if (OVRInput.Get(shootBallAction))
        {
            Debug.Log(shootBallAction.ToString() + " was pressed!");
            /*ball = Instantiate(ballPrefab, controllerTransform.transform.position + offset, Quaternion.identity);

            var ballPos = ball.transform.position;
            var vel = trackingspace.rotation * OVRInput.GetLocalControllerVelocity(controller);
            var angVel = OVRInput.GetLocalControllerAngularVelocity(controller);

            ball.GetComponent<BouncingBallLogic>().Release(ballPos, vel, angVel);
            ball.GetComponent<Rigidbody>().AddForce(controllerTransform.transform.forward * addedForce);
            */
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, ShardLayers))
            {
                Debug.Log("Hit object : " + hit.collider.gameObject.name);
                Explode(hit.point);
            }
        }
    }

    void Explode(Vector3 position)
    {
        foreach (var shard in Physics.OverlapSphere(position, ExplosionRadius, ShardLayers))
        {
            var rb = shard.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddExplosionForce(ExplosionForce, position, ExplosionRadius);
            shard.gameObject.AddComponent<AutoDestruct>().Time = 3.0f;
        }
    }
}