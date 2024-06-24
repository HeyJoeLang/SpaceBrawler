using System.Collections;
using UnityEngine;

public class StartBoxingGlove : MonoBehaviour
{
    public Vector3 target;
    public Transform leftController, rightController;
    public ParticleSystem glow;
    float rotationSpeed = 100f;
    enum State
    {
        MovingToTarget,
        StartingParticles,
        Still
    }
    State state = State.MovingToTarget;
    void Update()
    {
        switch(state)
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

        if (distanceLeft < .75f ||
            distanceRight < .75f)
        {
            FindObjectOfType<GameController>().GetComponent<GameController>().StartBoxing();
        }
    }
}
