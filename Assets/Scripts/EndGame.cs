using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Player")
        {
            HintManager.setHint("Press E to finish the game.");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.LogError("Gameover");
               
            }
        }
    }
}
