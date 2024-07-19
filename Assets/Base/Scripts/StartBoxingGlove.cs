using System;
using UnityEngine;

namespace heyjoelang
{
    public class StartBoxingGlove : MonoBehaviour
    {
        public event Action OnGloveContact;

        [SerializeField] private Transform leftController, rightController;
        [SerializeField] private ParticleSystem glow;

        private Vector3 target;
        private float rotationSpeed = 100f;
        private float minContactDistance = .75f;

        public void SetGloveTarget(Vector3 targetPosition)
        {
            target = targetPosition;
        }
        enum State
        {
            MovingToTarget,
            StartingParticles,
            Still
        }
        State state = State.MovingToTarget;
        void Update()
        {
            switch (state)
            {
                case State.MovingToTarget:
                    MoveToTarget();
                    break;
                case State.StartingParticles:
                    StartParticles();
                    break;
                case State.Still:
                    Still();
                    break;
            }
        }

        private void MoveToTarget()
        {
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * 1.0f * Time.deltaTime;

            if (Vector3.Distance(transform.position, target) < .1f)
            {
                state = State.StartingParticles;
            }
        }
        private void StartParticles()
        {
            glow.gameObject.SetActive(true);
            glow.Play();
            state = State.Still;
        }
        private void Still()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            float distanceLeft = Vector3.Distance(transform.position, leftController.position);
            float distanceRight = Vector3.Distance(transform.position, rightController.position);

            if (distanceLeft < minContactDistance || distanceRight < minContactDistance)
            {
                OnGloveContact?.Invoke();
            }
        }
    }
}
