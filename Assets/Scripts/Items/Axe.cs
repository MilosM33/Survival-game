using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item,IEquipable
{
    public Sprite image;
    public override string Name => "Axe";

    public override Sprite img => image;
    Animator animator;
    public Vector3 posOffset => new Vector3(-0.87f,0.3f,-0.45f);

    public Vector3 rotOffset => new Vector3(-156, 90, -60);

    public void Equip()
    {
        gameObject.SetActive(true);
        GameObject.Find("Player").GetComponent<Player>().AddItemToHand(transform,posOffset,rotOffset);
        animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.applyRootMotion = false;
    }
    

    
    public void Unequip()
    {
        gameObject.SetActive(true);
        GameObject.Find("Player").GetComponent<Player>().RemoveItemFromHand(transform,false);
        animator.enabled = false;
    }
    IEnumerator playAnim()
    {
        animator.SetBool("use", true);
        yield return new WaitForSeconds(0.18f);
        animator.SetBool("use", false);
    }
    public override bool Use()
    {
        if (Player._Player.stamina - 0.2f < 0)
        {
            HintManager.setHint("Low stamina", 1);
            return false;
        }
        StartCoroutine(playAnim());

        Player._Player.stamina -= 0.2f;
        Player._Player.energy -= 0.02f;
        if (Player._Player.selectedObject == null)
            return false;
        Breakable breakable = Player._Player.selectedObject.GetComponent<Breakable>();
        if (breakable != null)
        {
            breakable.DoDamage(1,transform.position); 
        }
        else
        {
            SharkAi shark = Player._Player.selectedObject.GetComponent<SharkAi>();
            if(shark != null)
            {
                shark.TakeDamage(1, transform.parent.position);
                shark.player = Player._Player.gameObject.transform;
            }
        }
        return true;
    }

    


}
