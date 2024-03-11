using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static int limit = 5; //리스트에 등록할 수 있는 최대 갯수
    public List<int> foodList = new List<int>(new int[limit]); //음식 코드를 담는 리스트
    public GameObject addPanel; //메뉴 등록 UI (켜져있을 때만 등록을 실행하기 위해 필요)
    public MenuReset menuReset; //메뉴 리셋 스크립트

    //저장용 리스트
    public List<Image> foodImageList = new List<Image>(); //모든 음식 이미지를 담는 리스트
    [TextArea]
    public List<string> foodTextList = new List<string>(); //모든 음식 설명을 담는 리스트
    //메뉴판 리스트
    public List<Image> manuImageList = new List<Image>(); //메뉴판 이미지를 담는 리스트
    public List<TMP_Text> manuTextList = new List<TMP_Text>(); //메뉴판 설명을 담는 리스트

    private void Awake()
    {
        foodList = new List<int>(new int[limit]);

        for (int i = 0; i < limit; i++)
        {
            foodList[i] = 0;
        }
    }

    public void HandleFoodCode(int foodCode) //음식 코드를 받아 처리하는 메서드
    {
        //리스트에 등록되지 않았고
        if (!foodList.Contains(foodCode))
        {
            // 0인 인덱스가 있다면
            if (foodList.Contains(0))
            {
                // foodList에서 값이 0인 첫 번째 인덱스에 음식 코드 추가
                int indexOfZero = foodList.IndexOf(0);
                if (indexOfZero != -1)
                {
                    foodList[indexOfZero] = foodCode;
                }
            }
        }
    }

    private void Update()
    {
        bool coroutineCheck1 = false; //등록 코루틴이 꺼졌는지 켜졌는지 확인하는 불 변수
        bool coroutineCheck2 = false; //초기화 코루틴이 꺼졌는지 켜졌는지 확인하는 불 변수

        if (addPanel.activeSelf) //addPanel이 활성화 상태고
        {
            if(coroutineCheck1 == false) //코루틴이 꺼져있을 때
            {
                StartCoroutine(AddManu()); //코루틴 호출
                coroutineCheck1 = true; //코루틴 변수 true
            }
        }
        else //활성화가 아닐 때
        {
            if(coroutineCheck1 == true) //코루틴이 켜져있다면
            {
                StopCoroutine(AddManu()); //코루틴 중단
                Debug.Log("코루틴 중단");
                coroutineCheck1 = false; //코루틴 변수 false
            }
        }

        if (menuReset.reset) //만약 초기화 버튼을 누른다면
        {
            if(coroutineCheck2 == false)
            {
                StartCoroutine(ResetMenu()); //코루틴 호출
                Debug.Log("초기화 코루틴 호출");
                coroutineCheck2 = true; //코루틴 변수 true
            }
        }
        else if (!menuReset.reset)
        {
            if(coroutineCheck2 == true)
            {
                StopCoroutine(ResetMenu()); //코루틴 중단
                Debug.Log("초기화 코루틴 중단");
                coroutineCheck2 = false; //코루틴 변수 false
            }
        }
    }

    IEnumerator AddManu() //리스트에 값이 있으면 그 값을 각 메뉴판 오브젝트에 담는 코루틴
    {
        for(int i = 0; i < limit; i++)
        {
            if(foodList[i] != 0) //푸드리스트에 값이 있다면
            {
                manuImageList[i].sprite = foodImageList[foodList[i]].sprite; //메뉴판에 들어온 번호에 해당하는 음식 이미지를 적용
                manuTextList[i].text = foodTextList[foodList[i]]; //메뉴판에 들어온 번호에 해당하는 음식 설명을 적용
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator ResetMenu()
    {
        for(int i = 0; i < limit; i++)
        {
            foodList[i] = 0;
            if (foodList[i] == 0) //값이 없고
            {
                if (foodImageList.Count > 0 && foodTextList.Count > 0) //해당 리스트가 비어있다면
                {
                    manuImageList[i].sprite = foodImageList[0].sprite; //스프라이트 리스트  0번으로 번경
                    manuTextList[i].text = foodTextList[0]; //텍스트 리스트 0번으로 번경
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
    }
}
