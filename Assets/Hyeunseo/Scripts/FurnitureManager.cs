using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FurnitureManager : MonoBehaviour
{
    public int limit = 8; //���� ����

    List<int> furnitureList; //���� �����ڵ带 �����ϴ� ����Ʈ
    List<int> furnitureNumList; //���� ������ �����ϴ� ����Ʈ

    public List<GameObject> furnitureUiList = new List<GameObject>(); //UI ����� ����Ʈ
    public List<TMP_Text> furnitureTextList = new List<TMP_Text>(); //���� ǥ�� UI�� ����Ʈ

    void Start()
    {
        furnitureList = new List<int>(new int[limit]);
        furnitureNumList = new List<int>(new int[limit]);
    }

    void Update()
    {
        StartCoroutine(UiActivate()); //UI Ȱ��ȭ �ڷ�ƾ ����
    }

    public void HandleFurnitureCode(int furnitureCode) //�����ڵ带 ���޹޴� �޼���
    {
        /*if(furnitureList.Count == 0)
        {
            furnitureList.Add(furnitureCode); //����Ʈ�� ����
            Debug.Log(furnitureCode);
            furnitureNumList[furnitureCode] = 1; //���� �ڵ��� �ش��ϴ� �ε����� 1�� ����
            Debug.Log(furnitureNumList[furnitureCode]);
        }
        else
        {
            foreach (var ist in furnitureList)
            {
                if (furnitureCode == ist) //���� ���� �ڵ尡 �̹� ����Ʈ�� �ִٸ�
                {
                    furnitureNumList[furnitureCode]++; //���� �ڵ��� �ش��ϴ� �ε����� 1�� �߰�
                    Debug.Log(furnitureNumList[furnitureCode]);
                }
                else //���� �ڵ尡 ����Ʈ�� ���ٸ�
                {
                    furnitureList[furnitureCode] = furnitureCode; //����Ʈ�� ����
                    Debug.Log(furnitureCode);
                    furnitureNumList[furnitureCode] = 1; //���� �ڵ��� �ش��ϴ� �ε����� 1�� ����
                    Debug.Log(furnitureNumList[furnitureCode]);
                }

                furnitureTextList[furnitureCode].text = furnitureNumList[furnitureCode].ToString(); //���� ǥ�� UI�� ���� ������Ʈ ��
            }
        }*/

        if (furnitureList.Count == 0)
        {
            furnitureList.Add(furnitureCode); //����Ʈ�� ����
            furnitureNumList[furnitureCode] = 1; //���� �ڵ��� �ش��ϴ� �ε����� 1�� ����
        }
        else
        {
            bool codeExists = false; // ���� �ڵ尡 �̹� ����Ʈ�� �ִ��� ���θ� Ȯ���ϴ� �� ����
            for (int i = 0; i < furnitureList.Count; i++)
            {
                if (furnitureList[i] == furnitureCode)
                {
                    furnitureNumList[furnitureCode]++; //���� �ڵ��� �ش��ϴ� �ε����� 1�� �߰�
                    codeExists = true; // ���� �ڵ尡 �̹� ����Ʈ�� �ִ� ��� true
                    break;
                }
            }

            // ���� �ڵ尡 ����Ʈ�� ���� ��� ����Ʈ�� �߰��ϰ� �ε��� ���� 1�� ����
            if (!codeExists)
            {
                furnitureList.Add(furnitureCode); //����Ʈ�� ����
                furnitureNumList[furnitureCode] = 1; //���� �ڵ��� �ش��ϴ� �ε����� 1�� ����
            }
        }

        // UI �ؽ�Ʈ ������Ʈ
        furnitureTextList[furnitureCode].text = furnitureNumList[furnitureCode].ToString();
    }

    IEnumerator UiActivate() //UI Ȱ��ȭ�ϴ� �ڷ�ƾ
    {
        for(int i = 0; i < limit; i++) //���� �ִ� �������� ��� ����
        {
            if(furnitureList.Contains(i)) //���� UI �ε����� �ش��ϴ� �ڵ尡 �ִٸ�
            {
                furnitureUiList[i].SetActive(true); //���� UI�� Ȱ��ȭ
            }
        }

        yield return new WaitForSeconds(1f);
    }
}
