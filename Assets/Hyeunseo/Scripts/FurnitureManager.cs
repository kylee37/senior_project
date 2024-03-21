using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FurnitureManager : MonoBehaviour
{
    List<int> furnitureList = new List<int>(); //받은 가구코드를 저장하는 리스트
    List<int> furnitureNumList = new List<int>(); //가구 갯수를 저장하는 리스트

    public List<GameObject> furnitureUiList = new List<GameObject>(); //UI 저장용 리스트
    public List<TMP_Text> furnitureTextList = new List<TMP_Text>(); //개수 표시 UI용 리스트

    public int limit = 8; //가구 종류

    void Start()
    {
        
    }

    void Update()
    {
        furnitureList.Sort(); //리스트 정렬

        StartCoroutine(UiActivate()); //UI 활성화 코루틴 실행
    }

    public void HandleFurnitureCode(int furnitureCode) //가구코드를 전달받는 메서드
    {
        furnitureList.Add(furnitureCode); //리스트에 저장
        furnitureNumList[furnitureCode] += 1; //가구 코드의 해당하는 인덱스에 1을 추가
    }

    IEnumerator UiActivate() //UI 활성화하는 코루틴
    {
        for(int i = 0; i <= limit; i++) //가구 최대 갯수까지 계속 돌림
        {
            if(furnitureList.Contains(i)) //가구 UI 인덱스에 해당하는 코드가 있다면
            {
                furnitureUiList[i].SetActive(true); //가구 UI를 활성화
            }
        }

        yield return new WaitForSeconds(1f);
    }
}
