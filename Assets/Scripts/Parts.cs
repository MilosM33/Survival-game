using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : Item, IStackable
{
    public string name;
    public Sprite image;
    public byte maxCount;

    public byte numberOfItem = 1;
    public override string Name => name;

    public override Sprite img => image;

    public byte count { get => numberOfItem; set => numberOfItem = value; }

    public byte countMax => maxCount;   



}
