using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnStartButton ()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Frooger");
    }

    public void OnStart2Button()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SpaceRocksMain");
    }

    public void OnReturnButton()
    {
        SceneManager.LoadScene("Menu");
    }


    public void OnQuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
