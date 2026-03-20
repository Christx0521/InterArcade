using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject scoreValue;
    [SerializeField] private GameObject gameOverLabel;

    private int score;

    public void IncrementScore()
    {
        score = score + 10;
        scoreValue.GetComponent<TMP_Text>().text = score.ToString();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverLabel.SetActive(true);
    }
}
