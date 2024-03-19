using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static int limit = 3; //����Ʈ�� ����� �� �ִ� �ִ� ����
    public List<int> foodList = new List<int>(new int[limit]); //���� �ڵ带 ��� ����Ʈ
    public GameObject addPanel; //�޴� ��� UI (�������� ���� ����� �����ϱ� ���� �ʿ�)
    public MenuReset menuReset; //�޴� ���� ��ũ��Ʈ

    //����� ����Ʈ
    public List<Image> foodImageList = new List<Image>(); //��� ���� �̹����� ��� ����Ʈ
    [TextArea]
    public List<string> foodTextList = new List<string>(); //��� ���� ������ ��� ����Ʈ
    //�޴��� ����Ʈ
    public List<Image> manuImageList = new List<Image>(); //�޴��� �̹����� ��� ����Ʈ
    public List<TMP_Text> manuTextList = new List<TMP_Text>(); //�޴��� ������ ��� ����Ʈ

    private void Awake()
    {
        foodList = new List<int>(new int[limit]);

        for (int i = 0; i < limit; i++)
        {
            foodList[i] = 0;
        }
    }

    public void HandleFoodCode(int foodCode) //���� �ڵ带 �޾� ó���ϴ� �޼���
    {
        //����Ʈ�� ��ϵ��� �ʾҰ�
        if (!foodList.Contains(foodCode))
        {
            // foodList���� ���� 0�� ù ��° �ε����� ���� �ڵ� �߰�
            int indexOfZero = foodList.IndexOf(0);
            if (indexOfZero != -1)
            {
                foodList[indexOfZero] = foodCode;
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
                coroutineCheck1 = false; //�ڷ�ƾ ���� false
            }
        }

        if (menuReset.reset) //���� �ʱ�ȭ ��ư�� �����ٸ�
        {
            if(coroutineCheck2 == false)
            {
                StartCoroutine(ResetMenu()); //�ڷ�ƾ ȣ��
                coroutineCheck2 = true; //�ڷ�ƾ ���� true
            }
        }
        else if (!menuReset.reset)
        {
            if(coroutineCheck2 == true)
            {
                StopCoroutine(ResetMenu()); //�ڷ�ƾ �ߴ�
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
                if (foodList[i] < foodImageList.Count && foodList[i] < foodTextList.Count) //�ε��� ���� ���� ���� ��
                {
                    manuImageList[i].sprite = foodImageList[foodList[i]].sprite; //�޴��ǿ� ���� ��ȣ�� �ش��ϴ� ���� �̹����� ����
                    manuTextList[i].text = foodTextList[foodList[i]]; //�޴��ǿ� ���� ��ȣ�� �ش��ϴ� ���� ������ ����
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator ResetMenu()
    {
        int manuImageListCount = manuImageList.Count;
        int manuTextListCount = manuTextList.Count;

        for (int i = 0; i < limit; i++)
        {
            foodList[i] = 0;
            if (foodList[i] == 0) //���� 0�̰�
            {
                if (foodImageList.Count > 0 && foodTextList.Count > 0) //�ش� ����Ʈ�� ����ִٸ�
                {
                    if (i < manuImageListCount && i < manuTextListCount) //�ε��� ���� ���� ���� ��
                    {
                        manuImageList[i].sprite = foodImageList[0].sprite; //��������Ʈ ����Ʈ  0������ ����
                        manuTextList[i].text = foodTextList[0]; //�ؽ�Ʈ ����Ʈ 0������ ����
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
    }
}
