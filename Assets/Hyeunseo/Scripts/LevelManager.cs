using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    int level = 0; //���� ����
    float exp = 0; //���� ����ġ
    public TMP_Text levelText; //���� ǥ�� UI

    public List<float> levelList = new List<float>(); //���� �������� �ʿ��� ����ġ ������ ����Ʈ
    public List<int> levelXpList = new List<int>(); //���� �������� ���� xp ���޷� ������ ����Ʈ

    public GameObject textUi; //ǥ�� UI
    public TMP_Text printText; //ǥ�� UI �ؽ�Ʈ

    public int totalXP = 0; //XP�� ������ ����

    void Update()
    {
        levelText.text = level.ToString() + ".Lv";

        if (exp >= levelList[level]) //���� ����ġ�� �䱸 ����ġ���� ���ų� ���� ��
        {
            totalXP = levelXpList[level]; //��Ż���ھ xp ���� �� ����

            level++; //������
            exp = 0; //���� ����ġ �ʱ�ȭ

            if (level > 1)
            {
                printText.text = "������!"; //ǥ�� UI�� ���� �ֱ�

                textUi.SetActive(true); //ǥ�� UI Ȱ��ȭ
                Invoke("CloseUI", 0.5f); //0.5�� �� UI ����
            }
        }
    }

    void CloseUI()
    {
        // UI�� ��Ȱ��ȭ
        if (textUi != null)
        {
            textUi.SetActive(false);
        }
    }
}
