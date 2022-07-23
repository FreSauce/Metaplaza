using UnityEngine;

public class ShoppingItem : MonoBehaviour
{
    public string Name;
    
    public ShoppingMenu shoppingMenu;

    public void print()
    {
        Debug.Log(Name);
    }
}
