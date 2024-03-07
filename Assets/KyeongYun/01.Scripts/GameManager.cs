using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float tribe1perference = 0;
    float tribe2perference = 0;

    //void update()
    //{
    //    if(종족1 선호도가 오르는 가구를 구매 / 배치 하였을 때 || 종족1 선호도가 오르는 어떠한 행동을 하였을 때)      
    //    {
    //        tribe1perference++;
    //    }
    //}

    public void BuildStruction()
    {

    }

    public void NextDay()
    {
        Debug.Log("운영 시작");
        Debug.Log("다음 운영일로");
        // 주점 여건에 따라 다양한 보상을 받는다.
        // 운영 결산 후 다음 운영일로 넘어간다.
        // 다음 운영일로 넘어간 현재 운영일 표시
        // 각 가구 보너스별로 얼마의 재화가 모였는가를 표시
        // 확인 후 넘어가는 버튼(혹은 화면 클릭하여 넘어가기)
        // 특정 조건이 달성된 것이 확인된다면 이벤트 띄우기
        // 방문한 종족의 수만큼 비례하여 가게 내의 종족 비율 조정 및 배치
    }
}
