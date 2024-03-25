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
            yield return null; // �� �������� ��ٸ��ϴ�.

            timer += Time.deltaTime; // ����� �ð��� �����մϴ�.

            // �ð��� ��, ��, �ʷ� ��ȯ�մϴ�.
            //int hours = (int)(timer / 3600);
            int minutes = (int)((timer % 3600) / 60);
            int seconds = (int)(timer % 60);

            // Ÿ�̸� �ؽ�Ʈ ������Ʈ
            timerText.text = 
            //hours.ToString("D2") + ":" +  �ð� ǥ���Ϸ��� �ּ� ����
            minutes.ToString("D2") + ":" + 
            seconds.ToString("D2");
        }
    }
}
