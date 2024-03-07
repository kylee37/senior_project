using UnityEngine;

public class GameController : MonoBehaviour
{
    public StateManager stateManager;
    public GameObject objectToSpawn;
    public GameObject panel;
    private GameObject spawnedObject;

    void Update()
    {
        // ���Ӱ� ���õ� ���� ó��
        if (stateManager.currentState == State.Spawned)
        {
            // �г��� ������ ���¿��� ������Ʈ�� �����ϰ� �̵��� �� �ִ� ����
            HandleBuildingInput();
        }
    }

    public void TogglePanel()
    {
        if (stateManager.currentState == State.Normal)
        {
            // �г��� ������ ���¸� Spawned���� ����
            panel.SetActive(false);
            stateManager.UpdateState(State.Spawned);

            // �� �κп��� � Prefab�� ����Ͽ� ������Ʈ�� �������� ����
            // ��: objectToSpawn�� GameController���� �̸� ������ Prefab
            SpawnObject(objectToSpawn);
        }
        else if (stateManager.currentState == State.Spawned)
        {
            // �г��� �ø��� ���¸� Normal�� ����
            panel.SetActive(true);
            stateManager.UpdateState(State.Normal);
        }
    }

    // �����ε�� SpawnObject �޼���
    public void SpawnObject(GameObject prefabToSpawn)
    {
        // �г��� ������ ���¿����� ������Ʈ ����
        spawnedObject = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        SetObjectAlpha(spawnedObject, 0.5f);
        Building();
    }

    void Building()
    {
        Debug.Log("��ġ ��");
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
            // Space Ű�� ������ �� ���¸� Normal�� �����ϰ� ���İ��� �ǵ�����
            Debug.Log("��ġ ��");
            stateManager.UpdateState(State.Normal);
            SetObjectAlpha(spawnedObject, 1.0f); // ���İ��� 1�� �ǵ���
        }
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
            // ���� ������Ʈ�� ��ġ�� �̵�
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
