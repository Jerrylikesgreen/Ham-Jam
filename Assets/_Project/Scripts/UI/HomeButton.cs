using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    // Call this from the Button OnClick()
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
