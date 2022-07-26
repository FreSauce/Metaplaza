using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartMenu : MonoBehaviour
{
    public static bool MenuOpen = false;

    public GameObject cartMenu;

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
}
