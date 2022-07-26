using TMPro;
using Photon.Voice.PUN;
using UnityEngine;
using Photon.Pun;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
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

    private string addToCartEndpoint = "https://ancient-retreat-18243.herokuapp.com/api/cart/addToCart/";
    private string fetchCartEndpoint = "https://ancient-retreat-18243.herokuapp.com/api/cart/getCart";

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
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if (door.isOpen)
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
                addToCart(shoppingItem.id);
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
                fetchCart();
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
                        InfoText.text = "Close \"E\"";

                    }
                    else
                    {
                        InfoText.text = "Open \"E\"";
                    }
                    // UseText.transform.position = hit.point - (hit.point - Camera.position).normalized * 0.1f;
                    // UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);
                }
                else if (hit.collider.TryGetComponent<ShoppingItem>(out ShoppingItem shoppingItem))
                {
                    InfoText.text = "Select \"B\" to buy " + shoppingItem.name;
                }

            }
            else
            {
                InfoText.text = "";
            }
        }
    }


    // ADD TO CART
    public async void addToCart(string _id)
    {
        if (_id != "")
        {
            try
            { 
                UnityWebRequest request = UnityWebRequest.Get(addToCartEndpoint + _id);
                request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
                Debug.Log(request.ToString());
                var handler = request.SendWebRequest();

                while (!handler.isDone)
                {
                    await Task.Yield();
                }

                Debug.Log(request.result);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                }
                else
                { 
                    Debug.Log("SUCCESSFULLY ADDED TO CART");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    // FETCH CART
    public async void fetchCart()
    {
        try
        {
            UnityWebRequest request = UnityWebRequest.Get(fetchCartEndpoint);
            request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
            var handler = request.SendWebRequest();

            while (!handler.isDone)
            {
                await Task.Yield();
            }

            Debug.Log(request.result);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}


