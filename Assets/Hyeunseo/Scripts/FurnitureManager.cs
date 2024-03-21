using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FurnitureManager : MonoBehaviour
{
    List<int> furnitureList = new List<int>(); //���� �����ڵ带 �����ϴ� ����Ʈ
    List<int> furnitureNumList = new List<int>(); //���� ������ �����ϴ� ����Ʈ

    public List<GameObject> furnitureUiList = new List<GameObject>(); //UI ����� ����Ʈ
    public List<TMP_Text> furnitureTextList = new List<TMP_Text>(); //���� ǥ�� UI�� ����Ʈ

    public int limit = 8; //���� ����

    void Start()
    {
        
    }

    void Update()
    {
        furnitureList.Sort(); //����Ʈ ����

        StartCoroutine(UiActivate()); //UI Ȱ��ȭ �ڷ�ƾ ����
    }

    public void HandleFurnitureCode(int furnitureCode) //�����ڵ带 ���޹޴� �޼���
    {
        furnitureList.Add(furnitureCode); //����Ʈ�� ����
        furnitureNumList[furnitureCode] += 1; //���� �ڵ��� �ش��ϴ� �ε����� 1�� �߰�
    }

    IEnumerator UiActivate() //UI Ȱ��ȭ�ϴ� �ڷ�ƾ
    {
        for(int i = 0; i <= limit; i++) //���� �ִ� �������� ��� ����
        {
            if(furnitureList.Contains(i)) //���� UI �ε����� �ش��ϴ� �ڵ尡 �ִٸ�
            {
                furnitureUiList[i].SetActive(true); //���� UI�� Ȱ��ȭ
            }
        }

        yield return new WaitForSeconds(1f);
    }
}
