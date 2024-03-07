using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuReset : MonoBehaviour
{
    public GameObject textUi; //ǥ�� UI
    public TMP_Text printText; //ǥ�� UI �ؽ�Ʈ
    public bool reset = false; //�޴� �Ŵ����� ������ �� ���� 

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        reset = true;
        printText.text = "�ʱ�ȭ �Ϸ�"; //ǥ�� UI�� ���� �ֱ�


        textUi.SetActive(true); //ǥ�� UI Ȱ��ȭ
        Invoke("CloseUI", 0.5f); //0.5�� �� UI ����, reset ���� false�� ����
    }
    void CloseUI()
    {
        reset = false;
        // UI�� ��Ȱ��ȭ
        if (textUi != null)
        {
            textUi.SetActive(false);
        }
    }
}
