using System.Collections;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    GameManager gameManager;
    PrefabSpawner prefabSpawner;
    Vector3[] path;
    public int targetIndex;
    private float moveSpeed = 5f;
    public GameObject targetObject; // NPC의 목적지를 저장할 변수
    public PosManager reservationSystem;
    public Vector3Int destinationPosition;
    public string NPCName;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        prefabSpawner = FindObjectOfType<PrefabSpawner>();
        reservationSystem = FindObjectOfType<PosManager>();
        targetObject = gameManager.GetRandomUnusedTargetObject(); // 랜덤한 목적지 설정

        // 목적지 예약 시스템을 초기화하고 목적지 예약
        if (targetObject != null)
        {
            destinationPosition = new Vector3Int((int)targetObject.transform.position.x, (int)targetObject.transform.position.y, 0);
            if (reservationSystem.ReserveDestination(destinationPosition, gameObject))
            {
                FindPath(); // 경로 찾기
            }
            else
            {
                Debug.Log("Destination already reserved. Finding new destination...");
                StartCoroutine(FindNewDestinationWithRetry(gameManager.targetObjects.Length));
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
        if (path == null || targetIndex >= path.Length)
            return;

        // 다음 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex], moveSpeed * Time.deltaTime);

        // 다음 위치에 도착하면 다음 인덱스로 이동
        if (Vector3.Distance(transform.position, path[targetIndex]) < 0.1f)
        {
            // 목적지에 도착했을 때의 동작
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
        Debug.Log("목적지 도착");

        // NPCName에 해당하는 rate 변수 가져오기
        int currentRate = 0;
        switch (NPCName)
        {
            case "rate1":
                currentRate = prefabSpawner.rate1;
                break;
            case "rate2":
                currentRate = prefabSpawner.rate2;
                break;
            case "rate3":
                currentRate = prefabSpawner.rate3;
                break;
            default:
                Debug.LogError("Invalid NPCName: " + NPCName);
                break;
        }

        switch (randomNum)
        {
            case int n when (n > 0 && n <= 6):
                Debug.Log("선호하는 음식 주문, 선호도 + 2");
                currentRate += 2;
                break;
            case int n when (n > 6 && n < 10):
                Debug.Log("아무것도 없는 음식 주문, 선호도 + 1");
                currentRate += 1;
                break;
            case int n when (n >= 10):
                Debug.Log("불호하는 음식 주문");
                break;
        }

        // 해당 NPC에 대한 rate 변수 업데이트
        switch (NPCName)
        {
            case "rate1":
                prefabSpawner.UpdateRate("rate1", currentRate);
                break;
            case "rate2":
                prefabSpawner.UpdateRate("rate2", currentRate);
                break;
            case "rate3":
                prefabSpawner.UpdateRate("rate3", currentRate);
                break;
            default:
                Debug.LogError("Invalid NPCName: " + NPCName);
                break;
        }

        Invoke("NPCExit", 3);
    }
    void NPCExit()
    {
        // 여기서 NPC가 떠날 때 로직 구현.
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
        gameManager.acheivement++;
        reservationSystem.CancelReservation(destinationPosition);
    }
    IEnumerator FindNewDestinationWithRetry(int maxRetryCount)
    {
        int retryCount = 0;
        while (retryCount < maxRetryCount)
        {
            yield return new WaitForSeconds(0.5f); // 재시도 간격을 0.5초로 설정하거나 필요에 따라 조절

            targetObject = gameManager.GetRandomUnusedTargetObject(); // 랜덤한 목적지 설정
            if (targetObject != null)
            {
                destinationPosition = new Vector3Int((int)targetObject.transform.position.x, (int)targetObject.transform.position.y, 0);
                if (reservationSystem.ReserveDestination(destinationPosition, gameObject))
                {
                    FindPath(); // 경로 찾기
                    yield break; // 목적지를 찾았으므로 반복문 탈출
                }
                else
                {
                    Debug.Log("Destination already reserved. Retrying...");
                }
            }
            else
            {
                Debug.LogError("No available target object found.");
                yield break; // 사용 가능한 목적지가 없으므로 반복문 탈출
            }

            retryCount++;
        }

        // 최대 재시도 횟수를 초과한 경우 NPC를 파괴합니다.
        Debug.LogWarning("Failed to find an available destination after " + maxRetryCount + " retries. Destroying NPC.");
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
    }
}