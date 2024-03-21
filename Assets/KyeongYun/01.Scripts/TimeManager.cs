using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TMP_Text timerText;

    public int timer = 0;

    private void Start()
    {
        StartCoroutine(TimerCoroution());
    }

    IEnumerator TimerCoroution()
    {
        timer += 1;

        timerText.text = (timer / 3600).ToString("D2") + ":" + (timer / 60 % 60).ToString("D2");

        yield return new WaitForSeconds(1/60f);

        StartCoroutine(TimerCoroution());
    }
}
