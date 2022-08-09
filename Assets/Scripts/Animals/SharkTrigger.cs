using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Fish") && (Random.Range(0, 10) == 2))
        {
            Debug.Log(other.name);
            Destroy(other.gameObject);
            AnimalController._AnimalController.SpawnFish();

        }
        else if (other.name == "Player")
        {
            GameOver._GameOver.OnGameOver("You were killed by shark.");
        }


    }
}
