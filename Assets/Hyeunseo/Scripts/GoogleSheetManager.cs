using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking; //���� ���

public class GoogleSheetManager : MonoBehaviour
{
    [SerializeField] FurnitureSO furnitureSO;
    [SerializeField] FoodSO foodSO;
    [SerializeField] MenuSO menuSO;

    void Start()
    {
        StartCoroutine(DownloadFurnitureSO()); //���� ������ �������� �ڷ�ƾ ����
        StartCoroutine(DownloadFoodSO()); //���� ������ �������� �ڷ�ƾ ����
    }

    //���� �������� ��Ʈ /edit���������� ���� + /export?format=tsv | range=��Ʈ ���� = ���� ���� | gid=�������� ��Ʈ ��ũ edit#gid=���� ���� = ���ϴ� ��Ʈ ����
    const string URL1 = "https://docs.google.com/spreadsheets/d/1KDhB4i4D9H5fO-u7nYITeFAqF22QBo5dlmHwUFytml8/export?format=tsv&range=A2:F&gid=0"; //���� �������� ��Ʈ
    const string URL2 = "https://docs.google.com/spreadsheets/d/1KDhB4i4D9H5fO-u7nYITeFAqF22QBo5dlmHwUFytml8/export?format=tsv&range=A2:H&gid=1439547288"; //���� �������� ��Ʈ

    IEnumerator DownloadFurnitureSO() //���� ���������Ʈ �������� �ڷ�ƾ
    {
        UnityWebRequest www = UnityWebRequest.Get(URL1); //�����ϸ� URL�� ������
        yield return www.SendWebRequest();
        SetFurnitureSO(www.downloadHandler.text);
    }

    IEnumerator DownloadFoodSO() //���� ���������Ʈ �������� �ڷ�ƾ
    {
        UnityWebRequest www = UnityWebRequest.Get(URL2); //�����ϸ� URL�� ������
        yield return www.SendWebRequest();
        SetFoodSO(www.downloadHandler.text);
    }

    void SetFurnitureSO(string tsv) //���������Ʈ �� �����ͼ� FurnitureSO�� �ֱ�
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for(int i = 0; i < rowSize; i++)
        {
            string[] comumn = row[i].Split('\t');

            for(int j = 0; j < columnSize; j++)
            {
                Furniture targetFurniture = furnitureSO.furnitures[i];

                targetFurniture.num = int.Parse(comumn[0]);
                targetFurniture.human = int.Parse(comumn[1]);
                targetFurniture.elf = int.Parse(comumn[2]);
                targetFurniture.dwarf = int.Parse(comumn[3]);
                targetFurniture.name = comumn[4];
                targetFurniture.price = int.Parse(comumn[5]);
            }
        }
    }

    void SetFoodSO(string tsv) //���������Ʈ �� �����ͼ� FoodSO�� MenuSO�� �ֱ�
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] comumn = row[i].Split('\t');

            for (int j = 0; j < columnSize; j++)
            {
                Food targetFood = foodSO.foods[i];
                Menu targetMenu = menuSO.menus[i];

                targetFood.num = int.Parse(comumn[0]);
                targetMenu.num = int.Parse(comumn[0]);

                targetFood.human = int.Parse(comumn[1]);
                targetFood.elf = int.Parse(comumn[2]);
                targetFood.dwarf = int.Parse(comumn[3]);
                targetMenu.name = (comumn[4]);
                targetMenu.explanation = (comumn[5]);
                targetMenu.price = int.Parse(comumn[6]);
            }
        }
    }
}
