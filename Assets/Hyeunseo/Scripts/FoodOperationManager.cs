using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodOperationManager : MonoBehaviour //코드가 더러워지는 것을 방지하기 위해 추후 가구 등록 시스템이 생길 경우 텍스트를 삭제하고 관리용 매니저를 만들어서 기본 선호도 += 음식 선호도 + 가구 선호도로 만들 예정
{
    //각 종족별 선호도를 저장할 변수
    private int sumHuman = 0;
    private int sumElf = 0;
    private int sumDwarf = 0;

    public TMP_Text operText; //주점 관리창의 텍스트

    public MenuManager menuManager; //메뉴리스트 확인을 위해 메뉴 매니저를 가져옴
    public MenuReset menuReset; //메뉴 리셋 확인을 위해 메뉴 리셋을 가져옴

    public FoodSO foodSO; // FoodSO 참조

    HashSet<int> processedFoodCodes = new HashSet<int>(); // 이미 처리한 음식 코드를 저장하는 HashSet

    bool resetCheck = false; //리셋 상태를 확인해주는 변수

    void Update()
    {
        StartCoroutine(UpdateOper()); //가구와 음식의 선호도를 종족 선호도에 반영하는 코루틴 실행

        operText.text = "인간 선호도 - " + sumHuman + "\n" + "엘프 선호도 - " + sumElf + "\n" + "드워프 선호도 - " + sumDwarf;

        if (menuReset.reset && resetCheck == false) //만약 메뉴 리셋 버튼이 눌리고, 리셋 체크가 flase일 경우
        {
            StartCoroutine(FoodCountReset()); //음식으로 추가된 점수 삭제하는 코루틴 호출
            resetCheck = true; //리셋 체크 true로 변경
        }
        else if (!menuReset.reset) //메뉴 리셋 버튼이 초기화되면
        {
            StopCoroutine(FoodCountReset()); //코루틴을 중단
            resetCheck = false; //리셋 체크 false로 변경
        }
    }

    IEnumerator UpdateOper()
    {
        for (int i = 0; i < menuManager.foodList.Count; i++)
        {
            int foodCode = menuManager.foodList[i]; //foodCode에 foodList[i]값 할당

            if (processedFoodCodes.Contains(foodCode)) //이미 처리한 음식 코드는 더이상 처리하지 않음
            {
                continue;
            }

            for (int j = 0; j < foodSO.foods.Length; j++)
            {
                Food food = foodSO.foods[j]; //food에 foods[j] 값 할당

                if (food.num == foodCode) //food.num이 foodCode와 같으면
                {
                    // 해당 음식의 humen, elf, dwerf 값을 더함
                    sumHuman += food.human;
                    sumElf += food.elf;
                    sumDwarf += food.dwarf;

                    processedFoodCodes.Add(foodCode); //이미 처리한 코드로 표시

                    break; //탈출
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
