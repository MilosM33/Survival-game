using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    static TextMeshProUGUI hint;

    private void Awake()
    {
        hintManager = this;
        hint = GetComponent<TextMeshProUGUI>();

    }
    public static bool canSet = true;
    public static HintManager hintManager;
    public static void setHint(string text)
    {
        if(canSet)
            hint.text = text;
    }
    public static void setHint(string text,float time)
    {
        canSet = false;
        hint.text = text;
        hintManager.StartCoroutine(hintManager.showHint(time));
    }

    public IEnumerator showHint(float t)
    {
        
        yield return new WaitForSeconds(t);
        hint.text = "";
        canSet = true;
        
    }




}
