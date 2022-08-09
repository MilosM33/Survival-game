using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public static AnimalController _AnimalController;
    public List<BoxCollider> fishArea;
    public List<GameObject> fishes;
    public GameObject shark;
    public int maxFish = 10;
    public int fishCount = 0;
    public Transform swap;
    void Start()
    {
        _AnimalController = this;
        for (int i = 0; i < maxFish; i++)
        {
            SpawnFish();
            
        }
        
    }
    public IEnumerator SpawnShark(float time)
    {
        yield return new WaitForSeconds(time);
        Bounds temp = fishArea[Random.Range(0, fishArea.Count)].bounds;
        Instantiate(shark, new Vector3(Random.Range(temp.min.x, temp.max.x), Random.Range(temp.min.y, temp.max.y), Random.Range(temp.min.z, temp.max.z)), Quaternion.identity);
    }

    public void SpawnFish()
    {
        Bounds temp = fishArea[Random.Range(0, fishArea.Count)].bounds;
        Instantiate(fishes[Random.Range(0, fishes.Count)], new Vector3(Random.Range(temp.min.x, temp.max.x), Random.Range(temp.min.y, temp.max.y), Random.Range(temp.min.z, temp.max.z)), Quaternion.identity);
        fishCount++;
    }

    float getDistance(Vector3 pos, Vector3 targ)
    {
        targ -= pos;
        return Mathf.Pow(targ.x, 2) + Mathf.Pow(targ.y, 2) + Mathf.Pow(targ.z, 2);
    }

    public Vector3 GetTarget(Vector3 pos)
    {
        Bounds temp = (fishArea[0].bounds.Contains(pos) == true ? fishArea[0].bounds : fishArea[1].bounds);
        if (Random.Range(1, 2) == 1)
        {
            //Swap between 2 places for swimming
            if (getDistance(pos,swap.position) < 200)
            {
                temp = (temp == fishArea[0].bounds ? fishArea[1].bounds : fishArea[0].bounds);
                return new Vector3(Random.Range(temp.min.x, temp.max.x), Random.Range(temp.min.y, temp.max.y), Random.Range(temp.min.z, temp.max.z));
            }
        }
        
        return new Vector3(Random.Range(temp.min.x, temp.max.x), Random.Range(temp.min.y, temp.max.y), Random.Range(temp.min.z, temp.max.z));
    }
    

}
