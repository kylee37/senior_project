using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeResearch : MonoBehaviour
{
    public int recipeCode; //������ �ڵ�
    public float price; //����
    float xp; //���� xp
    public GameObject textUi; //ǥ�� UI
    public TMP_Text printText; //ǥ�� UI �ؽ�Ʈ
    public XPManager xpManager; //xp�Ŵ��� ��ũ��Ʈ

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    void Update()
    {
        xp = xpManager.xp; //xp�Ŵ������� xp �����ϴ� ���� ������
    }

    void OnClick()
    {
        if (price <= xp) //�ʿ�xp���� xp�� ũ�ų� ������
        {
            xpManager.xp -= price; //���� xp���� �ʿ�xp��ŭ�� xp�� ��

            printText.text = "������ ���� ����!"; //ǥ�� UI�� ���� �ֱ�
        }
        else if (price > xp) //�ʿ�xp���� xp�� ������
        {
            printText.text = "������ �����մϴ�."; //ǥ�� UI�� ���� �ֱ�
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