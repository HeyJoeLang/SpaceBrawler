using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Collide_Wall : MonoBehaviour
{
    public ParticleSystem wallExplodePrefab;
    float ExplosionForce = 1000.0f;
    bool isExploding = false;
    float explodeTime = 0.5f;
    float explodeTimer;
    public AudioClip hitWallClip;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        if(ColliderController.Instance.HitWall(layer))
        {
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body != null)
            {
                if (body.isKinematic)
                {
                    body.isKinematic = false;
                    body.AddExplosionForce(ExplosionForce, transform.position, 1.0f);
                    Destroy(body, 2.0f);
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
    }
}
