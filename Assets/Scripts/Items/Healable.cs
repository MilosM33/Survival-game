using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healable : Item
{
    public string name = "Bandage";
    public Sprite image;
    public float amount = 0;
    public override string Name => name;

    public override Sprite img => image;

    public override bool Use()
    {
        if(Player._Player.health < 1)
        {
            float health = Player._Player.health += amount;
            float stamina = Player._Player.maxStamina += amount;
            Player._Player.health = Mathf.Clamp01(health);
            Player._Player.maxStamina = Mathf.Clamp01(stamina);
            Player._Player.onPlayerStateChanged?.Invoke();
            return true;
        }
        HintManager.setHint("Nothing to heal");
        return false;
    }
}
