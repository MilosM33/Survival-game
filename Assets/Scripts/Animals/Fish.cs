using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    public Vector3 target;
    public float speed = 0.01f;
    public float rotSpeed;
    public virtual void Update()
    {
        if (target == Vector3.zero || getDistance(transform.position, target) < 1)
        {
            ChooseSpot();
        }
        else
        {

            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), rotSpeed * Time.deltaTime);


        }

    }
    public virtual void ChooseSpot()
    {
        target = AnimalController._AnimalController.GetTarget(transform.position);
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            ChooseSpot();
        }
    }
    float getDistance(Vector3 pos, Vector3 targ)
    {
        targ -= pos;
        return Mathf.Pow(targ.x, 2) + Mathf.Pow(targ.y, 2) + Mathf.Pow(targ.z, 2);
    }
}
