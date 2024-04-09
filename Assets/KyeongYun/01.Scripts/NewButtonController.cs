using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewButtonController : MonoBehaviour
{
    // [HideInInspector] 
    public StateManager stateManager;
    [HideInInspector] public Spawner spawner;
    public GameObject panel;

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            Debug.Log("button");
            Debug.Log(stateManager.currentState);
            button.onClick.AddListener(TogglePanel);
        }
        else
        {
            Debug.Log("button is null");
        }
    }
    public void TogglePanel()
    {
       
        if (stateManager.currentState == State.Normal)
        {
            Debug.Log("배치 중");
            panel.SetActive(false);
            // 패널을 내리고 상태를 Spawned으로 변경
            stateManager.UpdateState(State.Spawned);
        }
    }
}
