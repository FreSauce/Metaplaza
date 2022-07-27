using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loginCanvas;
    public GameObject SignupCanvas;
    public GameObject MainMenuCanvas;
    public GameObject PlayBtn;
    public GameObject CharGenBtn;
    public GameObject logoutBtn;
    public GameObject SignupBtn;
    public GameObject LoginBtn;
    public void Show(TextMeshProUGUI obj)
    {
        obj.enabled = true;
    }

    public void Hide(TextMeshProUGUI obj)
    {
        obj.enabled = false;
    }

    public void Start()
    {
        Login.Show(MainMenuCanvas);

        if (PlayerPrefs.HasKey("token"))
        {
            Login.Hide(LoginBtn);
            Login.Hide(SignupBtn);
            Login.Show(PlayBtn);
            Login.Show(CharGenBtn);
            Login.Show(logoutBtn);
        }
        else
        {
            Login.Show(LoginBtn);
            Login.Show(SignupBtn);
            Login.Hide(PlayBtn);
            Login.Hide(CharGenBtn);
            Login.Hide(logoutBtn);
        }
    }

    public void Update()
    {
        Login.Show(MainMenuCanvas);
        if (PlayerPrefs.HasKey("token"))
        {
            Login.Hide(LoginBtn);
            Login.Hide(SignupBtn);
            Login.Show(PlayBtn);
            Login.Show(CharGenBtn);
            Login.Show(logoutBtn);
        }
        else
        {
            Login.Show(LoginBtn);
            Login.Show(SignupBtn);
            Login.Hide(PlayBtn);
            Login.Hide(CharGenBtn);
            Login.Hide(logoutBtn);
        }
    }
    public void LoadLoginScene()
    {
        Login.Hide(SignupCanvas);
        Login.Show(loginCanvas);
        Login.Hide(LoginBtn);
        Login.Hide(SignupBtn);
    }

    public void LoadSignupScene()
    {
        Login.Show(SignupCanvas);
        Login.Hide(loginCanvas);
        Login.Hide(LoginBtn);
        Login.Hide(SignupBtn);
    }

    public void LoadCharacterScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadPlayScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LogoutBtn()
    {
        PlayerPrefs.DeleteAll();
    }
}
