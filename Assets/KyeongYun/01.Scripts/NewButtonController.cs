using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewButtonController : MonoBehaviour
{
    [HideInInspector] public StateManager stateManager;
    public GameObject panel;


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
