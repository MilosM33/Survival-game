using System.Collections;
using System.Collections.Generic;
using TMPro;        
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    
    public int day = 0;
    public int maxDays = 12;
    [Range(0, 24)]
    public float time = 10;
    public float timeMultiplayer;
    public Gradient ambientColor;
    public Gradient fogColor = new Gradient();
    public Gradient lightColor;
    public Light sun;
    public Transform water;
    public AnimationCurve waterCurve;
    public float waterMultiplayer = 10f;
    public bool freeze = false;
    public bool delay = false;
    bool newDay = true;
    public static GameManager _GameManager;
    public GameObject lootableUi;
    public GameObject playerInventory;
    public GameObject delayMenuUI;
    public Image delayRadialBar;
    public TextMeshProUGUI delayMenuText;
    public delegate void delayEnd();
    public event delayEnd OnDelayEnd;
    public Inventory currentLoot;
    
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private List<RectTransform> uiElements;


    public Dictionary<string, float> stats = new Dictionary<string, float>()
    {
      {"Survived days",0 },
      {"Playtime", 0},
      {"Boat building",0 },
      {"Average energy",0 },
      {"Average hunger",0 },
      {"Average thirst",0 },
      {"Chopped trees",0 },
      {"Eaten food",0 },
      {"Picked items",0 },
      {"Containers looted",0 },
    };
    public void Awake()
    {
        _GameManager = this;
        GameObject.Find("Player").GetComponent<Player>().onPlayerStateChanged += UpdateUI;
    }

    private void UpdateUI()
    {
        if (uiElements.Count == 0)
            return;
        uiElements[0].localScale = new Vector3(1, Player._Player.health,1);
        uiElements[1].localScale = new Vector3(1, Player._Player.thirst, 1);
        uiElements[2].localScale = new Vector3(1, Player._Player.energy, 1);
        uiElements[3].localScale = new Vector3(1, Player._Player.hunger, 1);
        uiElements[4].localScale = new Vector3(1, Player._Player.stamina, 1);

    }
    private void OnValidate()
    {
        UpdateLighting(time);
    }

    public void ShowLootUi()
    {
        freeze = true;
        lootableUi.SetActive(true);
        playerInventory.SetActive(true);
        playerInventory.transform.SetParent(lootableUi.transform.GetChild(0).transform);
        playerInventory.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        GameManager._GameManager.stats["Containers looted"] += 1;

    }

    public void HideLootUi()
    {
        freeze = false;
        lootableUi.SetActive(false);
        playerInventory.SetActive(false);
        playerInventory.transform.SetParent(GameObject.Find("InventoryPlaceholder").transform);
        playerInventory.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

    }

    public void SpawnItems()
    {
        foreach (var item in GameObject.FindObjectsOfType<Lootable>())
        {
            if (item.spawn.Count == 0)
            {
                continue;

            }
            
            int count = Random.Range(item.minCount, item.maxCount);
            for (int y = 0; y < count; y++)
            {
                float rng = Random.Range(0.0f, 1f);
                float sum = 0;

                int index = 0;
                for (int i = 0; i < item.probability.Count; i++)
                {
                    sum += item.probability[i];
                    index = i;
                    if (sum > rng)
                        break;

                    
                }

                if (item.spawn.Count <= index)
                {
                    Debug.LogError("Problem with object " + item.name);
                    break;
                }

                GameObject temp = Instantiate(item.spawn[index]);
                IStackable stackable = temp.GetComponent<IStackable>();
                if(stackable != null)
                {
                    stackable.count = (byte)Random.Range(1, stackable.countMax);
                }
                //Prevent duplicity
                /*
                if (Random.Range(0, 3) == 1)
                {
                    Debug.Log("Removed" + item.name);
                    item.spawn.RemoveAt(index);
                    item.probability.RemoveAt(index);
                }
                */
                temp.SetActive(false);

                Item tempItem = temp.GetComponent<Item>();
                if (tempItem != null)
                    item.GetComponent<Inventory>().AddItem(temp.GetComponent<Item>());
                else
                    Debug.LogError("Problem with loot " + item.name);
                
            }

            //Saving little memory
            item.spawn = null;
            item.probability = null;
            
        }
    }


    private void Start()
    {
        SpawnItems();

    }
    // Update is called once per frame
    sbyte currentAmbient = 0b0;
    void Update()
    {

        float deltaTime = Time.deltaTime * timeMultiplayer;
       time += deltaTime;
        time %= 24;

        stats["Playtime"] += deltaTime;
        if (Player._Player.buildingBoat)
        {
            stats["Boat building"] += deltaTime;
        }
        //Playing ambient sounds

        if ((time < 8 || time > 20) && (currentAmbient & (1 << 0)) == 0)
        {
            currentAmbient = 0;
            currentAmbient |= 1 << 0;
            AudioManager._AudioManager.StopSoundSmooth("afternoon",1.5f);
            AudioManager._AudioManager.PlaySound("night");
        }
        else if(time > 8 && time < 14 &&(currentAmbient & (1 << 1)) == 0)
        {
            currentAmbient |= 1 << 1;
            AudioManager._AudioManager.StopSoundSmooth("night", 1.5f);
            AudioManager._AudioManager.PlaySound("morning");
        }
        else if (time > 14 && time < 20 && (currentAmbient & (1 << 2)) == 0)
        {
            currentAmbient = 0;
            currentAmbient |= 1<< 2;

            AudioManager._AudioManager.StopSoundSmooth("morning", 1.5f);
            AudioManager._AudioManager.PlaySound("afternoon");
        }

        

        if (Mathf.Round(time) == 0 && newDay)
        {
            day++;
            //Statistics
            stats["Survived days"] = day;
            stats["Average energy"] += Player._Player.energy;
            stats["Average hunger"] += Player._Player.hunger;
            stats["Average thirst"] += Player._Player.thirst;
            

            currentDayText.text = "Day: " + day;
            //Rise water
            Vector3 temp = water.transform.position;
            temp.y += waterCurve.Evaluate((float)day/maxDays)*waterMultiplayer;
            water.transform.position = temp;
            newDay = false;
        }
        
        if (time > 5)
            newDay = true;
        
        UpdateLighting(time);
        
    }

    void UpdateLighting(float t)
    {

        float temp = t / 24f;
        RenderSettings.ambientLight = ambientColor.Evaluate(temp);
        RenderSettings.fogColor = fogColor.Evaluate(temp);
        sun.color = lightColor.Evaluate(temp);
        Quaternion rot = sun.transform.rotation;
        sun.transform.rotation = Quaternion.Euler(240 + (360/24)*t, rot.y, rot.z);
        
    }


    public void ShowDelayMenu(float t,string text)
    {
        delayMenuText.text = text;
        StartCoroutine(delayMenu(t));
    }
    IEnumerator delayMenu(float t)
    {
        
        freeze = true;
        delay = true;
        delayMenuUI.SetActive(true);
        float timer = 0;
        delayRadialBar.fillAmount = 0;
        while (timer < t)
        {
            timer += Time.deltaTime;
            delayRadialBar.fillAmount = timer / t;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        delay = false;
        delayMenuUI.SetActive(false);
        freeze = false;

        if (OnDelayEnd != null)
            OnDelayEnd();
    }

    
}
