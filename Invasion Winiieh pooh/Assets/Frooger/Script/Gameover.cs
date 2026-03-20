using UnityEngine;
using TMPro;

public class Gameover : MonoBehaviour
{
    [SerializeField] private GameObject gameOverLabel;
    [SerializeField] private GameObject Winner;

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverLabel.SetActive(true);
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        Winner.SetActive(true);
    }
}
