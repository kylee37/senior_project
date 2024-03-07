using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeResearch : MonoBehaviour
{
    public int recipeCode; //레시피 코드
    public float price; //가격
    float xp; //보유 xp
    public GameObject textUi; //표시 UI
    public TMP_Text printText; //표시 UI 텍스트
    public XPManager xpManager; //xp매니저 스크립트

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
        xp = xpManager.xp; //xp매니저에서 xp 저장하는 변수 가져옴
    }

    void OnClick()
    {
        if (price <= xp) //필요xp보다 xp가 크거나 같으면
        {
            xpManager.xp -= price; //보유 xp에서 필요xp만큼의 xp를 뺌

            printText.text = "레시피 연구 성공!"; //표시 UI에 문장 넣기
        }
        else if (price > xp) //필요xp보다 xp가 적으면
        {
            printText.text = "영감이 부족합니다."; //표시 UI에 문장 넣기
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
