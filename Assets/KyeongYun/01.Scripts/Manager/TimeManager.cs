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

    public int seconds = 4; //���� �ð� 30���� ���ǿ��� ��������

    int days = 1;
    int minutes = 0;
    int hours = 4;

    public Button startButton; //���� �����ϴ� ��ư UI

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        dayText.text = "Day" + days.ToString("D1");
    }

    void OnStartButtonClick()
    {
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        hours = 4;

        while (true)
        {
            yield return null; // �� �������� ��ٸ���

            timer += Time.deltaTime; // ����� �ð��� ����
            timeSeconds += Time.deltaTime;

            // �ð��� ��, ��, �ʷ� ��ȯ
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
