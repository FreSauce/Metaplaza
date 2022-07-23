using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShoppingMenu : MonoBehaviour
{
    public static bool MenuOpen = false;

    public GameObject shoppingMenu;

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


    public void OpenMenu(string name)
    {
        shoppingMenu.SetActive(true);
        MenuOpen = true;
        SetCursorState(false);
    }

    public void CloseMenu()
    {
        shoppingMenu.SetActive(false);
        MenuOpen = false;
        SetCursorState(true);
    }
}
