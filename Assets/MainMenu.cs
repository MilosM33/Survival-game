using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public List<Transform> dir;
    public Transform current;
    public float minSpeed;
    public float maxSpeed = 1;
    public float speed = 1;
    int index = 0;
    bool done = true;
    float t = 0;
    void Start()
    {
        current = dir[0];
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(Camera.main.transform.forward, (current.position - transform.position).normalized) != 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(current.position - transform.position), t);
            t += Time.deltaTime * speed;
        }
        else if(done)
        {
            done = false;
            StartCoroutine(ChangeDirection());
        }
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(0.5f,5));
        index = Random.Range(0, dir.Count);
        current = dir[index];
        
        speed = Random.Range(minSpeed, maxSpeed);
        t = 0;
        done = true;
       
        
    }

    public void LoadGame(bool newGame)
    {
        SceneManager.LoadScene((int)SceneIDs.Game);
    }
}
