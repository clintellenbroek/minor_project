using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenOptions()
    {
        // later
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}