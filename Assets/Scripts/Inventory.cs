using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public Item[] items;

    public int slots = 12;
    public int index = 0;
    public GameObject template;
    public GameObject countText;
    public List<Transform> placeHolder;

    public void Start()
    {
        if(items == null || items.Length == 0)
        {
            items = new Item[slots];
        }
        if (transform.name == "Player")
        {
            ShowInventory();
            ReDraw();
        }
            

    }
    //For lootable ui
    public void ReDraw()
    {
        
        Transform temp = null;
        if (transform.name == "Player")
             temp = GameObject.Find("Panel").transform;
        else
        {
            temp = GameObject.Find("lootPanel").transform;
            TextMeshProUGUI lootText = GameObject.Find("currentLootText").GetComponent<TextMeshProUGUI>();
            lootText.text = GetComponent<Lootable>().Name;
        }
            

        for (int i = 0; i < 12; i++)
        {
            //Check and then clear
            Transform t = temp.GetChild(i).GetChild(0);
           if(t.GetComponent<Image>().sprite != null)
            {
                t.GetComponent<Image>().sprite = null;
                t.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                if(t.childCount != 0)
                {
                    DestroyImmediate(t.GetChild(0).gameObject);
                }
            }
            if (items[i] != null)
            {
               AddItemUi(items[i],t,items[i].GetComponent<IStackable>());
            }
            
        }
    }

    public void AddItem(Item item)
    {
        if(GetItemCount() < slots) {
        item.PickUp();
        if(gameObject.name =="Player")
          GameManager._GameManager.stats["Picked items"] += 1;
        IStackable temp = item.GetComponent<IStackable>();
        int slot = -1;
        bool found = false;
        for (int i = 0; i < items.Length; i++)
        {
            //Get first emptyslot
            if(slot == -1 && items[i] == null)
            {
                slot = i;
                index = slot;
                if (temp == null)
                    break;
            }
                //
                
            if (temp != null && items[i] !=null)
            {
                IStackable tempItem = items[i].GetComponent<IStackable>();
                if (items[i].Name == item.Name && tempItem.count + temp.count <= tempItem.countMax)
                {
                    found = true;
                    byte t = (byte)(tempItem.count + temp.count);
                    tempItem.count = t;
                    Destroy(item.gameObject);

                    if (placeHolder.Count != 0)
                    {
                      AddItemUi(item, placeHolder[index].GetChild(0), tempItem);
                    }

                    break;
                }
            }
        }
        if(found == false)
        {
         items[index] = item;
         if(placeHolder.Count !=0)
           AddItemUi(item, placeHolder[index].GetChild(0), temp);
        }
        }
        else
        {
            HintManager.setHint("Inventory full", 1);
        }
    }
    void AddItemUi(Item item,Transform frame, IStackable temp)
    {

        frame.GetComponent<Image>().sprite = item.img;
        frame.GetComponent<Image>().color = Color.white;

        //Count ui text
        if (temp != null && frame.childCount == 0)
        {
            GameObject text = Instantiate(countText);
            text.transform.SetParent(frame.transform);

            text.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            text.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -25, 0);
            text.GetComponent<TextMeshProUGUI>().text = temp.count.ToString();
        }
        else if(temp != null && frame.childCount != 0)
        {
            
            frame.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp.count.ToString();
        }
    }
    public IEnumerator DropCouroutine(int pos,IStackable stackable)
    {
        for (int i = 0; i < stackable.count; i++)
        {
            Item t = Instantiate(items[pos], transform.position + transform.forward * Random.Range(0.15f, 1)+Vector3.up*Random.Range(0.15f,1), Quaternion.identity);
            t.Drop(false);
            
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(items[pos].gameObject);
        items[pos] = null;
        

    }
    public void RemoveItem(int pos,bool spawn)
    {
        if (items[pos] != null)
        {
            if (index > pos)
                index = pos;
            IEquipable equipable = items[pos].GetComponent<IEquipable>();
            if (equipable != null)
            {
                equipable.Unequip();
                items[pos].Drop(true);
                
            }
            else if (spawn)
            {
                IStackable stackable = items[pos].GetComponent<IStackable>();
                
                if (stackable !=null)
                {
                    StartCoroutine(DropCouroutine(pos,stackable));
                    
                }
                else
                {
                    items[pos].Drop(true);
                    items[pos] = null;
                }
                    
                
            }
                
            else
                Destroy(items[pos].gameObject);


            placeHolder[pos].GetChild(0).GetComponent<Image>().sprite = null;
            placeHolder[pos].GetChild(0).GetComponent<Image>().color = new Color(0,0,0,0);
            if (placeHolder[pos].GetChild(0).childCount !=0)
            {
                Destroy(placeHolder[pos].GetChild(0).GetChild(0).gameObject);
            }
           
        }
        
    }

    public void UseItem(int pos)
    {
        Item item = items[pos];
        IEquipable equipable = item.GetComponent<IEquipable>();
        //weapon or something in hand
        if(equipable != null)
        {
            if (Player._Player.isInHand(item.transform))
                equipable.Unequip();
            else
                equipable.Equip();
        }
        //Item usable only in inventory
        else
        {

            if (!item.Use())
            {
                return;
            }
            if (item.transform.name.Contains("-.-"))
            {
                return;
            }
            IStackable temp = item.GetComponent<IStackable>();
            if (temp != null && temp.count - 1 > 0)
            {
                temp.count -= 1;
                placeHolder[pos].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = temp.count.ToString();
            }
            else
            {
                RemoveItem(pos, false);
            }

        }
        
    }
    public void CrossSwap(int index0, int index1,Inventory inv)
    {
        object temp = null;
        if (items[index0] != null)
            temp = items[index0].Clone();
        
        items[index0] = inv.items[index1];
        inv.items[index1] = (Item)temp;
    }
    public void SwapItem(int index0,int index1)
    {
        object temp = null;
        if (items[index0] != null)
            temp = items[index0].Clone();
        else  if(index >index1)
        {
            index = index1;
        }
        items[index0] = items[index1];
        items[index1] = (Item)temp;


    }

    public int GetItemCount()
    {
        int temp = 0;
        foreach (var item in items)
        {
            if (item != null)
                temp++;
        }
        return temp;
    }
    public bool isEmpty()
    {
        foreach (var item in items)
        {
            if(item != null)
            {
                return false;
            }
        }
        return true;
    }
    public int GetFirstEmptySpace()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                return i;
            }
        }
        return 99;
    }
    public void ShowInventory()
    {
        if (GameManager._GameManager.freeze)
        {
            template.SetActive(false);
            GameManager._GameManager.HideLootUi();
        }
        else
        {
            GameManager._GameManager.freeze = true;
            template.SetActive(true);
           
        }

    }


}
