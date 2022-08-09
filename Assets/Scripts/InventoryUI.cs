using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject menu;
    public GameObject button;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            HideActionMenu();
        }
    }

    public void HideActionMenu()
    {
       if(menu != null)
        {
            Debug.Log("Menu hidden");
            menu.SetActive(false);
        }

    }

    private void OnDisable()
    {
        HideActionMenu();
    }
    public void ShowActionMenu(Vector3 pos,int index)
    {
        if (menu == null)
            return;

        Inventory temp = GameObject.Find("Player").GetComponent<Inventory>();
        Item item = temp.items[index];
        
        menu.SetActive(true);
        ActionMenu actionMenu = menu.GetComponent<ActionMenu>();
        actionMenu.inventory = temp;
        actionMenu.index = index;
        menu.transform.position = pos;
        
        menu.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Name;
        IEquipable equipable = item.GetComponent<IEquipable>();
        Transform t = menu.transform.GetChild(1);
        if (equipable != null)
        {
            
            if(Player._Player.isInHand(item.transform))
            {
                t.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";

            }
            else
            {
                t.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            }
        }
        else
        {
            t.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
        }

    }

}