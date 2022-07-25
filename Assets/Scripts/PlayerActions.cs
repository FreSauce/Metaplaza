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
    private float MaxUseDistance = 1f;
    [SerializeField]
    private LayerMask UseLayers;

    private PhotonVoiceNetwork _voiceNetwork;

    public Canvas menuCanvas;

    public CartMenu cartMenu;

    private void Awake()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        _voiceNetwork = PhotonVoiceNetwork.Instance;
        _voiceNetwork.PrimaryRecorder.TransmitEnabled = false;

        menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();

        cartMenu = menuCanvas.GetComponent<CartMenu>();

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
                // CODE TO BUY THE PRODUCT
            }
        }
    }

    public void OnVoiceToggle()
    {
        _voiceNetwork.PrimaryRecorder.TransmitEnabled = !_voiceNetwork.PrimaryRecorder.TransmitEnabled;
        Debug.Log(_voiceNetwork.PrimaryRecorder.TransmitEnabled);
    }

    public void OnCart()
    {
        // CODE TO DISPLAY THE CART
        
        Debug.Log(PauseMenu.GameIsPaused);
        if (!PauseMenu.GameIsPaused)
        {
            if (!CartMenu.MenuOpen)
            {
                cartMenu.OpenMenu();
            }
            else
            {
                cartMenu.CloseMenu();
            }
        }
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
                    InfoText.text = "Select \"B\" to buy "+shoppingItem.name;
                }

            }
            else
            {
                InfoText.text = "";
            }
        }
    }
}


