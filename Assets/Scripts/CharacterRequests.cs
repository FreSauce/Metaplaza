using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterRequests : MonoBehaviour
{
    [SerializeField] private static string saveEndpoint= "https://ancient-retreat-18243.herokuapp.com/api/users/sendCharacterData";
    [SerializeField] private static string loadEndpoint = "https://ancient-retreat-18243.herokuapp.com/api/users/login";
    public static IEnumerator SaveCharacter(string data)
    {

        if (data.Length <= 0) { yield break; }

        WWWForm form = new WWWForm();

        form.AddField("jwt", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjYyZDVhMDcwNjEyMTdlZTMwYTJkNzM1ZSIsImlhdCI6MTY1ODE2NzQwOCwiZXhwIjoxNjY1OTQzNDA4fQ.PPhtoSRRxqnVHvhPUykMMuxjLAxDIWkhJ10GF8jgIvk");
        form.AddField("data", data);

        UnityWebRequest request = UnityWebRequest.Post(saveEndpoint, form);

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

        Debug.Log(request.result.ToString());

        if (request.result == UnityWebRequest.Result.Success)
        {
            SaveResponse response = JsonUtility.FromJson<SaveResponse>(request.downloadHandler.text);
            Debug.Log(response);
            if (response.code == 0) // save character success
            {
                Debug.Log("Save success");
            }
            else
            {
                switch (response.code)
                {
                    case 1:
                        
                        break;
                    default:
                        
                        break;
                }
            }
        }
        else
        {
            Debug.Log("Error connecting to the server...");
        }

        yield return null;
    }

    public static IEnumerator GetCharacter()
    {
        WWWForm form = new WWWForm();

        form.AddField("jwt", PlayerPrefs.GetString("jwt"));

        UnityWebRequest request = UnityWebRequest.Post(saveEndpoint, form);

        var handler = request.SendWebRequest();
        Debug.Log(request.url);

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
            GetResponse response = JsonUtility.FromJson<GetResponse>(request.downloadHandler.text);
            Debug.Log(response);
            if (response.code == 0) // get character success
            {
                Debug.Log("Save success");
                yield return response.data;
            }
            else
            {
                switch (response.code)
                {
                    case 1:

                        break;
                    default:

                        break;
                }
            }
        }
        else
        {
            Debug.Log("Error connecting to the server...");
        }

        yield return null;
    }


    [System.Serializable]
    public class SaveResponse
    {
        public int code;
        public string msg;
        public string data;
    }

    public class GetResponse
    {
        public int code;
        public string msg;
        public string data;
    }

}
