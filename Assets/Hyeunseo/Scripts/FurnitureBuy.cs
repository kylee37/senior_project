using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FurnitureBuy : MonoBehaviour
{
    public int furnitureCode; //가구코드
    public float price; //가격
    float gold; //보유 골드
    public GameObject textUi; //표시 UI
    public TMP_Text printText; //표시 UI 텍스트
    public GoldManager goldManager; //골드매니저 스크립트
    public FurnitureManager furnitureManager; //가구매니저 스크립트

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    void Update()
    {
        gold = goldManager.gold; //골드매니저에서 골드 저장하는 변수 가져옴
    }

    void OnClick()
    {
        if (price <= gold) //가격보다 보유골드가 크거나 같으면
        {
            goldManager.gold -= price; //보유 골드에서 가격만큼의 골드를 뺌
            furnitureManager.HandleFurnitureCode(furnitureCode); //가구 매니저 스크립트로 값 전달

            printText.text = "구매완료!"; //표시 UI에 문장 넣기
        }
        else if (price > gold) //가격보다 보유골드가 적으면
        {
            printText.text = "골드가 부족합니다."; //표시 UI에 문장 넣기
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
