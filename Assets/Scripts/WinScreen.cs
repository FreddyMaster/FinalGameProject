using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
  // Start is called before the first frame update
    public GameObject RestartButton;
    public GameObject MainMenuButton;
    public GameObject QuitButton;

    public void QuitGame()
    {
        if (QuitButton != null)
        {
            Application.Quit();
        }
    }

    public void RestartGame()
    {
        if (RestartButton != null)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void MainMenu()
    {
        if (MainMenuButton != null)
        {
            SceneManager.LoadScene(0);
        }
    }
}
