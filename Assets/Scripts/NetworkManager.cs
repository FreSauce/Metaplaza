using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public PhotonView playerPrefab;
    public Transform spawnPoint;

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
            // DontDestroyOnLoad(gameObject);
        }
    }

    public void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("Leave Room");
            PhotonNetwork.Disconnect();
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

    public void LeaveRoom()
    {
        Debug.Log("Function is called");
        PhotonNetwork.LeaveRoom();
    }

    //Callback functions:
    public override void OnConnectedToMaster()
    {
      //  if (PhotonNetwork.InRoom)
      //  {
      //      Debug.Log("Leave Room");
      //      PhotonNetwork.LeaveRoom();
      //  }

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
        
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
    }
}
