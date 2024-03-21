using System.Collections;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    GameManager gameManager;
    PrefabSpawner prefabSpawner;
    Vector3[] path;
    public int targetIndex;
    private float moveSpeed = 5f;
    public GameObject targetObject; // NPC�� �������� ������ ����
    public PosManager reservationSystem;
    public Vector3Int destinationPosition;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        prefabSpawner = FindObjectOfType<PrefabSpawner>();
        reservationSystem = FindObjectOfType<PosManager>();

        targetObject = gameManager.GetRandomUnusedTargetObject(); // ������ ������ ����

        // ������ ���� �ý����� �ʱ�ȭ�ϰ� ������ ����
        if (targetObject != null)
        {
            destinationPosition = new Vector3Int((int)targetObject.transform.position.x, (int)targetObject.transform.position.y, 0);
            if (reservationSystem.ReserveDestination(destinationPosition, gameObject))
            {
                FindPath(); // ��� ã��
            }
            else
            {
                Debug.Log("Destination already reserved. Finding new destination...");
                StartCoroutine(FindNewDestinationWithRetry(gameManager.targetObjects.Length - 1));
                // N ȸ Ž���� ��õ��ϴ� �ڷ�ƾ
            }
        }
        else
        {
            Debug.LogError("No available target object found.");
        }
    }


    void FindPath()
    {
        if (targetObject == null) return;

        // Vector3 startPos = transform.position;
        Vector3 endPos = targetObject.transform.position;

        // A* �˰����� ����Ͽ� ��� ã��
        gameManager.targetPos = new Vector2Int((int)endPos.x, (int)endPos.y);
        gameManager.PathFinding();
        path = new Vector3[gameManager.FinalNodeList.Count];
        for (int i = 0; i < gameManager.FinalNodeList.Count; i++)
        {
            path[i] = new Vector3(gameManager.FinalNodeList[i].x, gameManager.FinalNodeList[i].y, 0);
        }
        targetIndex = 0;

        // �̵� ��ΰ� �����Ǿ����Ƿ� �̵� ����
        MoveToNextTarget();
    }
    void MoveToNextTarget()
    {
        if (path == null || path.Length == 0)
            return;

        // ù ��° ��ġ�� �̵�
        transform.position = path[0];
        targetIndex = 1;
    }

    void Update()
    {
        if (path == null || targetIndex >= path.Length)
            return;

        // ���� ��ġ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex], moveSpeed * Time.deltaTime);

        // ���� ��ġ�� �����ϸ� ���� �ε����� �̵�
        if (Vector3.Distance(transform.position, path[targetIndex]) < 0.1f)
        {
            // �������� �������� ���� ����
            if (targetIndex == path.Length - 1)
            {
                OnDestinationReached();
            }

            targetIndex++;
        }
    }
    void OnDestinationReached()
    {
        int randomNum = Random.Range(1, 10);
        Debug.Log("������ ����");
        switch (randomNum)
        {
            case int n when (n > 0 && n <= 6):
                Debug.Log("��ȣ�ϴ� ���� �ֹ�, ��ȣ�� + 2");
                prefabSpawner.rate1 += 2;
                break;
            case int n when (n > 6 && n < 10):
                Debug.Log("�ƹ��͵� ���� ���� �ֹ�, ��ȣ�� + 1");
                prefabSpawner.rate1 += 1;
                break;
            case int n when (n >= 10):
                Debug.Log("��ȣ�ϴ� ���� �ֹ�");
                break;
        }
        Invoke("NPCExit", 3);
    }
    void NPCExit()
    {
        // ���⼭ NPC�� ���� �� ���� ����.
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
        gameManager.acheivement++;
        reservationSystem.CancelReservation(destinationPosition);
    }
    IEnumerator FindNewDestinationWithRetry(int maxRetryCount)
    {
        int retryCount = 0;
        while (retryCount < maxRetryCount)
        {
            yield return new WaitForSeconds(0.5f); // ��õ� ������ 1�ʷ� �����ϰų� �ʿ信 ���� ����

            targetObject = gameManager.GetRandomUnusedTargetObject(); // ������ ������ ����
            if (targetObject != null)
            {
                destinationPosition = new Vector3Int((int)targetObject.transform.position.x, (int)targetObject.transform.position.y, 0);
                if (reservationSystem.ReserveDestination(destinationPosition, gameObject))
                {
                    FindPath(); // ��� ã��
                    yield break; // �������� ã�����Ƿ� �ݺ��� Ż��
                }
                else
                {
                    Debug.Log("Destination already reserved. Retrying...");
                }
            }
            else
            {
                Debug.LogError("No available target object found.");
                yield break; // ��� ������ �������� �����Ƿ� �ݺ��� Ż��
            }

            retryCount++;
        }

        // �ִ� ��õ� Ƚ���� �ʰ��� ��� NPC�� �ı��մϴ�.
        Debug.LogWarning("Failed to find an available destination after " + maxRetryCount + " retries. Destroying NPC."); 
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
    }
}