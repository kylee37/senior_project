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

        // 목적지 예약 시스템을 초기화하고 목적지 예약
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
                // N 회 탐색을 재시도하는 코루틴
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

        // A* 알고리즘을 사용하여 경로 찾기
        gameManager.targetPos = new Vector2Int((int)endPos.x, (int)endPos.y);
        gameManager.PathFinding();
        path = new Vector3[gameManager.FinalNodeList.Count];
        for (int i = 0; i < gameManager.FinalNodeList.Count; i++)
        {
            path[i] = new Vector3(gameManager.FinalNodeList[i].x, gameManager.FinalNodeList[i].y, 0);
        }
        targetIndex = 0;

        // 이동 경로가 생성되었으므로 이동 시작
        MoveToNextTarget();
    }

    void MoveToNextTarget()
    {
        if (path == null || path.Length == 0)
            return;

        // 첫 번째 위치로 이동
        transform.position = path[0];
        targetIndex = 1;
    }

    void Update()
    {
        if (path == null || path.Length == 0 || targetIndex >= path.Length)
            return;

        // 다음 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex], moveSpeed * Time.deltaTime);

        // 다음 위치에 도착하면 다음 인덱스로 이동
        if (Vector3.Distance(transform.position, path[targetIndex]) < 0.1f)
        {
            targetIndex++;
        }

        // 이동 방향에 따라 애니메이션 변경
        if (targetIndex < path.Length)
        {
            Vector3 direction = (path[targetIndex] - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                // 이동 방향에 따라 MoveX와 MoveY 값을 설정하여 애니메이션을 제어
                float moveX = direction.x > 0 ? 1 : direction.x < 0 ? -1 : 0;
                float moveY = direction.y > 0 ? 1 : direction.y < 0 ? -1 : 0;

                // MoveX와 MoveY 값을 설정하여 애니메이션을 제어
                animator.SetFloat("MoveX", moveX);
                animator.SetFloat("MoveY", moveY);
            }
        }

        // 목적지에 도착했을 때의 동작
        if (targetIndex == path.Length)
        {
            OnDestinationReached();
        }
    }

    void OnDestinationReached()
    {
        int randomNum = Random.Range(1, 10);
        Debug.Log("목적지 도착");

        // NPCName에 해당하는 rate 변수 가져오기, 도착한 NPC 이름에 따라 선호도 증가
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

        // 도착 후 1초 후 2초(이모티콘 출력까지의 간격)동안 생각
        Invoke(nameof(Thinking), 1);

        Invoke(nameof(NPCExit), 10);
    }

    void NPCExit()
    {
        // 여기서 NPC가 떠날 때 로직 구현

        Debug.Log("NPC떠남");

        // TilemapCollider 켜기
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
        Vector3 emotePosition = transform.position + new Vector3(0, 1, 0); // NPC 위치의 약간 위 (y축으로 1만큼 위로 이동)
        emoteInstance = Instantiate(emotes[3], emotePosition, Quaternion.identity);
        Debug.Log("고민중...");
        Invoke(nameof(DestroyEmote), 2f); // 2초 후에 DestroyEmote 함수 호출
        StartCoroutine(ExecuteAfterDelay(2f)); // 2(delay)초 후에 Emotes 함수 실행
    }

    IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Emotes(emoteNum); // 지정된 시간 후에 Emotes 함수 실행
    }


    void Emotes(int emoteNum)
    {
        Vector3 emotePosition = transform.position + new Vector3(0, 1, 0); // NPC 위치의 약간 위 (y축으로 1만큼 위로 이동)
        emoteInstance = Instantiate(emotes[emoteNum], emotePosition, Quaternion.identity);
        // 이모티콘 관련 동작 실행
        Debug.Log("이모티콘 출력");

        // 이모티콘이 출력된 후 일정 시간 후에 파괴되도록 Invoke를 사용하여 DestroyEmote 함수 호출
        Invoke(nameof(DestroyEmote), 3f); // 3초 후에 DestroyEmote 함수 호출
    }

    void DestroyEmote()
    {
        if (emoteInstance != null)
        {
            Destroy(emoteInstance);
            Debug.Log("이모티콘 파괴");
        }
    }

    void GoodEmote()
    {
        Debug.Log("선호 음식 주문, 좋은 이모티콘 출력, +2");
    }

    void NormalEmote()
    {
        Debug.Log("일반 음식 주문, 평범한 이모티콘 출력, +1");
    }

    void BadEmote()
    {
        Debug.Log("불호 음식 주문, 나쁜 이모티콘 출력");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NPCMover>().NPCName == badNPC)
        {
            Debug.Log("이번트 발생");
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

        // 최대 재시도 횟수를 초과한 경우 NPC를 풀에 반환
        Debug.LogWarning("Failed to find an available destination after " + maxRetryCount + " retries. Destroying NPC.");
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
    }
}
