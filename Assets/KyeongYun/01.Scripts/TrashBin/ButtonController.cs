using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button myButton;
    public GameController gameController;
    public GameObject prefabToSpawn; // �� ��ư�� �ش��ϴ� Prefab�� Inspector���� ����

    void Start()
    {
        myButton.onClick.AddListener(SpawnObject);
    }

    void SpawnObject()
    {
        // �ش� ��ư�� ������ Prefab�� ����Ͽ� GameController�� SpawnObject() �Լ� ȣ��
        gameController.SpawnObject(prefabToSpawn);
    }
}
