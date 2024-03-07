using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewButtonController : MonoBehaviour
{
    [HideInInspector] public StateManager stateManager;
    public GameObject panel;
    public Spawner spawner;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(TogglePanel);
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
