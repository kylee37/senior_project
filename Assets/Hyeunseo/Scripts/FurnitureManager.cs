using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FurnitureManager : MonoBehaviour
{
    public int limit = 8; //가구 종류

    List<int> furnitureList; //받은 가구코드를 저장하는 리스트
    List<int> furnitureNumList; //가구 개수를 저장하는 리스트

    public List<GameObject> furnitureUiList = new List<GameObject>(); //UI 저장용 리스트
    public List<TMP_Text> furnitureTextList = new List<TMP_Text>(); //개수 표시 UI용 리스트

    void Start()
    {
        furnitureList = new List<int>(new int[limit]);
        furnitureNumList = new List<int>(new int[limit]);
    }

    void Update()
    {
        StartCoroutine(UiActivate()); //UI 활성화 코루틴 실행
    }

    public void HandleFurnitureCode(int furnitureCode) //가구코드를 전달받는 메서드
    {
        /*if(furnitureList.Count == 0)
        {
            furnitureList.Add(furnitureCode); //리스트에 저장
            Debug.Log(furnitureCode);
            furnitureNumList[furnitureCode] = 1; //가구 코드의 해당하는 인덱스를 1로 만듬
            Debug.Log(furnitureNumList[furnitureCode]);
        }
        else
        {
            foreach (var ist in furnitureList)
            {
                if (furnitureCode == ist) //만약 가구 코드가 이미 리스트에 있다면
                {
                    furnitureNumList[furnitureCode]++; //가구 코드의 해당하는 인덱스에 1을 추가
                    Debug.Log(furnitureNumList[furnitureCode]);
                }
                else //가구 코드가 리스트에 없다면
                {
                    furnitureList[furnitureCode] = furnitureCode; //리스트에 저장
                    Debug.Log(furnitureCode);
                    furnitureNumList[furnitureCode] = 1; //가구 코드의 해당하는 인덱스를 1로 만듬
                    Debug.Log(furnitureNumList[furnitureCode]);
                }

                furnitureTextList[furnitureCode].text = furnitureNumList[furnitureCode].ToString(); //개수 표시 UI의 값을 없데이트 함
            }
        }*/

        if (furnitureList.Count == 0)
        {
            furnitureList.Add(furnitureCode); //리스트에 저장
            furnitureNumList[furnitureCode] = 1; //가구 코드의 해당하는 인덱스를 1로 만듬
        }
        else
        {
            bool codeExists = false; // 가구 코드가 이미 리스트에 있는지 여부를 확인하는 불 변수
            for (int i = 0; i < furnitureList.Count; i++)
            {
                if (furnitureList[i] == furnitureCode)
                {
                    furnitureNumList[furnitureCode]++; //가구 코드의 해당하는 인덱스에 1을 추가
                    codeExists = true; // 가구 코드가 이미 리스트에 있는 경우 true
                    break;
                }
            }

            // 가구 코드가 리스트에 없는 경우 리스트에 추가하고 인덱스 값을 1로 설정
            if (!codeExists)
            {
                furnitureList.Add(furnitureCode); //리스트에 저장
                furnitureNumList[furnitureCode] = 1; //가구 코드의 해당하는 인덱스를 1로 만듬
            }
        }

        // UI 텍스트 업데이트
        furnitureTextList[furnitureCode].text = furnitureNumList[furnitureCode].ToString();
    }

    IEnumerator UiActivate() //UI 활성화하는 코루틴
    {
        for(int i = 0; i < limit; i++) //가구 최대 갯수까지 계속 돌림
        {
            if(furnitureList.Contains(i)) //가구 UI 인덱스에 해당하는 코드가 있다면
            {
                furnitureUiList[i].SetActive(true); //가구 UI를 활성화
            }
        }

        yield return new WaitForSeconds(1f);
    }
}
