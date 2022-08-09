using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : Item
{
    public override string Name => "\"Book about boats\"";
    public Sprite image;
    public override Sprite img => image;


    public override bool Use()
    {
        Player._Player.buildingBoat = true;
        HintManager.setHint("I should build boat near beach.",3);
        foreach (var item in GameObject.FindObjectsOfType<Flag>())
        {
            item.tag = "Item";
        }
        return true;
    }
}
