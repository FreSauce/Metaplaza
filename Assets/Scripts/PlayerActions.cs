using TMPro;
using Photon.Voice.PUN;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

public class PlayerActions : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshPro UseText;
    [SerializeField]
    private TextMeshProUGUI InfoText;
    [SerializeField]
    private GameObject InfoTextHolder;
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float MaxUseDistance = 5f;
    [SerializeField]
    private LayerMask UseLayers;

    private PhotonVoiceNetwork _voiceNetwork;

    public Canvas menuCanvas;

    public ShoppingMenu shoppingMenu;

    private void Awake()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        _voiceNetwork = PhotonVoiceNetwork.Instance;
        _voiceNetwork.PrimaryRecorder.TransmitEnabled = false;

        menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();

        shoppingMenu = menuCanvas.GetComponent<ShoppingMenu>();

        InfoText = menuCanvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnUse()
    {
        if(Physics.Raycast(Camera.position,Camera.forward,out RaycastHit hit,MaxUseDistance,UseLayers))
        {
            if(hit.collider.TryGetComponent<Door>(out Door door))
            {
                if(door.isOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open(transform.position);
                }
            }
        }
    }

    public void OnBuy()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<ShoppingItem>(out ShoppingItem shoppingItem))
            {
                Debug.Log(PauseMenu.GameIsPaused);
                if (!PauseMenu.GameIsPaused)
                {
                    if (!ShoppingMenu.MenuOpen)
                    {
                        shoppingMenu.OpenMenu();
                    }
                    else
                    {
                        shoppingMenu.CloseMenu();
                    }
                }
            }
        }
    }

    public void OnVoiceToggle()
    {
        _voiceNetwork.PrimaryRecorder.TransmitEnabled = !_voiceNetwork.PrimaryRecorder.TransmitEnabled;
        Debug.Log(_voiceNetwork.PrimaryRecorder.TransmitEnabled);
    }


    public void Update()
    {
        if (photonView.IsMine)
        {
            if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
            {

                if (hit.collider.TryGetComponent<Door>(out Door door))
                {

                    if (door.isOpen)
                    {
                        InfoText.text="Close \"E\"";
                        
                    }
                    else
                    {
                        InfoText.text="Open \"E\"";
                    }
                    // UseText.transform.position = hit.point - (hit.point - Camera.position).normalized * 0.1f;
                    // UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);
                }
                else if (hit.collider.TryGetComponent<ShoppingItem>(out ShoppingItem shoppingItem))
                {
                    InfoText.text="Select \"B\" to buy";
                
                    // UseText.transform.position = hit.point - (hit.point - Camera.position).normalized * 0.2f;
                    // UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);

                    // UseText.transform.rotation = Quaternion.LookRotation(hit.transform.position - Camera.transform.position);
                }

            }
            else
            {
                InfoText.text = "";
            }
        }
    }
}


