using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public PhotonView playerPrefab;
    public Vector3 spawnPoint;

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
            //keep instance even after changing scenes.
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void changeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }


    //Callback functions:
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        PhotonNetwork.JoinOrCreateRoom("urmom", null, null);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully");
        PhotonNetwork.NickName = PlayerPrefs.GetString("userId") + "||" + PlayerPrefs.GetString("username");
        
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity);
    }
}
