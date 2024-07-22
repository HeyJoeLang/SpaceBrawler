using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace heyjoelang
{
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
        bool hasExploded = false;
        private float stallDestroyTime;
        public float GetStallDestroyTime()
        {
            return stallDestroyTime;
        }

        public enum State_EnemyShip
        {
            FlyingToTarget,
            Wandering,
            Bouncing
        }

        [SerializeField]
        private State_EnemyShip currentState;
        private float wanderTime;
        private Vector3 wanderTarget;

        private float bounceTime = .5f;
        private float bounceTimer;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentState = State_EnemyShip.FlyingToTarget;
            target = Camera.main != null ? Camera.main.transform : null;
        }

        void Update()
        {
            if (target == null) return;

            switch (currentState)
            {
                case State_EnemyShip.FlyingToTarget:
                    FlyToTarget();
                    break;
                case State_EnemyShip.Wandering:
                    WanderAround();
                    break;
            }
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        private void FlyToTarget()
        {
            if (target == null) return;

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) < minWanderRadius)
            {
                currentState = State_EnemyShip.Wandering;
                SetNewWanderTarget();
            }
        }

        private void WanderAround()
        {
            if (target == null) return;

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
            stallDestroyTime = Random.Range(0f, 2f);
            yield return new WaitForSeconds(stallDestroyTime);
            endGameParticles.gameObject.SetActive(true);
            endGameParticles.Play();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
            Destroy(gameObject, 1f);
        }

        private void SetNewWanderTarget()
        {
            if (target == null) return;

            if (Vector3.Distance(transform.position, target.position) > maxWanderRadius)
            {
                currentState = State_EnemyShip.FlyingToTarget;
            }
            Vector3 newWanderPosition = transform.position + Random.insideUnitSphere * wanderRadius;
            if (newWanderPosition.y < 0)
            {
                newWanderPosition.y = Mathf.Abs(newWanderPosition.y);
            }
            wanderTarget = newWanderPosition;
        }

        public State_EnemyShip GetEnemyShipState()
        {
            return currentState;
        }

        public void SetEnemyShipState(State_EnemyShip sate)
        {
            currentState = sate;
        }

        public Transform GetTarget()
        {
            if (target == null)
            {
                Debug.LogError("No Target assigned yet attempting to be retrieved");
            }
            return target;
        }

        public void SetTarget(Transform value)
        {
            target = value;
        }
        public void Explode()
        {
            if (hasExploded)
            {
                return;
            }
            hasExploded = true;
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
            if (explodeParticles != null)
            {
                explodeParticles.gameObject.SetActive(true);
                explodeParticles.Play();
            }
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
            Debug.Log("MeshRenderer and MeshCollider are disabled");

            if (GameplayScoreboard.Instance != null)
            {
                GameplayScoreboard.Instance.IncreaseScore();
                Debug.Log("GameplayScoreboard score increased");
            }
            else
            {
                Debug.LogError("GameplayScoreboard instance is null");
            }

            Destroy(gameObject, 2);
        }
    }
}
