using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FixedTimeManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text dayText;

    private float timer = 0f;
    public float timeSeconds = 0f;

    public int seconds = 4; //게임 시간 30분이 현실에서 몇초인지

    int days = 1;
    int minutes = 0;
    int hours = 4;

    public Button startButton; //일차 시작하는 버튼 UI
    public Button pauseButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        pauseButton.onClick.AddListener(OnStopButtonClick);
        dayText.text = "Day" + days.ToString("D1");
    }

    void OnStartButtonClick()
    {
        StartCoroutine(UpdateTimer());

        startButton.gameObject.SetActive(false);
    }

    void OnStopButtonClick()
    {
        StopCoroutine(UpdateTimer());

        startButton.gameObject.SetActive(true);
    }

    IEnumerator UpdateTimer()
    {
        hours = 4;

        while (true)
        {
            yield return null; // 한 프레임을 기다립니다.

            timer += Time.deltaTime; // 경과된 시간을 누적합니다.
            timeSeconds += Time.deltaTime;

            // 시간을 시, 분, 초로 변환합니다.
            //int hours = (int)(timer / 3600);
            //int minutes = (int)((timer % 3600) / 60);
            //int seconds = (int)(timer % 60);

            //게임 시간에 맞춰 변경
            if (timer >= seconds)
            {
                if (minutes == 0)
                {
                    minutes += 30;
                }
                else if (minutes == 30)
                {
                    minutes = 0;
                    hours++;
                }

                timer = 0;
            }

            if (hours == 12 && minutes == 30)
            {
                minutes = 0;
                hours = 0;
                //타이머 텍스트 업데이트
                timerText.text =
                hours.ToString("D2") + ":" +
                minutes.ToString("D2");

                //데이 텍스트 업데이트
                days++;
                dayText.text = "Day" + days.ToString("D1");

                startButton.gameObject.SetActive(true);

                //GameStop
                yield break;
            }

            // 타이머 텍스트 업데이트
            timerText.text =
            //hours.ToString("D2") + ":" +  시간 표시하려면 주석 제거
            hours.ToString("D2") + ":" +
            minutes.ToString("D2");
        }
    }
}
