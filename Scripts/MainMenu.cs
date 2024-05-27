using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public bool endScreen = false;


    public TextMeshProUGUI nameInput;

    public GameObject c1;
    public GameObject c2;

    public void saveHighScore()
    {
        Debug.Log(nameInput.text);
        PlayerPrefs.SetFloat("HighScore", PlayerPrefs.GetFloat("Score"));
        PlayerPrefs.SetString("HighScoreHolder", nameInput.text);
        PlayerPrefs.Save();

        SceneManager.LoadScene("FinishScreen");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    void Start()
    {
        if (endScreen)
        {
            float score = PlayerPrefs.GetFloat("Score");
            float highScore = PlayerPrefs.GetFloat("HighScore");
            
            if (highScore == 0 || score < highScore && score > 0)
            {
                // New High Score
                c1.SetActive(false);
                c2.SetActive(true);
            }
            else
            {
                // Normal
                c1.SetActive(true);
                c2.SetActive(false);
            }

            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
