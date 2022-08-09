using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour,ICloneable
{
    public abstract string Name { get; }
    public abstract Sprite img { get; }
    public virtual void PickUp()
    {
        Debug.Log(Name + " picked up");
        gameObject.SetActive(false);
    }
    public virtual void Drop(bool addForce)
    {
        gameObject.SetActive(true);
        GetComponent<Rigidbody>().useGravity = true;
        if (addForce)
        {
            Vector3 temp = GameObject.FindGameObjectWithTag("Player").transform.position;
            temp += GameObject.FindGameObjectWithTag("Player").transform.forward * 1.12f;
            gameObject.transform.position = temp;
        }
        

    }

    
    public virtual bool Use()
    {
        HintManager.setHint("This feature will come in next update", 4);

        return false;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
