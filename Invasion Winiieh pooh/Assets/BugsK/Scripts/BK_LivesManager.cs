using UnityEngine;
using UnityEngine.UI;

public class BK_LivesManager : MonoBehaviour
{
    public int livesCounter;
    public Text livesText;

    void Update()
    {
        livesText.text = "x " + livesCounter;
    }
}
