using UnityEngine;

public class GameController : MonoBehaviour
{
    public StateManager stateManager;
    public GameObject objectToSpawn;
    public GameObject panel;
    private GameObject spawnedObject;

    void Update()
    {
        // 게임과 관련된 로직 처리
        if (stateManager.currentState == State.Spawned)
        {
            // 패널이 내려간 상태에서 오브젝트를 생성하고 이동할 수 있는 로직
            HandleBuildingInput();
        }
    }

    public void TogglePanel()
    {
        if (stateManager.currentState == State.Normal)
        {
            // 패널을 내리고 상태를 Spawned으로 변경
            panel.SetActive(false);
            stateManager.UpdateState(State.Spawned);

            // 이 부분에서 어떤 Prefab을 사용하여 오브젝트를 생성할지 전달
            // 예: objectToSpawn는 GameController에서 미리 지정한 Prefab
            SpawnObject(objectToSpawn);
        }
        else if (stateManager.currentState == State.Spawned)
        {
            // 패널을 올리고 상태를 Normal로 변경
            panel.SetActive(true);
            stateManager.UpdateState(State.Normal);
        }
    }

    // 오버로드된 SpawnObject 메서드
    public void SpawnObject(GameObject prefabToSpawn)
    {
        // 패널이 내려간 상태에서만 오브젝트 생성
        spawnedObject = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        SetObjectAlpha(spawnedObject, 0.5f);
        Building();
    }

    void Building()
    {
        Debug.Log("배치 중");
    }

    public void HandleBuildingInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveObject(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveObject(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveObject(Vector3.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveObject(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // Space 키를 눌렀을 때 상태를 Normal로 변경하고 알파값을 되돌리기
            Debug.Log("배치 完");
            stateManager.UpdateState(State.Normal);
            SetObjectAlpha(spawnedObject, 1.0f); // 알파값을 1로 되돌림
        }
    }

    void MoveObject(Vector3 direction)
    {
        // 현재 생성된 오브젝트 가져오기
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("SpawnedObject");

        if (spawnedObjects.Length > 0)
        {
            GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Length - 1];
            // 이동할 거리
            float moveDistance = 1.0f;
            // 현재 오브젝트의 위치를 이동
            lastSpawnedObject.transform.Translate(direction * moveDistance);
        }
    }

    void SetObjectAlpha(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            Color color = material.color;
            color.a = alpha;
            material.color = color;
        }
    }
}
