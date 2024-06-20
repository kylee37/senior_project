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

    public int seconds = 6; // ���� �ð� 30���� ���ǿ��� �� ������

    int days = 1;
    int minutes = 0;
    int hours = 4;

    public Button startButton; // ���� �����ϴ� ��ư UI
    public Button lockMenuButton; // �޴��� ��� ��ư
    public Button lockArgmButton; // ���� ��ġ ��� ��ư
    public GameObject textUi; // ǥ�� UI
    public TMP_Text printText; // ǥ�� UI �ؽ�Ʈ

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        dayText.text = days.ToString("D1");
    }

    private void Update()
    {
        // �� ��ư�� ������ ���
        lockMenuButton.onClick.AddListener(LockButtonClick);
        lockArgmButton.onClick.AddListener(LockButtonClick);
    }

    void LockButtonClick()
    {
        printText.text = "���� ���� �߿� ����� �� �����ϴ�.";
        textUi.SetActive(true); // ǥ�� UI Ȱ��ȭ
        Invoke(nameof(CloseUI), 0.5f); // 0.5�� �� UI ����
    }

    void CloseUI()
    {
        // UI�� ��Ȱ��ȭ
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
            yield return null; // �� �������� ��ٸ��ϴ�.

            timer += Time.deltaTime; // ����� �ð��� �����մϴ�.
            timeSeconds += Time.deltaTime;

            // ���� �ð��� ���� ����
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

                // Ÿ�̸� �ؽ�Ʈ ������Ʈ
                timerText.text =
                    hours.ToString("D2") + ":" +
                    minutes.ToString("D2");

                // 12�� 30���� �Ǹ� �Ϸ縦 ������ŵ�ϴ�.
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
