using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<ItemUI>().isSet)
        {
            if (transform.childCount > 0)
            {
                Transform temp = transform.GetChild(0);

                if (transform.parent != eventData.pointerDrag.transform.parent)
                {
                    Debug.Log("Cross inventory");
                    if (transform.parent.name == "lootPanel")
                    {
                        GameManager._GameManager.currentLoot.CrossSwap(int.Parse(temp.parent.name), int.Parse(eventData.pointerDrag.GetComponent<ItemUI>().parrent.name), Player._Player.inventory);
                    }
                    else
                    {
                        Player._Player.inventory.CrossSwap(int.Parse(temp.parent.name), int.Parse(eventData.pointerDrag.GetComponent<ItemUI>().parrent.name), GameManager._GameManager.currentLoot.GetComponent<Inventory>());
                    }

                }
                else
                {
                    Debug.Log("Same inventory");
                    if (transform.parent.name == "lootPanel")
                    {
                        GameManager._GameManager.currentLoot.SwapItem(int.Parse(temp.parent.name), int.Parse(eventData.pointerDrag.GetComponent<ItemUI>().parrent.name));
                    }
                    else
                    {
                        Player._Player.inventory.SwapItem(int.Parse(temp.parent.name), int.Parse(eventData.pointerDrag.GetComponent<ItemUI>().parrent.name));
                    }

                }
                // GameObject.FindObjectOfType<Inventory>().SwapItem(int.Parse(temp.parent.name),int.Parse(eventData.pointerDrag.GetComponent<ItemUI>().parrent.name));



                Debug.Log("Swap");
                temp.SetParent(eventData.pointerDrag.GetComponent<ItemUI>().parrent);
                temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            }
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<ItemUI>().parrent = transform;
            Debug.Log("End drop");


        }
    }
}
