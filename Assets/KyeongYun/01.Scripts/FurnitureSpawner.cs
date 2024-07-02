using UnityEngine;

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
        if (stateManager != null && stateManager.currentState == State.Spawned)
        {
            HandleBuildingInput();
        }
    }

    public void SpawnObject(GameObject prefabToSpawn)
    {
        spawnedObject = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        SetObjectAlpha(spawnedObject, 0.5f); // ��ġ ������ �� Alpha ���� ����
        buildPanel.SetActive(true);
    }

    void MoveObject(Vector3 direction)
    {
        if (spawnedObject != null)
        {
            float moveDistance = Input.GetKey(KeyCode.LeftShift) ? 0.2f : 1.0f;
            spawnedObject.transform.Translate(direction * moveDistance);
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
            if (spawnedObject != null && stateManager != null)
            {
                FurnitureItem furniture = spawnedObject.GetComponent<FurnitureItem>();
                if (furniture != null && furniture.CanPlace())
                {
                    Debug.Log("��ġ �Ϸ�");
                    stateManager.UpdateState(State.Normal);
                    SetObjectAlpha(spawnedObject, 1.0f);
                    buildPanel.SetActive(false);
                }
                else
                {
                    Debug.LogError("�̰��� ��ġ�� �� �����ϴ�");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (spawnedObject != null && stateManager != null)
            {
                Debug.Log("��ġ ��ҵ�");
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
