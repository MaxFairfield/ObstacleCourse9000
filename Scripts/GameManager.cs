using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // UI elements for displaying timer and high score
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI highScoreText;

    // Player and checkpoint indicator game objects
    public GameObject player;
    public GameObject checkpointIndicator;

    // Variables to track game start time and states
    private float startTime;

    private bool gameStarted;
    private bool gameFinished;

    // Flag to reset high score for debugging or game resets
    public bool resetHighScore = false;

    // Pause menu related variables
    public GameObject pauseMenu;
    private bool isPaused = false;

    // High score holder name
    private string highScoreHolder;

    // Converts a float time value to a formatted string (mm:ss:ff)
    string GetScore(float time)
    {
        TimeSpan timeElapsed = TimeSpan.FromSeconds(time);

        string formattedTime = string.Format(
            "{0:00}:{1:00}:{2:00}",
            timeElapsed.Minutes,
            timeElapsed.Seconds,
            timeElapsed.Milliseconds / 10
        );

        return formattedTime;
    }

    // Starts the game and initializes the timer
    public void StartGame()
    {
        if (!gameStarted)
        {
            startTime = Time.time;
            gameStarted = true;
        }
    }

    // Finishes the game, calculates the score, and saves it
    public void FinishGame()
    {
        if (gameStarted && !gameFinished)
        {
            gameFinished = true;

            float currentScore = Time.time - startTime;

            timerText.text = GetScore(currentScore);

            PlayerPrefs.SetFloat("Score", currentScore);
            PlayerPrefs.Save();

            SceneManager.LoadScene("FinishScreen");
        }
    }

    // Initializes high score from PlayerPrefs and resets if flag is set
    void Start()
    {
        highScoreHolder = PlayerPrefs.GetString("HighScoreHolder");

        if (resetHighScore)
        {
            PlayerPrefs.SetFloat("HighScore", 0);
            PlayerPrefs.Save();
        }

        float highScore = PlayerPrefs.GetFloat("HighScore");

        if (highScoreText != null && highScoreHolder != null && highScore != 0f)
        {
            highScoreText.text = "High Score: " + GetScore(highScore) + " (" + highScoreHolder + ")";
        }
    }

    // Updates timer, checks for checkpoints, and handles pause menu input
    void Update()
    {
        if (gameStarted && !gameFinished)
        {
            timerText.text = GetScore(Time.time - startTime);
        }

        PlayerController pc = player.GetComponent<PlayerController>();

        if (pc.checkpoint != null)
        {
            Vector3 targetPosition = new Vector3(pc.checkpoint.transform.position.x, 5, pc.checkpoint.transform.position.z);

            Vector3 smoothedPosition = Vector3.Lerp(checkpointIndicator.transform.position, targetPosition, 10 * Time.deltaTime);
            checkpointIndicator.transform.position = smoothedPosition;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resumes the game from pause
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Pauses the game and shows pause menu
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Quits the game application
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
