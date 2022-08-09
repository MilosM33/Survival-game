using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : Item,IEquipable
{
    public override string Name => "Water bottle";

    [SerializeField] private Sprite Image;
    public override Sprite img => Image;

    public Vector3 posOffset => new Vector3(0,0.7f,0);

    public Vector3 rotOffset => new Vector3(-120,0,0);

    float waterLevel = 1;

    public void Equip()
    {
        gameObject.SetActive(true);
        Player._Player.AddItemToHand(transform,posOffset,rotOffset);
        if(waterLevel > 0)
        {
            AudioManager._AudioManager.PlaySound("waterEquip");
        }
    }

    public void Unequip()
    {
        gameObject.SetActive(false);
        Player._Player.RemoveItemFromHand(transform,false);
    }
    IEnumerator drinkAnim(float temp)
    {
        AudioManager._AudioManager.PlaySound("waterDrink");
        GameManager._GameManager.ShowDelayMenu(4,"Drinking");
        yield return new WaitForSeconds(5);
        temp = Random.Range(0.1f, temp);
        waterLevel -= temp;
        Player._Player.thirst = Mathf.Clamp01(Player._Player.thirst + temp);
        Player._Player.onPlayerStateChanged?.Invoke();
    }
    public override bool Use()
    {
        
        if (waterLevel > 0)
        {
            
            float temp = 1 - Player._Player.thirst;
            if (temp > 0)
            {

                StartCoroutine(drinkAnim(temp));
            }
            else
                HintManager.setHint("Not thirsty",1);
        }
        else
        {
            if (Player._Player.underwater && waterLevel < 1)
            {
                waterLevel = 1;
                HintManager.setHint("Bottle refilled", 1);
            }

            else
               HintManager.setHint("Bottle is empty", 1);
        }
        return true;
    }

}
