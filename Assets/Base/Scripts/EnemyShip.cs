using System.Collections;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    private Transform target; // The target the enemy ship will fly towards and shoot at
    public float speed = 5f; // Speed of the enemy ship
    public float wanderRadius = 1f; // Radius for wandering around the target
    public float minWanderRadius = 1f; // Radius around the target within which the ship can wander
    public float maxWanderRadius = 2f;
    public float bounceForce = 5f; // Force applied when bouncing
    public float rotationSpeed = 10f;
    public ParticleSystem explodeParticles;
    public ParticleSystem endGameParticles;
    public AudioClip hitSound;
    AudioSource audioSource;
    private enum State
    {
        FlyingToTarget,
        Wandering,
        Bouncing
    }

    [SerializeField]
    private State currentState;
    private float wanderTime;
    private Vector3 wanderTarget;

    private float bounceTime = .5f;
    private float bounceTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentState = State.FlyingToTarget;
        target = Camera.main.transform;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.FlyingToTarget:
                FlyToTarget();
                break;
            case State.Wandering:
                WanderAround();
                break;
        }
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void FlyToTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < minWanderRadius)
        {
            currentState = State.Wandering;
            SetNewWanderTarget();
        }
    }
    private void WanderAround()
    {
        float lerpSpeed = speed * Time.deltaTime;
        transform.position = Vector3.Slerp(transform.position, wanderTarget, lerpSpeed);
        if (Vector3.Distance(transform.position, wanderTarget) < .1f)
        {
            SetNewWanderTarget();
        }
    }
    public void EndGameExplode()
    {
        StartCoroutine(StallEndGameExplosion());
    }
    IEnumerator StallEndGameExplosion()
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        endGameParticles.gameObject.SetActive(true);
        endGameParticles.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        Destroy(gameObject, 1f);
    }
    private void SetNewWanderTarget()
    {
        if (Vector3.Distance(transform.position, target.position) > maxWanderRadius)
        {
            currentState = State.FlyingToTarget;
        }
        Vector3 newWanderPosition = transform.position + Random.insideUnitSphere * wanderRadius;
        if(newWanderPosition.y < 0)
        {
            newWanderPosition.y = Mathf.Abs(newWanderPosition.y);
        }
;        wanderTarget = newWanderPosition;
    }
    public void Explode()
    {
        audioSource.PlayOneShot(hitSound);
        explodeParticles.gameObject.SetActive(true);
        explodeParticles.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        Destroy(gameObject, 2);
       GameplayScoreboard.Instance.IncreaseScore();
    }
}