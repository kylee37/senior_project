using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static int limit = 5; //����Ʈ�� ����� �� �ִ� �ִ� ����
    public List<int> foodList = new List<int>(new int[limit]); //���� �ڵ带 ��� ����Ʈ
    public GameObject addPanel; //�޴� ��� UI (�������� ���� ����� �����ϱ� ���� �ʿ�)
    public MenuReset menuReset; //�޴� ���� ��ũ��Ʈ

    //����� ����Ʈ
    public List<Image> foodImageList = new List<Image>(); //��� ���� �̹����� ��� ����Ʈ
    public List<string> foodTextList = new List<string>(); //��� ���� ������ ��� ����Ʈ
    //�޴��� ����Ʈ
    public List<Image> manuImageList = new List<Image>(); //�޴��� �̹����� ��� ����Ʈ
    public List<TMP_Text> manuTextList = new List<TMP_Text>(); //�޴��� ������ ��� ����Ʈ

    private void Awake()
    {
        for (int i = 0; i < limit; i++)
        {
            foodList[i] = 0;
        }

        Debug.Log("foodList�� ��� �ε����� 0���� �ʱ�ȭ");
    }

    public void HandleFoodCode(int foodCode) //���� �ڵ带 �޾� ó���ϴ� �޼���
    {
        //����Ʈ�� ��ϵ��� �ʾҰ� ����Ʈ�� ��ϵ� ���� �ڵ��� ������ ����Ʈ���� �۴ٸ�
        if (!foodList.Contains(foodCode) && foodList.Count < limit)
        {
            int indexOfZero = foodList.IndexOf(0);
            if (indexOfZero != -1)
            {
                foodList[indexOfZero] = foodCode;
                Debug.Log("���� �ڵ� �߰�");
            }
        }
    }

    private void Update()
    {
        bool coroutineCheck1 = false; //��� �ڷ�ƾ�� �������� �������� Ȯ���ϴ� �� ����
        bool coroutineCheck2 = false; //�ʱ�ȭ �ڷ�ƾ�� �������� �������� Ȯ���ϴ� �� ����

        if (addPanel.activeSelf) //addPanel�� Ȱ��ȭ ���°�
        {
            if(coroutineCheck1 == false) //�ڷ�ƾ�� �������� ��
            {
                StartCoroutine(AddManu()); //�ڷ�ƾ ȣ��
                coroutineCheck1 = true; //�ڷ�ƾ ���� true
            }
        }
        else //Ȱ��ȭ�� �ƴ� ��
        {
            if(coroutineCheck1 == true) //�ڷ�ƾ�� �����ִٸ�
            {
                StopCoroutine(AddManu()); //�ڷ�ƾ �ߴ�
                Debug.Log("�ڷ�ƾ �ߴ�");
                coroutineCheck1 = false; //�ڷ�ƾ ���� false
            }
        }

        if (menuReset.reset) //���� �ʱ�ȭ ��ư�� �����ٸ�
        {
            if(coroutineCheck2 == false)
            {
                StartCoroutine(ResetMenu()); //�ڷ�ƾ ȣ��
                Debug.Log("�ʱ�ȭ �ڷ�ƾ ȣ��");
                coroutineCheck2 = true; //�ڷ�ƾ ���� true
            }
        }
        else if (!menuReset.reset)
        {
            if(coroutineCheck2 == true)
            {
                StopCoroutine(ResetMenu()); //�ڷ�ƾ �ߴ�
                Debug.Log("�ʱ�ȭ �ڷ�ƾ �ߴ�");
                coroutineCheck2 = false; //�ڷ�ƾ ���� false
            }
        }
    }

    IEnumerator AddManu() //����Ʈ�� ���� ������ �� ���� �� �޴��� ������Ʈ�� ��� �ڷ�ƾ
    {
        for(int i = 0; i < limit; i++)
        {
            if(foodList[i] != 0) //Ǫ�帮��Ʈ�� ���� �ִٸ�
            {
                manuImageList[i].sprite = foodImageList[foodList[i]].sprite; //�޴��ǿ� ���� ��ȣ�� �ش��ϴ� ���� �̹����� ����
                manuTextList[i].text = foodTextList[foodList[i]]; //�޴��ǿ� ���� ��ȣ�� �ش��ϴ� ���� ������ ����
            }
        }

        yield return 0.5f;
    }

    IEnumerator ResetMenu()
    {
        for(int i = 0; i < limit; i++)
        {
            foodList[i] = 0;
            if (foodList[i] == 0) //���� ������
            {
                manuImageList[i].sprite = foodImageList[0].sprite; //��������Ʈ ����Ʈ  0������ ����
                manuTextList[i].text = foodTextList[0]; //�ؽ�Ʈ ����Ʈ 0������ ����
            }
        }

        yield return 0.5f;
    }
}
