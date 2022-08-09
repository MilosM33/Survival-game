using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boyancy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * 150);
            Debug.Log("Okey");
        }
    }
}
