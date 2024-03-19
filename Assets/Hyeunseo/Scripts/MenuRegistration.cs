using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuRegistration : MonoBehaviour
{
    public int foodCode = 1; //���� �ڵ�
    public GameObject textUi; //ǥ�� UI
    public TMP_Text printText; //ǥ�� UI �ؽ�Ʈ
    public MenuManager menuManager; //�޴� �Ŵ��� ��ũ��Ʈ
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        if (menuManager != null) // ���� menuManager�� null�� �ƴ϶��
        {
            if (menuManager.foodList.Contains(0)) //�޴��Ŵ����� Ǫ�帮��Ʈ�� 0�� �ڸ��� �ִٸ�
            {
                menuManager.HandleFoodCode(foodCode); // foodCode ���� MenuManager ��ũ��Ʈ�� ����
                Debug.Log("�� ����");
                printText.text = "��� �Ϸ�!"; //ǥ�� UI�� ���� �ֱ�
            }
            else
            {
                printText.text = "�޴����� �����մϴ�."; //ǥ�� UI�� ���� �ֱ�
            }
        }
        else
        {
            printText.text = "�޴����� �����ϴ�."; //ǥ�� UI�� ���� �ֱ�
        }
        textUi.SetActive(true); //ǥ�� UI Ȱ��ȭ
        Invoke("CloseUI", 0.5f); //0.5�� �� UI ����
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
