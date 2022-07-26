using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CartMenu : MonoBehaviour
{
    public static bool MenuOpen = false;

    public GameObject cartMenu;
    [SerializeField]
    public GameObject cartItemPrefab;
    public List<GameObject> cartItems;

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


    public void OpenMenu()
    {
        cartMenu.SetActive(true);
        MenuOpen = true;
        SetCursorState(false);
    }

    public void CloseMenu()
    {
        cartMenu.SetActive(false);
        MenuOpen = false;
        SetCursorState(true);
    }

    public void setItems(ICartItem[] items)
    {
        foreach(ICartItem item in items)
        {
            Debug.Log(item.ToString());
            GameObject temp = Instantiate(cartItemPrefab);
            temp.transform.SetParent(cartMenu.transform);
            temp.transform.localScale = Vector3.one;
            CartItem ct = temp.GetComponent<CartItem>();
            ct.name.text = item.name;
            ct.price.text = item.price;
            ct.quantity.text = item.quantity.ToString();
            cartItems.Add(temp);
        }

    }
}

[System.Serializable]
public class CartResponse
{
    public string status;
    public ICartItem[] data;
}

[System.Serializable]
public class ICartItem
{
    public string _id;
    public string name;
    public string link;
    public string img;
    public string price;
    public int quantity;
    public int __v;
}