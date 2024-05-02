using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMover : MonoBehaviour
{
    GameManager gameManager;

    PrefabSpawner prefabSpawner;

    Vector3[] path;
    public int targetIndex;
    public GameObject targetObject;             // NPC�� �������� ������ ����

    public PosManager reservationSystem;
    public Vector3Int destinationPosition;

    public string NPCName;                      // �ν����� â�� �ش� NPC ����
    public string badNPC;                       // ���谡 ���� NPC ����
    public string goodFood;                     // ��ȣ�ϴ� ���� ����
    private float moveSpeed = 5f;

    private Animator animator;                  // NPC�� �ִϸ����� ����


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        prefabSpawner = FindObjectOfType<PrefabSpawner>();
        reservationSystem = FindObjectOfType<PosManager>();
        targetObject = gameManager.GetRandomUnusedTargetObject();   // ������ ������ ����
        animator = GetComponent<Animator>();                        // �ִϸ����� ������Ʈ ��������

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
        // TilemapCollider ����
        if (targetObject != null)
        {
            TilemapCollider2D tilemapCollider = targetObject.GetComponent<TilemapCollider2D>();
            if (tilemapCollider != null)
            {
                tilemapCollider.enabled = false;
            }
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
        if (path == null || path.Length == 0 || targetIndex >= path.Length)
            return;

        // ���� ��ġ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex], moveSpeed * Time.deltaTime);

        // ���� ��ġ�� �����ϸ� ���� �ε����� �̵�
        if (Vector3.Distance(transform.position, path[targetIndex]) < 0.1f)
        {
            targetIndex++;
        }

        // �̵� ���⿡ ���� �ִϸ��̼� ����
        if (targetIndex < path.Length)
        {
            Vector3 direction = (path[targetIndex] - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                // �̵� ���⿡ ���� MoveX�� MoveY ���� �����Ͽ� �ִϸ��̼��� ����
                float moveX = direction.x > 0 ? 1 : direction.x < 0 ? -1 : 0;
                float moveY = direction.y > 0 ? 1 : direction.y < 0 ? -1 : 0;

                // MoveX�� MoveY ���� �����Ͽ� �ִϸ��̼��� ����
                animator.SetFloat("MoveX", moveX);
                animator.SetFloat("MoveY", moveY);
            }
        }

        // �������� �������� ���� ����
        if (targetIndex == path.Length)
        {
            OnDestinationReached();
        }
    }
    void OnDestinationReached()
    {
        int randomNum = Random.Range(1, 10);
        Debug.Log("������ ����");

        // NPCName�� �ش��ϴ� rate ���� ��������, ������ NPC �̸��� ���� ��ȣ�� ����
        int currentRate = 0;
        switch (NPCName)
        {
            case "Human": // NPC �̸�
                currentRate = prefabSpawner.humanRate;
                prefabSpawner.UpdateRate("humanRate", currentRate);
                break;
            case "Dwarf":
                currentRate = prefabSpawner.dwarfRate;
                prefabSpawner.UpdateRate("dwarfRate", currentRate);
                break;
            case "Elf":
                currentRate = prefabSpawner.elfRate;
                prefabSpawner.UpdateRate("elfRate", currentRate);
                break;
            default:
                Debug.LogError("Invalid NPCName: " + NPCName);
                break;
        }

        switch (randomNum)
        {
            case int n when (n > 0 && n <= 6):
                Invoke(nameof(GoodEmote), 3);
                currentRate += 2;
                break;
            case int n when (n > 6 && n < 10):
                Invoke(nameof(NormalEmote), 3);
                currentRate += 1;
                break;
            case int n when (n >= 10):
                Invoke(nameof(BadEmote), 3);
                break;
        }

        // ���� �� 1�� �� 2��(�̸�Ƽ�� ��±����� ����)���� ����
        Invoke(nameof(Thinking), 1);

        Invoke(nameof(NPCExit), 10);
    }

    void NPCExit()
    {
        // ���⼭ NPC�� ���� �� ���� ����

        Debug.Log("NPC����");

        // TilemapCollider �ѱ�
        if (targetObject != null)
        {
            TilemapCollider2D tilemapCollider = targetObject.GetComponent<TilemapCollider2D>();
            if (tilemapCollider != null)
            {
                tilemapCollider.enabled = true;
            }
        }
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
        gameManager.acheivement++;
        reservationSystem.CancelReservation(destinationPosition);
    }

    void Thinking()
    {
        Debug.Log("�����...");
    }

    void GoodEmote()
    {
        Debug.Log("��ȣ ���� �ֹ�, ���� �̸�Ƽ�� ���, +2");
    }

    void NormalEmote()
    {
        Debug.Log("�Ϲ� ���� �ֹ�, ����� �̸�Ƽ�� ���, +1");
    }

    void BadEmote()
    {
        Debug.Log("��ȣ ���� �ֹ�, ���� �̸�Ƽ�� ���");
    }

    void Eating()
    {
        Debug.Log("�Ļ���...");
    }

    IEnumerator FindNewDestinationWithRetry(int maxRetryCount)
    {
        int retryCount = 0;
        while (retryCount < maxRetryCount)
        {
            yield return new WaitForSeconds(0.5f); // ��õ� ������ 0.5�ʷ� �����ϰų� �ʿ信 ���� ����

            targetObject = gameManager.GetRandomUnusedTargetObject(); // ������ ������ ����
            if (targetObject != null)
            {
                destinationPosition = new Vector3Int((int)targetObject.transform.position.x, (int)targetObject.transform.position.y, 0);
                if (reservationSystem.ReserveDestination(destinationPosition, gameObject))
                {
                    FindPath();     // ��� ã��
                    yield break;    // �������� ã�����Ƿ� �ݺ��� Ż��
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

        // �ִ� ��õ� Ƚ���� �ʰ��� ��� NPC�� Ǯ�� ��ȯ
        Debug.LogWarning("Failed to find an available destination after " + maxRetryCount + " retries. Destroying NPC.");
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
    }
    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������ NPCName�� Ư�� NPCName�� ������ Ȯ��
        if (other.GetComponent<NPCMover>().NPCName == badNPC)
        {
            // ���⼭ Ư�� NPC�� ���� ��ȣ�ۿ� �ۼ�
            Debug.Log("�̹�Ʈ �߻�");
        }
    }
}