using UnityEngine;

public class PlaytestStarter : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(StartGame), 0.1f);
    }

    private void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }
        else
        {
            Debug.LogError("No hay GameManager.Instance.");
        }
    }
}