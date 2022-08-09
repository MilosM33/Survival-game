using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable :MonoBehaviour
{
    public byte health = 5;
    public GameObject result;
    public byte count = 5;
    public ParticleSystem particle;
    public void DoDamage(byte a,Vector3 pos = new Vector3())
    {
        health -= a;
        
        if(particle != null)
        {
            particle.transform.position = pos;
            particle.gameObject.SetActive(true);
            particle.Play();
        }
        if(health <= 0)
        {
            Debug.Log("Destroy");
            if (result == null)
                return;
            StartCoroutine(delay());
            
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.15f);
        Vector3 temp = new Vector3(0, 0.5f, 0);
        for (int i = 0; i < count; i++)
        {
            Instantiate(result, transform.position + temp, Quaternion.identity);
            temp.y += 0.5f;
        }

        if (name.Contains("tree"))
            GameManager._GameManager.stats["Chopped trees"] += 1;
            Destroy(gameObject);
    }
   
}
