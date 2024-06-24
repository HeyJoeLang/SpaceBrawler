using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayScoreboard : MonoBehaviour
{
    static int score = 0;
    public TMP_Text scoreText;
    public AudioClip boxingSong;
    public TMP_Text timerText;
    private float timeRemaining = 120f;
    private bool timerRunning = false;
    public GameObject timeRemainingParent;
    public GameObject endGameUI;

    public enum State
    {
        YetToStart,
        Start,
        Boxing,
        Finished
    }
    public State state = State.YetToStart;
    public void IncreaseScore()
    {
        score += 10;
        scoreText.text = string.Format("{0}", score);
    }
    void Start()
    {
    }

    void Update()
    {
        switch(state)
        {
            case State.YetToStart:
                YetToStart();
                break;
            case State.Start:
                Starting();
                break;
            case State.Boxing:
                Boxing();
                break;
            case State.Finished:
                Finish();
                break;
        }
    }
    void YetToStart()
    {

    }
    void Starting()
    {
        timerRunning = true;
        GetComponent<AudioSource>().PlayOneShot(boxingSong);
        state = State.Boxing;
    }
    void Boxing()
    {

        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                UpdateTimerText();
                timeRemainingParent.SetActive(false);
                endGameUI.SetActive(true);
                FindObjectOfType<GameController>().GetComponent<GameController>().EndBoxing();
                TimerEnded();
            }
        }
    }
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        // Do something when the timer ends
        Debug.Log("Timer has ended!");
    }
    void Finish()
    {

        //Rotate towards player
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 directionToCamera = transform.position - cameraPosition;
        directionToCamera.y = 0;
        if (directionToCamera.sqrMagnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = targetRotation;
        }
    }
}
