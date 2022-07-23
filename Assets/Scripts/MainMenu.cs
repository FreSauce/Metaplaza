using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI LoginBtn;
    [SerializeField] public TextMeshProUGUI Signup;
    [SerializeField] public TextMeshProUGUI PlayButton;
    [SerializeField] public TextMeshProUGUI CharGen;
    [SerializeField] public TextMeshProUGUI Logout;
    public GameObject loginCanvas;
    public GameObject SignupCanvas;

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
        if (PlayerPrefs.HasKey("token"))
        {
            Hide(LoginBtn);
            Hide(Signup);
            Show(PlayButton);
            Show(CharGen);
            Show(Logout);
        }
        else
        {
            Hide(Logout);
            Show(LoginBtn);
            Show(Signup);
            Hide(CharGen);
            Hide(PlayButton);
        }
    }

    public void LoadLoginScene()
    {
        Login.Hide(SignupCanvas);
        Login.Show(loginCanvas);
        Hide(LoginBtn);
        Hide(Signup);
    }

    public void LoadSignupScene()
    {
        Login.Show(SignupCanvas);
        Login.Hide(loginCanvas);
        Hide(LoginBtn);
        Hide(Signup);
    }

    public void LoadCharacterScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadPlayScene()
    {
        SceneManager.LoadScene(3);
    }
    public void LogoutBtn()
    {
        PlayerPrefs.DeleteAll();
    }
}
