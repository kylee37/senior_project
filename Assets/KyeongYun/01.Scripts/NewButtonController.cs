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
            Debug.Log("��ġ ��");
            panel.SetActive(false);
            // �г��� ������ ���¸� Spawned���� ����
            stateManager.UpdateState(State.Spawned);
        }
    }
}
