using UnityEngine;
using TMPro;

public class BK_ScoreManager : MonoBehaviour
{
    public TMP_Text scoreTxt;
    public TMP_Text hiScoreTxt;

    public float scoreCount;
    public float hiScoreCount;

    void Start()
    {
        hiScoreCount = PlayerPrefs.GetFloat("HighScore", 0);
        scoreCount = 0;
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    public void AddScore(int pointsToAdd)
    {
        scoreCount += pointsToAdd;

        if (scoreCount > hiScoreCount)
        {
            hiScoreCount = scoreCount;
            PlayerPrefs.SetFloat("HighScore", hiScoreCount);
        }
    }

    void UpdateUI()
    {
        scoreTxt.SetText("Score: {0}", Mathf.Round(scoreCount));
        hiScoreTxt.SetText("TOP: {0}", Mathf.Round(hiScoreCount));
    }
}
