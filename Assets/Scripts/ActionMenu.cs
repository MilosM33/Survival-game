using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour
{
    public Inventory inventory;
    public int index;
    void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Use()
    {
        inventory.UseItem(index);
        Hide();
    }
    
    public void Drop()
    {
        inventory.RemoveItem(index, true);
        Hide();
    }

   




}
