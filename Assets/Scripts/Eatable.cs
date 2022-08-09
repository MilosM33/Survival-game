using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eatable : Item,IStackable
{
    public string name;
    public Sprite image;
    public float hungerAmount = 0;
    public float thirstAmount = 0;
    private byte numberOfItem = 1;
    public byte maxCount = 4;
    public override string Name => name;

    public override Sprite img => image;

    public byte count { get => numberOfItem; set => numberOfItem = value; }

    public byte countMax => maxCount;

    public override bool Use()
    {
        if(Player._Player.hunger >=1)
        {
            HintManager.setHint("Not hungry");
            return false;
        }
        
        Player._Player.hunger = Mathf.Clamp01(Player._Player.hunger+hungerAmount);
        Player._Player.thirst = Mathf.Clamp01(Player._Player.thirst+thirstAmount);
        Player._Player.onPlayerStateChanged?.Invoke();
        GameManager._GameManager.stats["Eaten food"]+=1;
        return true;
    }
}
