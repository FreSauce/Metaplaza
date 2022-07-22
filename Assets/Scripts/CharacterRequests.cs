using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharacterRequests : MonoBehaviour
{
    [SerializeField] private static string saveEndpoint= "https://ancient-retreat-18243.herokuapp.com/api/users/sendCharacterData";
    [SerializeField] private static string loadEndpoint = "https://ancient-retreat-18243.herokuapp.com/api/users/getUserData/";
    AdvancedPeopleSystem.CharacterCustomization character;

    private void Start()
    {
        character = GetComponent<AdvancedPeopleSystem.CharacterCustomization>();
        //Debug.Log(character);
        StartCoroutine(GetCharacter(PlayerPrefs.GetString("userId"), (response) =>
        {
            if(response != null)
            {
            //Debug.Log(response.characterType);
            //Debug.Log(PlayerPrefs.GetString("token"));
            if (response.characterType == "Male")
            {
                character.SwitchCharacterSettings(0);
            }
            else if(response.characterType == "Female")
            {
                character.SwitchCharacterSettings(1);
            }
            character.ApplyCharacterData(response.characterData);
            }
        }));
    }

    private void Update()
    {
        if(!PlayerPrefs.HasKey("token"))
        {
            Debug.Log("Token: " + PlayerPrefs.GetString("token"));
            SceneManager.LoadScene(0);
        }
    }

    public void InitializeCharacter(string userId)
    {
        character = GetComponent<AdvancedPeopleSystem.CharacterCustomization>();
        //Debug.Log(character);
        StartCoroutine(GetCharacter(userId, (response) =>
        {
            //Debug.Log(response.characterType);
            //Debug.Log(PlayerPrefs.GetString("token"));
            if (response.characterType == "Male")
            {
                character.SwitchCharacterSettings(0);
            }
            else if (response.characterType == "Female")
            {
                character.SwitchCharacterSettings(1);
            }
            character.ApplyCharacterData(response.characterData);
        }));
    }

    public static IEnumerator SaveCharacter(string data, System.Action<PostResponse> Callback)
    {
        if (data.Length <= 0) { yield break; }
        //Debug.Log(data);
        WWWForm form = new WWWForm();
        form.AddField("character_data", data);
        UnityWebRequest request = UnityWebRequest.Post(saveEndpoint, form);
        request.SetRequestHeader("authorization", "Bearer " + PlayerPrefs.GetString("token"));
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
        //Debug.Log(request.result.ToString());

        if (request.result == UnityWebRequest.Result.Success)
        {
            PostResponse response = JsonUtility.FromJson<PostResponse>(request.downloadHandler.text);
            Callback(response);
        }
        else
        {
            Debug.Log("Error connecting to the server...");
            PlayerPrefs.DeleteKey("token");
            Callback(null);
        }
        request.Dispose();
        yield return null;
    }

    public static IEnumerator GetCharacter(string userId, System.Action<GetResponse> Callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(loadEndpoint + userId);
        request.SetRequestHeader("authorization", "Bearer " + PlayerPrefs.GetString("token"));
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

        //Debug.Log(request.result.ToString());

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(request.downloadHandler.text);
            GetResponse response = JsonUtility.FromJson<GetResponse>(request.downloadHandler.text);
            Callback(response);
        }
        else
        {
            Debug.Log("Error connecting to the server: " + request.result);
            PlayerPrefs.DeleteKey("token");
            Callback(null);
        }
        request.Dispose();
        yield return null;
    }


    [System.Serializable]
    public class PostResponse
    {
        public int code;
        public string msg;
        public string data;
    }

    public class GetResponse
    {
        public string status;
        public string characterData;
        public string characterType;
        public string name;
    }


}
