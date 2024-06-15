using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUIManager : MonoBehaviour
{
    public Button[] buttons; // 버튼 UI 리스트

    private void Start()
    {
        // 모든 버튼의 상태를 초기화
        ResetButtonStates();

        // 각 버튼의 클릭 이벤트에 OnButtonClick 함수 연결
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 클로저 사용을 위해 index를 지역변수에 저장
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void OnButtonClick(int index)
    {
        // 클릭된 버튼을 제외한 다른 버튼들의 상태 변경
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i != index)
            {
                buttons[i].image.color = new Color(0.85f, 0.85f, 0.85f, 1f); // 어둡게 변경
            }
            else
            {
                buttons[i].image.color = new Color(1f, 1f, 1f, 1f); // 클릭된 버튼은 원래 색상으로 유지
            }
        }
    }

    private void ResetButtonStates()
    {
        // 모든 버튼의 색상을 원래대로 초기화
        foreach (Button button in buttons)
        {
            button.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
