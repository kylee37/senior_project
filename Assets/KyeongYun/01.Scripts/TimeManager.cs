using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TMP_Text timerText;

    private float timer = 0f;

    private void Start()
    {
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return null; // 한 프레임을 기다립니다.

            timer += Time.deltaTime; // 경과된 시간을 누적합니다.

            // 시간을 시, 분, 초로 변환합니다.
            //int hours = (int)(timer / 3600);
            int minutes = (int)((timer % 3600) / 60);
            int seconds = (int)(timer % 60);

            // 타이머 텍스트 업데이트
            timerText.text = 
            //hours.ToString("D2") + ":" +  시간 표시하려면 주석 제거
            minutes.ToString("D2") + ":" + 
            seconds.ToString("D2");
        }
    }
}
