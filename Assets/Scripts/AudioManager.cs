using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;
    void Start()
    {
        _AudioManager = this;
        Init();

        if (mainMenu)
        {
            PlaySound("sea");
            PlaySound("morning");
        }
    }
    public static AudioManager _AudioManager;
    public bool mainMenu = false;
    public void Init()
    {
        foreach (var item in sounds)
        {
            AudioSource audioSource =gameObject.AddComponent<AudioSource>();
            audioSource.clip = item.audio;
            audioSource.loop = item.loop;
            audioSource.volume = item.volume;
            item.audioSource = audioSource;
        }
    }
    public  void PlaySound(string id)
    {
        Sound s =sounds.Find(p => p.id == id);
        
        if (s != null)
        {
            s.audioSource.Play();
            
        }
        else
        {
            Debug.LogError("Sound not found");
            
        }
    }
    public void StopSound(string id)
    {
        Sound s = sounds.Find(p => p.id == id);

        if (s != null)
        {
            s.audioSource.Stop();
        }
        else
        {
            Debug.LogError("Sound not found");
            
        }
    }
    public void StopSoundSmooth(string id,float time)
    {
        Sound s = sounds.Find(p => p.id == id);
        StartCoroutine(Smooth(time, s));
        
    }
    IEnumerator Smooth(float time,Sound s)
    {
        float temp = 0;
        float dec = s.volume / time;
        while(temp < time)
        {
            temp += Time.deltaTime;
            s.audioSource.volume -= dec*Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            
        }
        if (s != null)
        {
            s.audioSource.Stop();

        }
        else
        {
            Debug.LogError("Sound not found");
            Debug.Break();
        }
        s.audioSource.volume = s.volume;
    }
}
