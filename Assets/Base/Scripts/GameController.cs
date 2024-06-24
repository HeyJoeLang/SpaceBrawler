using Meta.XR.MRUtilityKit;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float boxingTime = 120.0f;
    public GameObject controllerLeftModel, controllerRightModel;
    public GameObject punchGunLeft, punchGunRight;
    public EnemySpawner enemySpawner;
    public AudioClip boxingBell;
    public GameObject StartMenu;
    public GameObject Ceiling;
    public StartBoxingGlove startBoxingGlove;
    public GameplayScoreboard scoreboard;
    public Animator fade;
    public ParticleSystem[] transfromParticles;
    public GameObject auroraParticles;
    AudioSource audioSource;

    //public OVRScreenFade screenFade;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitForRoomCreation());
    }
    IEnumerator WaitForRoomCreation()
    {
        int attempts = 3;
        while (Ceiling == null || attempts == 0)
        {
            Ceiling = GameObject.Find("CEILING");
            if (Ceiling != null)
            {
                break;
            }
            yield return new WaitForSeconds(1);
            attempts--;
        }
        CenterObjectInRoom(StartMenu.transform);
        CenterObjectInRoom(scoreboard.transform);
    }
    void CenterObjectInRoom(Transform objectToCenter)
    {
        objectToCenter.position = new Vector3(Ceiling.transform.position.x, objectToCenter.position.y, Ceiling.transform.position.z);

        //Rotate towards player
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 directionToCamera = objectToCenter.position - cameraPosition;
        directionToCamera.y = 0;
        if (directionToCamera.sqrMagnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            objectToCenter.rotation = targetRotation;
        }
    }
    IEnumerator TransformControllers()
    {
        for (int i = 0; i < transfromParticles.Length; i++)
        {
            transfromParticles[i].gameObject.SetActive(true);
            transfromParticles[i].Play();
        }
        yield return new WaitForSeconds(2);
        for (int i = 0; i < transfromParticles.Length; i++)
        {
            transfromParticles[i].gameObject.SetActive(false);
        }
    }
    public void StartBoxing()
    {
        auroraParticles.SetActive(true);
        audioSource.Stop();
        StartCoroutine(TransformControllers());
        scoreboard.transform.position = startBoxingGlove.transform.position + new Vector3(0, 0.25f, 0);
        Destroy(startBoxingGlove.gameObject);
        controllerLeftModel.SetActive(false);
        controllerRightModel.SetActive(false);
        punchGunLeft.SetActive(true);
        punchGunRight.SetActive(true);
        enemySpawner.gameObject.SetActive(true);
        scoreboard.gameObject.SetActive(true);
        scoreboard.state = GameplayScoreboard.State.Start;
    }
    public void PlayGame()
    {
        GetComponent<AudioSource>().PlayOneShot(boxingBell);
        startBoxingGlove.transform.position = Ceiling.transform.position + new Vector3(0, 3, 0);
        startBoxingGlove.target = StartMenu.transform.position + new Vector3(0, -0.25f, 0);
        StartMenu.GetComponent<Animator>().SetTrigger("Shrink");
        Destroy(StartMenu, 2.0f);
        startBoxingGlove.gameObject.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Reset()
    {
        StartCoroutine(StallReset());
    }
    public void EndBoxing()
    {
        audioSource.Play();
        StartCoroutine(TransformControllers());
        controllerLeftModel.SetActive(true);
        controllerRightModel.SetActive(true);
        punchGunLeft.SetActive(false);
        punchGunRight.SetActive(false);
        enemySpawner.EndGameExplodeUFOs();

    }
    IEnumerator StallReset()
    {
       // screenFade.FadeOut();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
    }
}
