using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    public GameObject mainmenu;
    public int current =5;
    public void ChangeProfile()
    {
        current = (current >= 5 ? 0 : current + 1);
        GameObject.Find("quality").GetComponent<TextMeshProUGUI>().text = Enum.GetName(typeof(QualityLevel), current);
        QualitySettings.SetQualityLevel(current);

    }

    public void CloseTab()
    {
        mainmenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void OpenTab()
    {
        mainmenu.SetActive(false);
        gameObject.SetActive(true);
    }
}
