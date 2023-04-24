using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCounterUIHandler : MonoBehaviour
{
    Text lapText;

    private void Awake()
    {
        lapText = GetComponent<Text>();
    }

    public void SetLapText(string text)
    {
        lapText.text = text;
    }
}
