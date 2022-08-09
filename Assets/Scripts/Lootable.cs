using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public string Name = "Backpack";
    public List<GameObject> spawn;
    public List<float> probability;
    public int minCount = 2;
    public int maxCount = 4;
    
    
}
