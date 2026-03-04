using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuInicio;
    [SerializeField] private GameObject menuJuegos;

    private void Start()
    {
        menuInicio.SetActive(true);
        menuJuegos.SetActive(false);
    }

    public void IrAMenuJuegos()
    {
        menuInicio.SetActive(false);
        menuJuegos.SetActive(true);
    }

    public void VolverAlInicio()
    {
        menuJuegos.SetActive(false);
        menuInicio.SetActive(true);
    }

    public void CargarJuego(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }
}