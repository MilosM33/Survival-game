using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Item,IStackable
{
    public override string Name => "Apple";

    public Sprite image;
    public override Sprite img => image;

    public byte numberOfItem = 1;
    public byte count { get => numberOfItem; set => numberOfItem = value; }

    public byte countMax => 3;

    public override bool Use()
    {
        if (Player._Player.hunger < 1)
        {
            Player._Player.thirst += 0.2f;
            Player._Player.hunger += 0.1f;
            Player._Player.onPlayerStateChanged?.Invoke();
            return true;
        }
        else
            HintManager.setHint("Not hungry", 1);
        return false;
    }
}
