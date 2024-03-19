using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    int level = 0; //주점 레벨
    float exp = 0; //보유 경험치
    public TMP_Text levelText; //레벨 표시 UI

    public List<float> levelList = new List<float>(); //주점 레벨업에 필요한 경험치 데이터 리스트
    public List<int> levelXpList = new List<int>(); //주점 레벨업에 따른 xp 지급량 데이터 리스트

    public GameObject textUi; //표시 UI
    public TMP_Text printText; //표시 UI 텍스트

    public int totalXP = 0; //XP를 전달할 변수

    void Update()
    {
        levelText.text = level.ToString() + ".Lv";

        if (exp >= levelList[level]) //보유 경험치가 요구 경험치보다 많거나 같을 때
        {
            totalXP = levelXpList[level]; //토탈스코어에 xp 증가 값 적용

            level++; //레벨업
            exp = 0; //보유 경험치 초기화

            if (level > 1)
            {
                printText.text = "레벨업!"; //표시 UI에 문장 넣기

                textUi.SetActive(true); //표시 UI 활성화
                Invoke("CloseUI", 0.5f); //0.5초 뒤 UI 삭제
            }
        }
    }

    void CloseUI()
    {
        // UI를 비활성화
        if (textUi != null)
        {
            textUi.SetActive(false);
        }
    }
}
