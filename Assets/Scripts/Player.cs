using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health = 0.5f;

    public float hunger = 0.8f;

    public float thirst = 0.6f;

    public float energy = 1f;

    public float stamina = 0;

    public float maxStamina = 0.5f;

    public float oxygen = 1f;

    public bool buildingBoat = false;
    public bool underwater = false;
    public bool godMode = true;
    public  Action onPlayerStateChanged;
    public Transform hand;
    public Transform selectedObject;
    public Inventory inventory;
    public static Player _Player;
    public GameObject blackscreen;
    bool hintSet = false;
    float lastEated = 5;
    float lastDrinked = 2;

   
    void Start()
    {
        _Player = this;
        inventory = GetComponent<Inventory>();
        
        //ADD Load from save file
        stamina = maxStamina;

        
       
    }

    // Update is called once per frame
    void Update()
    {

        //Food water change
        float time = GameManager._GameManager.time;
        if(Mathf.Abs(time-lastEated) < 0.2f)
        {
            lastEated = time + 5;
            lastEated %= 24;
            hunger -= Time.deltaTime;
            onPlayerStateChanged?.Invoke();

            if (hunger < 0)
                TakeDamage(0.15f);
        }
        if (Mathf.Abs(time - lastDrinked) < 0.2f)
        {
            lastDrinked = time + 2;
            lastDrinked %= 24;
            thirst -= Time.deltaTime;
            onPlayerStateChanged?.Invoke();
            if (thirst < 0)
                TakeDamage(0.2f);
        }

        

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit ,1f,1) && hit.collider !=null)
        {
            selectedObject = hit.transform;

            hintSet = true;
            if (selectedObject.tag =="Item" && (hand.childCount == 0 || hand.GetChild(0) != selectedObject.transform))
            {
                HintManager.setHint("Press E to pick item");
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    HintManager.setHint("");
                    Item item = selectedObject.GetComponent<Item>();
                    
                    inventory.AddItem(item);
                }

            }
            else if (selectedObject.GetComponent<Lootable>())
            {
                HintManager.setHint("Press E to loot");
                if (Input.GetKeyDown(KeyCode.E) && GameManager._GameManager.freeze == false)
                {
                    GameManager._GameManager.OnDelayEnd+=GameManager._GameManager.ShowLootUi;
                    GameManager._GameManager.currentLoot = selectedObject.GetComponent<Inventory>();
                    GameManager._GameManager.OnDelayEnd += selectedObject.GetComponent<Inventory>().ReDraw;
                    GameManager._GameManager.ShowDelayMenu(UnityEngine.Random.Range(1, 3.5f),"Looting...");
                }
                else if(Input.GetKeyDown(KeyCode.E))
                {
                    GameManager._GameManager.HideLootUi();
                }
            }
            else if(selectedObject.name.Contains("blanket"))
            {
                if(energy < 0.75f)
                {
                    HintManager.setHint("Press E to sleep");
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        
                        StartCoroutine(sleep((1 - energy) * 8));

                    }
                }
            }
        }
        else if(hintSet && GameManager._GameManager.freeze == false)
        {
            selectedObject = null;
            HintManager.setHint("");
            
        }
        
    }
    public bool isInHand(Transform item)
    {
        if (hand.childCount > 0)
            return hand.GetChild(0) == item;

        return false;
    }
    public void UseItemInHand()
    {
        hand.GetChild(0).GetComponent<Item>().Use();

    }
    public void AddItemToHand(Transform item,Vector3 posOffset = new Vector3(),Vector3 rotOffset = new Vector3())
    {
        item.GetComponent<Rigidbody>().useGravity = false;
        /*
        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        
        item.GetComponent<Rigidbody>().velocity = Vector3.zero;
        */
        item.gameObject.layer = 2;
        item.GetComponent<Rigidbody>().isKinematic = true;

        Debug.Log("Add item to hand");
        if(hand.childCount == 0)
        {
            item.transform.SetParent(hand);
            item.transform.localPosition = Vector3.zero;
            item.transform.localPosition += posOffset;
            item.localRotation = Quaternion.Euler(rotOffset);
        }
        else
        {
            Debug.Log("Switching item");
            GameObject temp = hand.GetChild(0).gameObject;
            temp.SetActive(false);
            RemoveItemFromHand(temp.transform,false);


            item.transform.SetParent(hand);
            item.transform.localPosition = Vector3.zero;
            item.transform.localPosition += posOffset;
            item.localRotation = Quaternion.Euler(rotOffset);
        }

    }
    public void RemoveItemFromHand(Transform item,bool drop)
    {
        item.GetComponent<Rigidbody>().useGravity = true;

        //item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.gameObject.layer = 0;
        item.parent = null;
        
        if(drop == false)
        {
            item.gameObject.SetActive(false);
        }
    }
    public IEnumerator sleep(float time)
    {
        
        Image screen =blackscreen.GetComponent<Image>();
        Color c = new Color(0, 0, 0, 0);
        screen.color = c;
        
        float tValue = Time.deltaTime;
            float add = tValue * 0.5f;
            Time.timeScale = 5;
            GameManager._GameManager.timeMultiplayer = 2f;
            GameManager._GameManager.freeze = true;
            float tSum = 0;

            while (tSum < 1)
            {
            tSum += Time.deltaTime;
            c.a = tSum;
            screen.color = c;
            }
             tSum = 0;
            GameManager._GameManager.ShowDelayMenu(time, "Sleeping");
            blackscreen.SetActive(true);
        while (tSum < time)
            {
                tValue = Time.deltaTime;
                energy += add;
                stamina += add;
                tSum += tValue;
                health = Mathf.Clamp01(health);
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
                Player._Player.onPlayerStateChanged?.Invoke();
                yield return new WaitForSeconds(tValue);
            }
        blackscreen.SetActive(false);
        c.a = 0;
        screen.color = c;
        GameManager._GameManager.timeMultiplayer = 0.05f;
        Time.timeScale = 1;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Water")
        {
            underwater = true;
            Debug.Log("InWater");
            if (UnityEngine.Random.Range(0, 3) == 1)
            {
                Debug.Log("Shark comming");
                SharkAi shark = GameObject.FindObjectOfType<SharkAi>();
                shark.player = transform;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Water")
        {
            underwater = false;
            Debug.Log("Exit Underwater");
            SharkAi shark = GameObject.FindObjectOfType<SharkAi>();
            shark.player = null;
            shark.target = shark.transform.position;
        }
    }
    void TakeDamage(float dmg)
    {
        if (godMode)
        {
            Debug.Log($"God mode saved from {dmg}");
            return;
        }
        float temp = health - dmg;
        if(temp <= 0)
        {
            Debug.Log("OnDeadEvent");
        }
        else
        {
            health = temp;
        }
    }

}
