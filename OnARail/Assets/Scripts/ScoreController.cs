using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI score;
    public GameObject gameOverPanel;
    public Image timerBar;

    public float totalTime = 30;
    public float baseScoreEffect = 5f; //represents the base # of seconds added when increasing the score
    public float scoreEffectMultipler = 0.95f; // represents the decrease in the time added when increasing score

    private int currentScore = 0;
    private float currentTime;

    private void Start()
    {
        currentTime = totalTime;
        Time.timeScale = 1;
    }

    private void Update()
    {
        score.text = currentScore.ToString();

        currentTime -= Time.deltaTime;
        timerBar.fillAmount = Mathf.Clamp(currentTime / totalTime, 0, 1);

        if(timerBar.fillAmount <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IncreaseScore()
    {
        currentScore++;
        currentTime = Mathf.Clamp(currentTime + baseScoreEffect, 0, totalTime);
        baseScoreEffect *= scoreEffectMultipler;
    }
}
