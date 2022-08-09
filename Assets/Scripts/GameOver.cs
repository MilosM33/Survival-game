using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameOver : MonoBehaviour
{
    public GameObject canvas;

    public static GameOver _GameOver;
    public void Start()
    {
        _GameOver = this;
    }
    public void OnGameOver(string text)
    {
       GameObject temp= Instantiate(canvas);
        GameObject.Find("GameOver_Result").GetComponent<TextMeshProUGUI>().text = text;

        string statistics = "";
        for (int i = 0; i < GameManager._GameManager.stats.Count; i++)
        {
            var value = GameManager._GameManager.stats.ElementAt(i);
            statistics += $"{value.Key}: {value.Value}\n";
        }
        GameObject.Find("GameOver_Statistics").GetComponent<TextMeshProUGUI>().text = statistics;
        GameManager._GameManager.freeze = true;
        Time.timeScale = 0;

    }
}
