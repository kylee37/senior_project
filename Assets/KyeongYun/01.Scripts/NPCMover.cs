using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMover : MonoBehaviour
{
    private GameManager gameManager;
    private PrefabSpawner prefabSpawner;
    private Animator animator;
    private GoldManager goldManager;
    private Vector3[] path;
    private int emoteNum;
    private GameObject emoteInstance;

    public int targetIndex;
    public GameObject targetObject;

    public PosManager reservationSystem;
    public Vector3Int destinationPosition;

    public string NPCName;
    public string badNPC;
    public string goodFood;
    public GameObject[] emotes;

    private float moveSpeed = 5f;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        prefabSpawner = FindObjectOfType<PrefabSpawner>();
        reservationSystem = FindObjectOfType<PosManager>();
        goldManager = FindObjectOfType<GoldManager>();
        targetObject = gameManager.GetRandomUnusedTargetObject();
        animator = GetComponent<Animator>();

        // ������ ���� �ý����� �ʱ�ȭ�ϰ� ������ ����
        if (targetObject != null)
        {
            destinationPosition = new Vector3Int((int)targetObject.transform.position.x, (int)targetObject.transform.position.y, 0);
            if (reservationSystem.ReserveDestination(destinationPosition, gameObject))
            {
                FindPath();
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
            case "Human":
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
                emoteNum = 0;
                currentRate += 2;
                break;
            case int n when (n > 6 && n < 10):
                Invoke(nameof(NormalEmote), 3);
                emoteNum = 1;
                currentRate += 1;
                break;
            case int n when (n >= 10):
                Invoke(nameof(BadEmote), 3);
                emoteNum = 2;
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
        goldManager.gold += 100;
        reservationSystem.CancelReservation(destinationPosition);
    }


    void Thinking()
    {
        Vector3 emotePosition = transform.position + new Vector3(0, 1, 0); // NPC ��ġ�� �ణ �� (y������ 1��ŭ ���� �̵�)
        emoteInstance = Instantiate(emotes[3], emotePosition, Quaternion.identity);
        Debug.Log("�����...");
        Invoke(nameof(DestroyEmote), 2f); // 2�� �Ŀ� DestroyEmote �Լ� ȣ��
        StartCoroutine(ExecuteAfterDelay(2f)); // 2(delay)�� �Ŀ� Emotes �Լ� ����
    }

    IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Emotes(emoteNum); // ������ �ð� �Ŀ� Emotes �Լ� ����
    }


    void Emotes(int emoteNum)
    {
        Vector3 emotePosition = transform.position + new Vector3(0, 1, 0); // NPC ��ġ�� �ణ �� (y������ 1��ŭ ���� �̵�)
        emoteInstance = Instantiate(emotes[emoteNum], emotePosition, Quaternion.identity);
        // �̸�Ƽ�� ���� ���� ����
        Debug.Log("�̸�Ƽ�� ���");

        // �̸�Ƽ���� ��µ� �� ���� �ð� �Ŀ� �ı��ǵ��� Invoke�� ����Ͽ� DestroyEmote �Լ� ȣ��
        Invoke(nameof(DestroyEmote), 3f); // 3�� �Ŀ� DestroyEmote �Լ� ȣ��
    }

    void DestroyEmote()
    {
        if (emoteInstance != null)
        {
            Destroy(emoteInstance);
            Debug.Log("�̸�Ƽ�� �ı�");
        }
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

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NPCMover>().NPCName == badNPC)
        {
            Debug.Log("�̹�Ʈ �߻�");
        }
    }

    IEnumerator FindNewDestinationWithRetry(int maxRetryCount)
    {
        int retryCount = 0;
        while (retryCount < maxRetryCount)
        {
            yield return new WaitForSeconds(0.5f);

            targetObject = gameManager.GetRandomUnusedTargetObject();
            if (targetObject != null)
            {
                destinationPosition = new Vector3Int((int)targetObject.transform.position.x, (int)targetObject.transform.position.y, 0);
                if (reservationSystem.ReserveDestination(destinationPosition, gameObject))
                {
                    FindPath();
                    yield break;
                }
                else
                {
                    Debug.Log("Destination already reserved. Retrying...");
                }
            }
            else
            {
                Debug.LogError("No available target object found.");
                yield break;
            }

            retryCount++;
        }

        // �ִ� ��õ� Ƚ���� �ʰ��� ��� NPC�� Ǯ�� ��ȯ
        Debug.LogWarning("Failed to find an available destination after " + maxRetryCount + " retries. Destroying NPC.");
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
    }
}
