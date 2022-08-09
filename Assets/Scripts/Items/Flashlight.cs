using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item, IEquipable
{
    public override string Name => "FlashLight";

    [SerializeField] private Sprite image;
    public override Sprite img => image;

    public Vector3 posOffset => new Vector3(-0.3f,1,-1.5f);

    public Vector3 rotOffset => new Vector3(-90,0,0);

    GameObject light;

    public void Awake()
    {
        light = transform.GetChild(0).gameObject;
    }
    public void Equip()
    {
        gameObject.SetActive(true);
        
        Player._Player.AddItemToHand(transform,posOffset,rotOffset);
    }

    public void Unequip()
    {
        gameObject.SetActive(true);
        Player._Player.RemoveItemFromHand(transform,false);
        
    }

    public override bool Use()
    {
        AudioManager._AudioManager.PlaySound("flashlight");
        light.SetActive(!light.activeSelf);
        return true;
    }
}
