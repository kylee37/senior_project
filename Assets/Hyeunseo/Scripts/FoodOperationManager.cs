using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodOperationManager : MonoBehaviour //�ڵ尡 ���������� ���� �����ϱ� ���� ���� ���� ��� �ý����� ���� ��� �ؽ�Ʈ�� �����ϰ� ������ �Ŵ����� ���� �⺻ ��ȣ�� += ���� ��ȣ�� + ���� ��ȣ���� ���� ����
{
    //�� ������ ��ȣ���� ������ ����
    private int sumHuman = 0;
    private int sumElf = 0;
    private int sumDwarf = 0;

    public TMP_Text operText; //���� ����â�� �ؽ�Ʈ

    public MenuManager menuManager; //�޴�����Ʈ Ȯ���� ���� �޴� �Ŵ����� ������
    public MenuReset menuReset; //�޴� ���� Ȯ���� ���� �޴� ������ ������

    public FoodSO foodSO; // FoodSO ����

    HashSet<int> processedFoodCodes = new HashSet<int>(); // �̹� ó���� ���� �ڵ带 �����ϴ� HashSet

    bool resetCheck = false; //���� ���¸� Ȯ�����ִ� ����

    void Update()
    {
        StartCoroutine(UpdateOper()); //������ ������ ��ȣ���� ���� ��ȣ���� �ݿ��ϴ� �ڷ�ƾ ����

        operText.text = "�ΰ� ��ȣ�� - " + sumHuman + "\n" + "���� ��ȣ�� - " + sumElf + "\n" + "����� ��ȣ�� - " + sumDwarf;

        if (menuReset.reset && resetCheck == false) //���� �޴� ���� ��ư�� ������, ���� üũ�� flase�� ���
        {
            StartCoroutine(FoodCountReset()); //�������� �߰��� ���� �����ϴ� �ڷ�ƾ ȣ��
            resetCheck = true; //���� üũ true�� ����
        }
        else if (!menuReset.reset) //�޴� ���� ��ư�� �ʱ�ȭ�Ǹ�
        {
            StopCoroutine(FoodCountReset()); //�ڷ�ƾ�� �ߴ�
            resetCheck = false; //���� üũ false�� ����
        }
    }

    IEnumerator UpdateOper()
    {
        for (int i = 0; i < menuManager.foodList.Count; i++)
        {
            int foodCode = menuManager.foodList[i]; //foodCode�� foodList[i]�� �Ҵ�

            if (processedFoodCodes.Contains(foodCode)) //�̹� ó���� ���� �ڵ�� ���̻� ó������ ����
            {
                continue;
            }

            for (int j = 0; j < foodSO.foods.Length; j++)
            {
                Food food = foodSO.foods[j]; //food�� foods[j] �� �Ҵ�

                if (food.num == foodCode) //food.num�� foodCode�� ������
                {
                    // �ش� ������ humen, elf, dwerf ���� ����
                    sumHuman += food.human;
                    sumElf += food.elf;
                    sumDwarf += food.dwarf;

                    processedFoodCodes.Add(foodCode); //�̹� ó���� �ڵ�� ǥ��

                    break; //Ż��
                }
            }
        }

        yield break;
    }

    IEnumerator FoodCountReset()
    {
        sumHuman = 0;
        sumElf = 0;
        sumDwarf = 0;

        processedFoodCodes.Clear();

        yield break;
    }
}
