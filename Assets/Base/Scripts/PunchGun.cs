using UnityEngine;

public class PunchGun : MonoBehaviour
{

    [SerializeField]
    private OVRInput.RawButton shootPunchAction;
    [SerializeField]
    private ParticleSystem shotParticles;
    public Transform punchTargetTransform;
    public Transform startTransform;
    public Transform punchObject; // Assign the child object to this in the Inspector
    public float punchDistance = 3.0f; // Distance to punch forward
    public float punchTime = 0.5f; // Time to complete the punch and recoil

    private Vector3 originalPosition;
    private bool isPunching = false;
    private float punchTimer = 0.0f;
    public AudioClip shootSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalPosition = punchObject.localPosition;
    }

    void Update()
    {
        if (OVRInput.Get(shootPunchAction) && !isPunching)
        {
            audioSource.PlayOneShot(shootSound);
            isPunching = true;
            punchTimer = 0.0f;
            shotParticles.Play();
        }

        if (isPunching)
        {
            punchTimer += Time.deltaTime;
            float halfPunchTime = punchTime / 2.0f;

            if (punchTimer <= halfPunchTime)
            {
                // Punching forward
                float t = punchTimer / halfPunchTime;
                punchObject.position = Vector3.Lerp(startTransform.position, punchTargetTransform.position, t);
            }
            else if (punchTimer <= punchTime)
            {
                // Recoiling back
                float t = (punchTimer - halfPunchTime) / halfPunchTime;
                punchObject.position = Vector3.Lerp(punchTargetTransform.position, startTransform.position, t);
            }
            else
            {
                // Punch complete
                punchObject.position = startTransform.position;
                isPunching = false;
            }
        }
    }
}