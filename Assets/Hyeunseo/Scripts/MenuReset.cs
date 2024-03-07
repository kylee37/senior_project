using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuReset : MonoBehaviour
{
    public GameObject textUi; //표시 UI
    public TMP_Text printText; //표시 UI 텍스트
    public bool reset = false; //메뉴 매니저로 전달할 불 변수 

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        reset = true;
        printText.text = "초기화 완료"; //표시 UI에 문장 넣기


        textUi.SetActive(true); //표시 UI 활성화
        Invoke("CloseUI", 0.5f); //0.5초 뒤 UI 삭제, reset 변수 false로 변경
    }
    void CloseUI()
    {
        reset = false;
        // UI를 비활성화
        if (textUi != null)
        {
            textUi.SetActive(false);
        }
    }
}
