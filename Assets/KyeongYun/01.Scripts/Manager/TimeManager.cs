using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text dayText;

    private float timer = 0f;
    public float timeSeconds = 0f;

    public int seconds = 6; // 게임 시간 30분이 현실에서 몇 초인지

    int days = 1;
    int minutes = 0;
    int hours = 4;

    public Button startButton; // 일차 시작하는 버튼 UI
    public Button lockMenuButton; // 메뉴판 잠금 버튼
    public Button lockArgmButton; // 가구 배치 잠금 버튼
    public GameObject textUi; // 표시 UI
    public TMP_Text printText; // 표시 UI 텍스트

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        dayText.text = days.ToString("D1");
    }

    private void Update()
    {
        // 록 버튼을 눌렀을 경우
        lockMenuButton.onClick.AddListener(LockButtonClick);
        lockArgmButton.onClick.AddListener(LockButtonClick);
    }

    void LockButtonClick()
    {
        printText.text = "일차 진행 중에 사용할 수 없습니다.";
        textUi.SetActive(true); // 표시 UI 활성화
        Invoke(nameof(CloseUI), 0.5f); // 0.5초 뒤 UI 삭제
    }

    void CloseUI()
    {
        // UI를 비활성화
        if (textUi != null)
        {
            textUi.SetActive(false);
        }
    }

    void OnStartButtonClick()
    {
        StartCoroutine(UpdateTimer());

        startButton.gameObject.SetActive(false);
        lockMenuButton.gameObject.SetActive(true);
        lockArgmButton.gameObject.SetActive(true);
    }

    IEnumerator UpdateTimer()
    {
        hours = 4;
        minutes = 0;

        while (true)
        {
            yield return null; // 한 프레임을 기다립니다.

            timer += Time.deltaTime; // 경과된 시간을 누적합니다.
            timeSeconds += Time.deltaTime;

            // 게임 시간에 맞춰 변경
            if (timer >= seconds)
            {
                timer = 0;

                if (minutes == 0)
                {
                    minutes = 30;
                }
                else
                {
                    minutes = 0;
                    hours++;
                }

                // 타이머 텍스트 업데이트
                timerText.text =
                    hours.ToString("D2") + ":" +
                    minutes.ToString("D2");

                // 12시 30분이 되면 하루를 증가시킵니다.
                if (hours == 12 && minutes == 0)
                {
                    days++;
                    dayText.text = days.ToString("D1");

                    startButton.gameObject.SetActive(true);
                    lockMenuButton.gameObject.SetActive(false);
                    lockArgmButton.gameObject.SetActive(false);

                    // GameStop
                    yield break;
                }
            }
        }
    }
}
