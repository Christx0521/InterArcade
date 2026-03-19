using UnityEngine;

public class PlaytestStarter : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(StartNextLevel), 0.1f);
    }

    private void StartNextLevel()
    {
        GameManager.Instance.LevelComplete();
    }
}