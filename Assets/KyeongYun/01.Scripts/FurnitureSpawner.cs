using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class FurnitureSpawner : MonoBehaviour
{
    [HideInInspector] public StateManager stateManager;
    [HideInInspector] public GameObject buildPanel;
    private GameObject spawnedObject;
    void Start()
    {
        buildPanel.SetActive(false);
        stateManager = FindObjectOfType<StateManager>();
        if (stateManager == null)
        {
            Debug.LogError("StateManager not found!");
        }
        else
        {
            Debug.Log("StateManager found: " + stateManager.currentState);
        }
    }

    void Update()
    {
        // 게임과 관련된 로직 처리
        if (stateManager != null && stateManager.currentState == State.Spawned)
        {
            // 패널이 내려간 상태에서 오브젝트를 생성하고 이동할 수 있는 로직
            HandleBuildingInput();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(stateManager != null ? stateManager.currentState.ToString() : "StateManager is null");
        }
    }

    public void SpawnObject(GameObject prefabToSpawn)
    {
        spawnedObject = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        SetObjectAlpha(spawnedObject, 0.2f); // 배치상태일 때 Alpha값을 줄임
        buildPanel.SetActive(true); 
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
            if(Input.GetKey(KeyCode.LeftShift))
            {
                moveDistance = 0.2f;
            }
            // 현재 오브젝트의 위치를 이동
            lastSpawnedObject.transform.Translate(direction * moveDistance);
        }
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
            // 키를 눌렀을 때 상태를 Normal로 변경하고 알파값을 되돌리기
            if (stateManager != null && CollideChecker.isColliding == false)
            {
                Debug.Log("배치 완료");
                stateManager.UpdateState(State.Normal);

                // buttonController의 prefabToSpawn을 사용하여 알파값을 1로 되돌림
                SetObjectAlpha(spawnedObject, 1.0f);
                buildPanel.SetActive(false);
                //여기서 배치 확정을 짓는다면 가지고 있는 재화 - 필요한 재화
            }
            else if (CollideChecker.isColliding == true)
            {
                Debug.LogError("이곳엔 배치할 수 없습니다");
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            if (stateManager != null)
            {
                Debug.Log("배치 취소됨");
                stateManager.UpdateState(State.Normal);

                Destroy(spawnedObject);
                buildPanel.SetActive(false);
            }
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
