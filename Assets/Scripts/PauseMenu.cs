using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  // Start is called before the first frame update
    public GameObject ResumeButton;
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

    public void ResumeGame()
    {
        if (ResumeButton != null)
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }

    public void RestartGame()
    {
        if (RestartButton != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
