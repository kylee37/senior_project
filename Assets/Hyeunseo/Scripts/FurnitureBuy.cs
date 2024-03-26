using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FurnitureBuy : MonoBehaviour
{
    public int furnitureCode; //�����ڵ�
    public float price; //����
    float gold; //���� ���
    public GameObject textUi; //ǥ�� UI
    public TMP_Text printText; //ǥ�� UI �ؽ�Ʈ
    public GoldManager goldManager; //���Ŵ��� ��ũ��Ʈ
    public FurnitureManager furnitureManager; //�����Ŵ��� ��ũ��Ʈ

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
        gold = goldManager.gold; //���Ŵ������� ��� �����ϴ� ���� ������
    }

    void OnClick()
    {
        if (price <= gold) //���ݺ��� ������尡 ũ�ų� ������
        {
            goldManager.gold -= price; //���� ��忡�� ���ݸ�ŭ�� ��带 ��
            furnitureManager.HandleFurnitureCode(furnitureCode); //���� �Ŵ��� ��ũ��Ʈ�� �� ����

            printText.text = "���ſϷ�!"; //ǥ�� UI�� ���� �ֱ�
        }
        else if (price > gold) //���ݺ��� ������尡 ������
        {
            printText.text = "��尡 �����մϴ�."; //ǥ�� UI�� ���� �ֱ�
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
