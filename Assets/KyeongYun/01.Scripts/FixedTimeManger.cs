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

    public int seconds = 4; //���� �ð� 30���� ���ǿ��� ��������

    int days = 1;
    int minutes = 0;
    int hours = 4;

    public Button startButton; //���� �����ϴ� ��ư UI
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
            yield return null; // �� �������� ��ٸ��ϴ�.

            timer += Time.deltaTime; // ����� �ð��� �����մϴ�.
            timeSeconds += Time.deltaTime;

            // �ð��� ��, ��, �ʷ� ��ȯ�մϴ�.
            //int hours = (int)(timer / 3600);
            //int minutes = (int)((timer % 3600) / 60);
            //int seconds = (int)(timer % 60);

            //���� �ð��� ���� ����
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
                //Ÿ�̸� �ؽ�Ʈ ������Ʈ
                timerText.text =
                hours.ToString("D2") + ":" +
                minutes.ToString("D2");

                //���� �ؽ�Ʈ ������Ʈ
                days++;
                dayText.text = "Day" + days.ToString("D1");

                startButton.gameObject.SetActive(true);

                //GameStop
                yield break;
            }

            // Ÿ�̸� �ؽ�Ʈ ������Ʈ
            timerText.text =
            //hours.ToString("D2") + ":" +  �ð� ǥ���Ϸ��� �ּ� ����
            hours.ToString("D2") + ":" +
            minutes.ToString("D2");
        }
    }
}
