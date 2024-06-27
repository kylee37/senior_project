using UnityEngine;
using UnityEngine.Tilemaps;

public class FurnitureSpawner : MonoBehaviour
{
    [HideInInspector] public StateManager stateManager;
    [HideInInspector] public GameObject buildPanel;
    private GameObject spawnedObject;
    public Tilemap targetTilemap;  // 타일맵을 할당합니다.
    private Collider2D spawnedCollider;

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
        if (stateManager != null && stateManager.currentState == State.Spawned)
        {
            HandleBuildingInput();
        }
    }

    public void SpawnObject(GameObject prefabToSpawn)
    {
        spawnedObject = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        spawnedCollider = spawnedObject.GetComponent<Collider2D>();
        SetObjectAlpha(spawnedObject, 0.5f); // 배치상태일 때 Alpha값을 줄임
        buildPanel.SetActive(true);
    }

    void MoveObject(Vector3 direction)
    {
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("SpawnedObject");

        if (spawnedObjects.Length > 0)
        {
            GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Length - 1];
            float moveDistance = 1.0f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDistance = 0.2f;
            }
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
            if (stateManager != null)
            {
                if (IsOnTargetTilemap(spawnedObject))
                {
                    Debug.Log("배치 완료");
                    stateManager.UpdateState(State.Normal);
                    SetObjectAlpha(spawnedObject, 1.0f);
                    buildPanel.SetActive(false);
                }
                else
                {
                    Debug.LogError("타일맵 위에 있어야 합니다!");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
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

    bool IsOnTargetTilemap(GameObject obj)
    {
        if (targetTilemap == null || spawnedCollider == null)
        {
            Debug.LogError("Target Tilemap 또는 Collider가 설정되지 않았습니다.");
            return false;
        }

        // 오브젝트의 각 콜라이더 정점들이 타일맵 위에 있는지 검사합니다.
        Bounds bounds = spawnedCollider.bounds;
        Vector3Int min = targetTilemap.WorldToCell(bounds.min);
        Vector3Int max = targetTilemap.WorldToCell(bounds.max);

        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (!targetTilemap.HasTile(tilePosition))
                {
                    return false;
                }
            }
        }
        return true;
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
