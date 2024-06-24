using Bytesized;
using Oculus.Interaction;
using UnityEngine;

public class BreakWallTrigger : MonoBehaviour
{

    public LayerMask ShardLayer;
    public LayerMask FurnitureLayer;
    public LayerMask UFOLayer;
    public LayerMask PunchGloveLayer;
    public ParticleSystem wallExplodePrefab;
    public float ExplosionForce = 100.0f;
    bool isExploding = false;
    float explodeTime = 0.5f;
    float explodeTimer;
    public AudioClip hitWallClip;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null )
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void FixedUpdate()
    {
        if(isExploding)
        {
            explodeTimer -= Time.deltaTime;
            if(explodeTimer <= 0 ) 
            {
                isExploding = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        if (layer == ColliderUtils.LayerMaskToLayer(ShardLayer))
        {
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body != null)
            {
                if (body.isKinematic)
                {
                    body.isKinematic = false;
                    body.AddExplosionForce(ExplosionForce, transform.position, 1.0f);
                    body.gameObject.AddComponent<AutoDestruct>().Time = 3.0f;
                    if (isExploding == false)
                    {
                        if(hitWallClip != null)
                        {
                            audioSource.PlayOneShot(hitWallClip);
                        }
                        wallExplodePrefab.gameObject.SetActive(true);
                        isExploding = true;
                        explodeTimer = explodeTime;
                        wallExplodePrefab.Play();
                    }
                }
            }
        }
        if (layer == ColliderUtils.LayerMaskToLayer(UFOLayer) && transform.gameObject.layer == ColliderUtils.LayerMaskToLayer(PunchGloveLayer))
        {
            other.GetComponent<EnemyShip>().Explode();
        }
        /*if (layer == ColliderUtils.LayerMaskToLayer(FurnitureLayer))
        {
            Debug.LogFormat("{0} hit Furniture: {1}", transform.name, other.gameObject.name);
            if(transform.gameObject.layer == ColliderUtils.LayerMaskToLayer(UFOLayer))
            {
                GetComponent<EnemyShip>().StartBounce(other.ClosestPoint(transform.position));
            }
        }
        */
        
    }
}
