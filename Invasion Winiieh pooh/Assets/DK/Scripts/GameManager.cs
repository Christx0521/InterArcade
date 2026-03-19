using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int level;
    public int score;

    public int firstLevel = 2;
    public int lastLevel = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        score = 0;
        level = firstLevel;
        SceneManager.LoadScene(level);
    }

    public void LevelComplete()
    {
        score += 1000;

        if (level < lastLevel)
        {
            level++;
            SceneManager.LoadScene(level);
        }
        else
        {
            level = 0;
            SceneManager.LoadScene(0);
        }
    }

    public void LevelFailed()
    {
        SceneManager.LoadScene(level);
    }
}