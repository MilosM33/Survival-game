using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler,IPointerClickHandler
{
    public bool isSet = false;
    public Transform parrent;
    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        parrent = transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isSet = !(GetComponent<Image>().sprite == null);
        //Maybe replace
        parrent = transform.parent;
        transform.parent = transform.parent.parent;
        Debug.Log("Moved");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSet)
        {
            transform.position = eventData.position;
            Debug.Log("Moving");
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Debug.Log("EndDrag");

        //If not drop to any slot
        if(transform.parent == parrent.parent)
        {
            transform.parent = parrent;
            
        }
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSet = !(GetComponent<Image>().sprite == null);
        if (isSet && eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Show menu");
            //Border Panel Inventory
            parrent.parent.parent.GetComponent<InventoryUI>().ShowActionMenu(transform.position,int.Parse(transform.parent.name));

        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            
       
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(transform.parent.parent.name == "lootPanel")
            {
                int index = Player._Player.inventory.GetFirstEmptySpace();
                int index2 = int.Parse(transform.parent.name);
                Player._Player.inventory.items[index] = GameManager._GameManager.currentLoot.items[index2];
                GameManager._GameManager.currentLoot.items[index2] = null;
                Player._Player.inventory.ReDraw();
                GameManager._GameManager.currentLoot.ReDraw();
                Debug.Log("clicked");
            }
            else
            {
            int index = GameManager._GameManager.currentLoot.GetFirstEmptySpace();
            int index2 = int.Parse(transform.parent.name);
            GameManager._GameManager.currentLoot.items[index] = Player._Player.inventory.items[index2];
             GameManager._GameManager.currentLoot.ReDraw();
             Player._Player.inventory.items[index2] = null;
            Player._Player.inventory.ReDraw();
           }
            
        }
            
        }
    }
}
