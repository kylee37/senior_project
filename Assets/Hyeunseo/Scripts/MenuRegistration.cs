using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuRegistration : MonoBehaviour
{
    public int foodCode = 1; //음식 코드
    public GameObject textUi; //표시 UI
    public TMP_Text printText; //표시 UI 텍스트
    public MenuManager menuManager; //메뉴 매니저 스크립트
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        if (menuManager != null) // 만약 menuManager가 null이 아니라면
        {
            if (menuManager.foodList.Contains(0)) //메뉴매니저의 푸드리스트에 0인 자리가 있다면
            {
                if (!menuManager.foodList.Contains(foodCode)) // foodCode가 등록되어 있지 않다면
                {
                    menuManager.HandleFoodCode(foodCode); // foodCode를 MenuManager 스크립트로 전달
                    Debug.Log("값 전달");
                    printText.text = "등록 완료!"; // UI에 해당 문구 표시
                }
                else //등록되어있으면
                {
                    printText.text = "이미 등록 되어있습니다."; // 이미 존재하는 경우 UI에 해당 문구 표시
                }
            }
            else
            {
                printText.text = "메뉴판이 부족합니다."; //표시 UI에 문장 넣기
            }
        }
        else
        {
            printText.text = "메뉴판이 없습니다."; //표시 UI에 문장 넣기
        }
        textUi.SetActive(true); //표시 UI 활성화
        Invoke("CloseUI", 0.5f); //0.5초 뒤 UI 삭제
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
