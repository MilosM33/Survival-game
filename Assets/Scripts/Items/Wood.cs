using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Item,IStackable
{

    public string Type;
    public override string Name => Type;
    public Sprite image;
    public override Sprite img => image;

    private byte numberOfItem = 1;
    public byte count { get => numberOfItem; set => numberOfItem = value; }

    public byte countMax => 10;
}
