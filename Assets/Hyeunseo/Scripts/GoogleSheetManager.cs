using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking; //웹과 통신

public class GoogleSheetManager : MonoBehaviour
{
    [SerializeField] FurnitureSO furnitureSO;
    [SerializeField] FoodSO foodSO;
    [SerializeField] MenuSO menuSO;

    void Start()
    {
        StartCoroutine(DownloadFurnitureSO()); //가구 데이터 가져오는 코루틴 실행
        StartCoroutine(DownloadFoodSO()); //음식 데이터 가져오는 코루틴 실행
    }

    //구글 스프레드 시트 /edit이전까지의 내용 + /export?format=tsv | range=시트 범위 = 범위 지정 | gid=스프레드 시트 링크 edit#gid=뒤의 숫자 = 원하는 시트 지정
    const string URL1 = "https://docs.google.com/spreadsheets/d/1KDhB4i4D9H5fO-u7nYITeFAqF22QBo5dlmHwUFytml8/export?format=tsv&range=A2:F&gid=0"; //가구 스프레드 시트
    const string URL2 = "https://docs.google.com/spreadsheets/d/1KDhB4i4D9H5fO-u7nYITeFAqF22QBo5dlmHwUFytml8/export?format=tsv&range=A2:H&gid=1439547288"; //음식 스프레드 시트

    IEnumerator DownloadFurnitureSO() //가구 스프레드시트 가져오는 코루틴
    {
        UnityWebRequest www = UnityWebRequest.Get(URL1); //시작하면 URL을 가져옴
        yield return www.SendWebRequest();
        SetFurnitureSO(www.downloadHandler.text);
    }

    IEnumerator DownloadFoodSO() //음식 스프레드시트 가져오는 코루틴
    {
        UnityWebRequest www = UnityWebRequest.Get(URL2); //시작하면 URL을 가져옴
        yield return www.SendWebRequest();
        SetFoodSO(www.downloadHandler.text);
    }

    void SetFurnitureSO(string tsv) //스프레드시트 값 가져와서 FurnitureSO에 넣기
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

    void SetFoodSO(string tsv) //스프레드시트 값 가져와서 FoodSO랑 MenuSO에 넣기
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
