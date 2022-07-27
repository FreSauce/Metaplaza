using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public NetworkManager networkManager;
    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        SetCursorState(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);

        GameIsPaused = true;
        SetCursorState(false);
    }

    public void LoadMenu()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        //networkManager.LeaveRoom();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -2);
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
