using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUIManager : MonoBehaviour
{
    public Button[] buttons; // ��ư UI ����Ʈ

    private void Start()
    {
        // ��� ��ư�� ���¸� �ʱ�ȭ
        ResetButtonStates();

        // �� ��ư�� Ŭ�� �̺�Ʈ�� OnButtonClick �Լ� ����
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Ŭ���� ����� ���� index�� ���������� ����
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void OnButtonClick(int index)
    {
        // Ŭ���� ��ư�� ������ �ٸ� ��ư���� ���� ����
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i != index)
            {
                buttons[i].image.color = new Color(0.85f, 0.85f, 0.85f, 1f); // ��Ӱ� ����
            }
            else
            {
                buttons[i].image.color = new Color(1f, 1f, 1f, 1f); // Ŭ���� ��ư�� ���� �������� ����
            }
        }
    }

    private void ResetButtonStates()
    {
        // ��� ��ư�� ������ ������� �ʱ�ȭ
        foreach (Button button in buttons)
        {
            button.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
