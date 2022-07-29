using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public bool isConnectedToMaster = false;
    public bool hasJoinedRoom = false;
    public static Vector3 spawnPoint = new Vector3(0, 2, 0);

    private void Awake()
    {
        //prevent network manager duplication
        if(instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            // keep instance even after changing scenes.
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Start()
    {
        handleSceneChange();
    }

    public void handleSceneChange()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.GetActiveScene().buildIndex == 0 && hasJoinedRoom)
        {
            Debug.Log("Leave Room");
            PhotonNetwork.LeaveRoom();
            hasJoinedRoom = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2 && !hasJoinedRoom)
        {
            Debug.Log("joining room");
            PhotonNetwork.ConnectUsingSettings();
            hasJoinedRoom = true;
        }
    }

    public void LeaveRoom()
    {
        Debug.Log("Function is called");
        PhotonNetwork.LeaveRoom();
    }

    //Callback functions:
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        PhotonNetwork.JoinOrCreateRoom("urmom", null, null);
        isConnectedToMaster = true;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully");
        PhotonNetwork.NickName = PlayerPrefs.GetString("userId") + "||" + PlayerPrefs.GetString("username");
        PhotonNetwork.Instantiate("VRPlayer", spawnPoint, Quaternion.identity);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Disconnect();
    }
}
