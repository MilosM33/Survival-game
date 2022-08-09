using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAudioTrigger : MonoBehaviour
{

    bool playing = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" && !playing)
        {
            AudioManager._AudioManager.PlaySound("sea");
            playing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            AudioManager._AudioManager.StopSoundSmooth("sea",1.5f);
            playing = false;
        }
    }
}
