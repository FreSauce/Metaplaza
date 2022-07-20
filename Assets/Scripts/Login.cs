using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    //private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,24})";
    public GameObject loginCanvas;
    public GameObject SignupCanvas;
    [SerializeField] private string loginEndpoint = "https://ancient-retreat-18243.herokuapp.com/api/users/login";
    [SerializeField] private string signupEndpoint = "https://ancient-retreat-18243.herokuapp.com/api/users/signup";

    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button createButton;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField confirmPasswordInputField;
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_InputField signupEmailInputField;
    [SerializeField] private TMP_InputField signupPasswordInputField;
    public void Hide(GameObject obj)
    {
       obj.SetActive(false);
    }
    public void Show(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void Start()
    {
        Show(loginCanvas);
        Hide(SignupCanvas);
    }

    private void Update()
    {
        if (PlayerPrefs.GetString("token", "") != "")
        {
            SceneManager.LoadScene(1);
        }
    }

    public void OnLoginClick()
    {
        string username = emailInputField.text;
        string password = passwordInputField.text;
        Hide(SignupCanvas);
        Show(loginCanvas);
        Debug.Log(password);
        if (username.Length > 0 && password.Length > 0)
        {
            alertText.text = "Signing in...";
            StartCoroutine(TryLogin(username, password));
        }
    }



    public void OnCreateClick()
    {
        string email = signupEmailInputField.text;
        string password = signupPasswordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;
        string username = userNameInputField.text;
        Hide(loginCanvas);
        Show(SignupCanvas);
        if (email.Length > 0 && password.Length > 0 && confirmPassword.Length > 0 && username.Length > 0)
        {
            alertText.text = "Creating account...";
            StartCoroutine(TryCreate(email, password, confirmPassword, username));
        }
    }

    private IEnumerator TryLogin(string username,string password)
    {


        WWWForm form = new WWWForm();
        form.AddField("email", username);
        form.AddField("password", password);
        if (username.Length <= 0 && password.Length <= 0) { yield break; }
        UnityWebRequest request = UnityWebRequest.Post(loginEndpoint, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Debug.Log(request.downloadHandler.text);
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
            Debug.Log(response.user._id);
            PlayerPrefs.SetString("token", response.token);
            PlayerPrefs.SetString("userId", response.user._id);
        }
        else
        {
            alertText.text = "Error connecting to the server...";
            PlayerPrefs.SetString("token", null);
            ActivateButtons(true);
        }


        yield return null;
    }

    private IEnumerator TryCreate(string email,string password, string username,string confirmPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("name", username);
        form.AddField("confirmPassword", confirmPassword);
        UnityWebRequest request = UnityWebRequest.Post(signupEndpoint, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }
        Debug.Log(request.result);
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(request.downloadHandler.text);
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
            Debug.Log(response.token);
            PlayerPrefs.SetString("token", response.token);
        }
        else
        {
            alertText.text = "Error connecting to the server...";
        }

        ActivateButtons(true);

        yield return null;
    }

    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
        createButton.interactable = toggle;
    }
}
