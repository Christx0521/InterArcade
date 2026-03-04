using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BK_GameManagerBugs : MonoBehaviour
{
    public GameObject gOMenu;
    public GameObject pauseMenu;

    public Button restartButton;
    public Button resumeButton;
    public Button quitButton;

    public BK_PlayerController player;
    public BK_ShieldController[] shields;

    private bool isPaused;
    private bool isGameOver;

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;

        if (gOMenu != null)
            gOMenu.SetActive(false);

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(GameReset);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    public void TogglePause()
    {
        if (isGameOver) return;

        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(1f);

        if (player == null) yield break;

        player.transform.position = new Vector2(0, player.transform.position.y);
        player.Revive();

        player.invincible = true;
        yield return StartCoroutine(player.Blink(2f, 0.15f));
        player.invincible = false;
    }

    public IEnumerator WaveReset()
    {
        yield return new WaitForSeconds(2f);

        BK_InvaderMovement inv = Object.FindFirstObjectByType<BK_InvaderMovement>();
        if (inv == null) yield break;

        inv.NextRound();

        foreach (Transform alien in inv.transform)
            alien.gameObject.SetActive(true);

        inv.transform.position = inv.startPosition;

        foreach (BK_ShieldController s in shields)
            if (s != null)
                s.ResetShield();
    }

    public void GameOver()
    {
        isGameOver = true;

        if (gOMenu != null)
            gOMenu.SetActive(true);

        Time.timeScale = 0f;
    }

    public void GameReset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
