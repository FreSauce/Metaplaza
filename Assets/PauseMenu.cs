using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!ShoppingMenu.MenuOpen)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        SetCursorState(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        SetCursorState(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
