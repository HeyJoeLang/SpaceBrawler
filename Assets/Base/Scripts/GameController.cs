using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameController : Singleton<GameController>
{
    #region Variables

    [Header("Toggle Game Objects")]
    [SerializeField] private GameObject controllerLeftModel;
    [SerializeField] private GameObject controllerRightModel;
    [SerializeField] private GameObject punchGunLeft;
    [SerializeField] private GameObject punchGunRight;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private StartBoxingGlove startBoxingGlove;
    [SerializeField] private GameplayScoreboard scoreboard;
    [SerializeField] private ParticleSystem auroraParticles;
    [SerializeField] private GameObject ceiling;

    [Header("Audio")]
    [SerializeField] private AudioClip boxingBell;
    [SerializeField] private AudioClip menuBackgroundMusic;
    [SerializeField] private AudioClip boxingBackgroundMusic;
    private AudioSource audioSource;

    [Header("UI")]
    [SerializeField] private Button button_PlayGame;
    [SerializeField] private Button button_Restart;
    [SerializeField] private Button button_Quit;
    [SerializeField] private GameObject timeRemainingParent;
    [SerializeField] private GameObject endGameUI;

    private float boxingDuration = 120.0f;
    private float boxingTimeLeft = 120.0f;

    private const float StartBoxingGloveOffsetY = 3.0f;
    private const float ScoreboardOffsetY = 0.25f;
    private const float StartMenuShrinkDelay = 2.0f;

    private Animator startMenuAnimator;

    private enum State
    {
        Main_Menu,
        Start_Hit_Glove,
        Update_Hit_Glove,
        Start_Boxing,
        Update_Boxing,
        End_Boxing,
        Finished
    }
    private State gameState = State.Main_Menu;

    #endregion
    #region UnityFunctions

    private void Start()
    {
        CacheReferences();
        StartCoroutine(WaitForRoomCreation());
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.clip = menuBackgroundMusic;
    }

    void OnEnable()
    {
        AddListeners();
    }
    void OnDisable()
    {
        RemoveListeners();
    }
    private void Update()
    {
        switch (gameState)
        {
            case State.Start_Boxing:
                StartBoxing();
                break;
            case State.Start_Hit_Glove:
                StartHitGlove();
                break;
            case State.Update_Hit_Glove:
                UpdateHitGlove();
                break;
            case State.Update_Boxing:
                UpdateBoxing();
                break;
            case State.End_Boxing:
                EndBoxing();
                break;
            case State.Finished:
                Finished();
                break;
        }
    }

    #endregion
    #region StateFunctions

    private void StartHitGlove()
    {
        audioSource.PlayOneShot(boxingBell);

        startMenuAnimator.SetTrigger("Shrink");
        Destroy(startMenu, StartMenuShrinkDelay);

        startBoxingGlove.gameObject.transform.position = ceiling.transform.position + new Vector3(0, StartBoxingGloveOffsetY, 0);
        startBoxingGlove.SetGloveTarget(startMenu.transform.position + new Vector3(0, -0.25f, 0));
        startBoxingGlove.gameObject.SetActive(true);

        gameState = State.Update_Hit_Glove;
    }
    void UpdateHitGlove()
    {
    }
    public void StartBoxing()
    {
        audioSource.clip = boxingBackgroundMusic;

        auroraParticles.gameObject.SetActive(true);
        ParticleSystem.MainModule main = auroraParticles.main;
        main.startDelay = boxingDuration / 2.0f;
        auroraParticles.Play();

        boxingDuration = boxingTimeLeft = boxingBackgroundMusic.length;

        scoreboard.transform.position = startBoxingGlove.transform.position + new Vector3(0, ScoreboardOffsetY, 0);
        Destroy(startBoxingGlove.gameObject);

        controllerLeftModel.SetActive(false);
        controllerRightModel.SetActive(false);
        punchGunLeft.SetActive(true);
        punchGunRight.SetActive(true);
        enemySpawner.gameObject.SetActive(true);
        scoreboard.gameObject.SetActive(true);
        gameState = State.Update_Boxing;
    }
    private void UpdateBoxing()
    {
        if (boxingTimeLeft > 0)
        {
            boxingTimeLeft -= Time.deltaTime;
        }
        else
        {
            boxingTimeLeft = 0;
            gameState = State.End_Boxing;
        }
        GameplayScoreboard.Instance.UpdateTimerText(boxingTimeLeft);
    }
    private void EndBoxing()
    {
        timeRemainingParent.SetActive(false);
        endGameUI.SetActive(true);
        audioSource.clip = menuBackgroundMusic;
        controllerLeftModel.SetActive(true);
        controllerRightModel.SetActive(true);
        punchGunLeft.SetActive(false);
        punchGunRight.SetActive(false);
        enemySpawner.EndGameExplodeUFOs();

        timeRemainingParent.SetActive(false);
        endGameUI.SetActive(true);
        gameState = State.Finished;
    }

    private void Finished()
    {
    }

    #endregion
    #region UtilityFunctions
    private void CacheReferences()
    {
        audioSource = GetComponent<AudioSource>();
        startMenuAnimator = startMenu.GetComponent<Animator>();
    }

    private void AddListeners()
    {
        button_PlayGame.onClick.AddListener(StartHitGlove);
        button_Restart.onClick.AddListener(Restart);
        button_Quit.onClick.AddListener(Quit);
        startBoxingGlove.OnGloveContact += StartBoxing;
    }
    private void RemoveListeners()
    {
        button_PlayGame.onClick.RemoveListener(StartHitGlove);
        button_Restart.onClick.RemoveListener(Restart);
        button_Quit.onClick.RemoveListener(Quit);
        startBoxingGlove.OnGloveContact -= StartBoxing;
    }

    IEnumerator WaitForRoomCreation()
    {
        const int maxAttempts = 10;
        int attempts = maxAttempts;

        while (ceiling == null && attempts > 0)
        {
            ceiling = GameObject.Find("CEILING");
            if (ceiling != null)
            {
                break;
            }
            yield return new WaitForSeconds(.5f);
            attempts--;
        }

        if (ceiling != null)
        {
            CenterObjectInRoom(startMenu.transform);
            CenterObjectInRoom(scoreboard.transform);
        }
    }

    private void CenterObjectInRoom(Transform objectToCenter)
    {
        objectToCenter.position = new Vector3(ceiling.transform.position.x, objectToCenter.position.y, ceiling.transform.position.z);
        Vector3 directionToCamera = objectToCenter.position - Camera.main.transform.position;
        directionToCamera.y = 0;
        objectToCenter.rotation = Quaternion.LookRotation(directionToCamera);
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
}