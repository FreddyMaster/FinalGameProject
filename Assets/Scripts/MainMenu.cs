using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartButton;
    public GameObject QuitButton;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuitGame()
    {
        if (QuitButton != null)
        {
            Application.Quit();
            Debug.Log("Quitting Game");
        }
    }

    public void StartGame()
    {
        if (StartButton != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
