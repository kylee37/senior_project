using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button myButton;
    public GameController gameController;
    public GameObject prefabToSpawn; // 각 버튼에 해당하는 Prefab을 Inspector에서 지정

    void Start()
    {
        myButton.onClick.AddListener(SpawnObject);
    }

    void SpawnObject()
    {
        // 해당 버튼에 지정된 Prefab을 사용하여 GameController의 SpawnObject() 함수 호출
        gameController.SpawnObject(prefabToSpawn);
    }
}
