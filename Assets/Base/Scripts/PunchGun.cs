using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class PunchGun : MonoBehaviour
{
    [SerializeField]
    private OVRInput.RawButton shootPunchAction;
    [SerializeField]
    private Transform punchEndTransform;
    [SerializeField]
    private Transform punchStartTransform;
    [SerializeField]
    private Transform punchGloveObject;
    [SerializeField]
    private float punchTime = 0.5f;

    [SerializeField]
    private ParticleSystem shotParticles;
    [SerializeField]
    private AudioClip shootSound;

    private AudioSource audioSource;
    private bool isPunching = false;
    private float punchTimer = 0.0f;
    private float halfPunchTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        halfPunchTime = punchTime / 2.0f;
    }

    void Update()
    {
        if (OVRInput.Get(shootPunchAction) && !isPunching)
        {
            StartPunch();
        }

        if (isPunching)
        {
            UpdatePunch();
        }
    }
    void StartPunch()
    {
        audioSource.PlayOneShot(shootSound);
        isPunching = true;
        punchTimer = 0.0f;
        shotParticles.Play();
    }
    void UpdatePunch()
    {
        punchTimer += Time.deltaTime;
        if (punchTimer <= halfPunchTime) // Punching forward
        {
            float t = punchTimer / halfPunchTime;
            punchGloveObject.position = Vector3.Lerp(punchStartTransform.position, punchEndTransform.position, t);
        }
        else if (punchTimer <= punchTime) // Recoiling back
        {

            float t = (punchTimer - halfPunchTime) / halfPunchTime;
            punchGloveObject.position = Vector3.Lerp(punchEndTransform.position, punchStartTransform.position, t);
        }
        else // Punch complete
        {
            punchGloveObject.position = punchStartTransform.position;
            isPunching = false;
        }
    }
}