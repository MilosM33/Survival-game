using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HeadTrigger : MonoBehaviour
{
     static PostProcessVolume postProcessVolume;
    public PostProcessProfile normal;
    public PostProcessProfile underwater;
    
    Vignette vignette;
    float intensity = 0;
    float smoothness = 0;
    void Start()
    {
        postProcessVolume = GameObject.Find("PostFx").GetComponent<PostProcessVolume>();
        vignette = underwater.GetSetting<Vignette>();

        intensity = vignette.intensity.value;
        smoothness = vignette.smoothness.value;
    }
    
    
    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Water")
        {
            postProcessVolume.profile = underwater;

            Player._Player.oxygen -= Time.deltaTime*0.05f;

            if (Player._Player.oxygen > 0.1f)
            {
                vignette.intensity.value += Time.deltaTime * 0.025f;
                vignette.smoothness.value += Time.deltaTime * 0.05f;
            }
            else if(Player._Player.oxygen < 0.1f)
            {
                GameOver._GameOver.OnGameOver("You were drowned");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Water")
        {
            postProcessVolume.profile = normal;
            vignette.intensity.value = intensity;
            vignette.smoothness.value = smoothness;
            Player._Player.oxygen = 1;
        }
    }

    void OnApplicationQuit()
    {
        vignette.intensity.value = intensity;
        vignette.smoothness.value = smoothness;
    }


}
