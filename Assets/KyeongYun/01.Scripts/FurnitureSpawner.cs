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
        // ���Ӱ� ���õ� ���� ó��
        if (stateManager != null && stateManager.currentState == State.Spawned)
        {
            // �г��� ������ ���¿��� ������Ʈ�� �����ϰ� �̵��� �� �ִ� ����
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
        SetObjectAlpha(spawnedObject, 0.2f); // ��ġ������ �� Alpha���� ����
        buildPanel.SetActive(true); 
    }

    void MoveObject(Vector3 direction)
    {
        // ���� ������ ������Ʈ ��������
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("SpawnedObject");

        if (spawnedObjects.Length > 0)
        {
            GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Length - 1];
            // �̵��� �Ÿ�
            float moveDistance = 1.0f;
            if(Input.GetKey(KeyCode.LeftShift))
            {
                moveDistance = 0.2f;
            }
            // ���� ������Ʈ�� ��ġ�� �̵�
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
            // Ű�� ������ �� ���¸� Normal�� �����ϰ� ���İ��� �ǵ�����
            if (stateManager != null && CollideChecker.isColliding == false)
            {
                Debug.Log("��ġ �Ϸ�");
                stateManager.UpdateState(State.Normal);

                // buttonController�� prefabToSpawn�� ����Ͽ� ���İ��� 1�� �ǵ���
                SetObjectAlpha(spawnedObject, 1.0f);
                buildPanel.SetActive(false);
                //���⼭ ��ġ Ȯ���� ���´ٸ� ������ �ִ� ��ȭ - �ʿ��� ��ȭ
            }
            else if (CollideChecker.isColliding == true)
            {
                Debug.LogError("�̰��� ��ġ�� �� �����ϴ�");
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            if (stateManager != null)
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
