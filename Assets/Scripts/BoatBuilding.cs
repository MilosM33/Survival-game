using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Recipe
{
    public string name;
    public Sprite img;
    public int count = 0;
    public int requiredCount = 0;
}

public class BoatBuilding : MonoBehaviour
{
    public GameObject BoatUi;

    public List<BoxCollider> placeholders;
    public Transform cells;

    public List<Recipe> items;

    public GameObject boat;
    public Transform boatSpawn;

    private void Start()
    {
        if(cells.childCount != items.Count)
        {
            Debug.LogError("More items than cells");
            Debug.Break();
        }

        //Init
        for (int i = 0; i < items.Count; i++)
        {
            Recipe item = items[i];
            Transform temp = cells.GetChild(i);
            temp.GetChild(0).GetComponent<Image>().sprite = item.img;
            temp.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{item.count}/{item.requiredCount}";
        }
    }
    public bool AddItem(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Recipe recipe = items[i];
            if (recipe.name == name && recipe.count+1 <= recipe.requiredCount)
            {
                recipe.count++;
                cells.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{recipe.count}/{recipe.requiredCount}";
                return true;
            }
        }
        return false;
    }
    public bool Done()
    {
        foreach (var item in items)
        {
            if (item.count != item.requiredCount)
                return false;
        }
        return true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            if (AddItem(other.GetComponent<Item>().Name))
            {
                other.gameObject.SetActive(false);
            }

            if (Done())
            {
                Instantiate(boat, boatSpawn.position, Quaternion.Euler(new Vector3(-90,0,0)));
            }
        }
        else if(other.name == "Player" && Player._Player.buildingBoat)
        {
            BoatUi.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name =="Player")
        {
            if (!Player._Player.buildingBoat)
            {
                
                HintManager.setHint("I should find something about boats.");
                HintManager.canSet = false;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            if (!Player._Player.buildingBoat)
            {
                HintManager.canSet = true;
                HintManager.setHint("");
            }
            BoatUi.SetActive(false);
        }
    }

}
