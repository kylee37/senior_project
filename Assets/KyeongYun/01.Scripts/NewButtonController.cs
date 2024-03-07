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
            Debug.Log("��ġ ��");
            panel.SetActive(false);
            // �г��� ������ ���¸� Spawned���� ����
            stateManager.UpdateState(State.Spawned);
        }
    }
}
