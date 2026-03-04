using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class PAC_GameManager : MonoBehaviour
{
    public static PAC_GameManager Instance { get; private set; }

    [Header("Gameplay")]
    [SerializeField] private PAC_Ghost[] ghosts;
    [SerializeField] private PAC_Pacman pacman;
    [SerializeField] private Transform pellets;
    [SerializeField] private PAC_FruitSpawner fruitSpawner;

    [Header("UI")]
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text highScoreText;

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private string menuSceneName = "Menu";

    [SerializeField]
    private int[] fruitPoints =
    {
        100, 300, 500, 700, 1000, 2000, 3000, 5000
    };

    public int score { get; private set; }
    public int lives { get; private set; } = 3;
    public int highScore { get; private set; }

    private int ghostMultiplier = 1;
    private int pelletsEaten;
    private int fruitScoreIndex;

    private bool isPaused;
    private bool isGameOver;
    private bool pacmanInvincible = false;

    private void Awake()
    {
        if (Instance != null)
            DestroyImmediate(gameObject);
        else
            Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);

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

        LoadHighScore();
        NewGame();
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
        SceneManager.LoadScene(menuSceneName);
    }

    public void GameReset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("PAC_HighScore", 0);
        highScoreText.text = "HI " + highScore.ToString();
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("PAC_HighScore", highScore);
        PlayerPrefs.Save();
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);

        foreach (PAC_Ghost ghost in ghosts)
        {
            PAC_Movement movement = ghost.GetComponent<PAC_Movement>();
            if (movement != null)
                movement.speed = 8f;
        }

        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in pellets)
            pellet.gameObject.SetActive(true);

        pelletsEaten = 0;
        fruitScoreIndex = 0;

        IncreaseGhostSpeed();
        fruitSpawner?.ResetFruits();
        ResetState();
    }

    private void IncreaseGhostSpeed()
    {
        foreach (PAC_Ghost ghost in ghosts)
        {
            if (ghost.gameObject.layer == LayerMask.NameToLayer("Ghost"))
            {
                PAC_Movement movement = ghost.GetComponent<PAC_Movement>();
                if (movement != null)
                    movement.speed += 0.1f;
            }
        }
    }

    private void ResetState()
    {
        foreach (PAC_Ghost ghost in ghosts)
            ghost.ResetState();

        pacman.ResetState();
    }

    private void ResetAfterDeath()
    {
        foreach (PAC_Ghost ghost in ghosts)
            ghost.ResetState();

        pacman.ResetState();
    }

    private void GameOver()
    {
        isGameOver = true;

        if (gameOverText != null)
            gameOverText.enabled = true;

        if (gameOverMenu != null)
            gameOverMenu.SetActive(true);

        foreach (PAC_Ghost ghost in ghosts)
            ghost.gameObject.SetActive(false);

        pacman.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives;
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');

        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "HI " + highScore.ToString();
            SaveHighScore();
        }
    }

    public void AddScore(int value)
    {
        SetScore(score + value);
    }

    public void PacmanEaten()
    {
        if (pacmanInvincible) return;

        pacmanInvincible = true;

        pacman.DeathSequence();
        SetLives(lives - 1);

        if (lives > 0)
            Invoke(nameof(ResetAfterDeath), 3f);
        else
            GameOver();

        Invoke(nameof(ResetPacmanInvincible), 3f);
    }

    private void ResetPacmanInvincible()
    {
        pacmanInvincible = false;
    }

    public void GhostEaten(PAC_Ghost ghost)
    {
        AddScore(ghost.points * ghostMultiplier);
        ghostMultiplier++;
    }

    public void PelletEaten(PAC_Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        AddScore(pellet.points);

        pelletsEaten++;

        if (pelletsEaten % 30 == 0)
            fruitSpawner?.TrySpawnFruit();

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerPelletEaten(PAC_PowerPellet pellet)
    {
        foreach (PAC_Ghost ghost in ghosts)
            ghost.frightened.Enable(pellet.duration);

        PelletEaten(pellet);

        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    public void FruitEaten()
    {
        if (fruitScoreIndex < fruitPoints.Length)
        {
            AddScore(fruitPoints[fruitScoreIndex]);
            fruitScoreIndex++;
        }
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
            if (pellet.gameObject.activeSelf)
                return true;

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }
}