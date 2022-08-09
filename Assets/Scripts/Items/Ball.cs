using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Item,IStackable
{
    public override string Name => "Ball";

    public override Sprite img => image;

    [SerializeField]private Sprite image;
    private byte numberOfItem = 1;
    public byte count { get => numberOfItem; set => numberOfItem = value; }

    public byte countMax => 2;
    

}
