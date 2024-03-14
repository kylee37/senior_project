using UnityEngine;
using System.Linq;

public class ObjectMovement : MonoBehaviour
{
    GameManager gameManager;
    Vector3[] path;
    private int targetIndex;

    public float moveSpeed = 5f; // 이동 속도 변수 추가
    public GameObject[] targetObjects; // 여러 목표 오브젝트를 저장할 배열

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager 객체 참조
        FindPath(); // 경로 찾기
    }

    void FindPath()
    {
        float closestDistance = Mathf.Infinity; // 가장 가까운 거리를 저장할 변수 초기화
        Vector3 closestEndPos = Vector3.zero; // 가장 가까운 목표 위치를 저장할 변수 초기화

        // 현재 위치
        Vector3 startPos = transform.position;

        // 가장 가까운 목표 위치 찾기
        foreach (GameObject targetObject in targetObjects)
        {
            Vector3 endPos = targetObject.transform.position;
            float distance = Vector3.Distance(startPos, endPos);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEndPos = endPos;
            }
        }

        // 가장 가까운 목표 위치에 대한 인덱스 찾기
        int closestTargetIndex = -1;
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i].transform.position == closestEndPos)
            {
                closestTargetIndex = i;
                break;
            }
        }

        // 가장 가까운 목표 위치에 대한 경로 찾기
        gameManager.targetPos = new Vector2Int((int)closestEndPos.x, (int)closestEndPos.y); // 가장 가까운 목표 위치 설정
        gameManager.PathFinding();
        path = new Vector3[gameManager.FinalNodeList.Count];
        for (int i = 0; i < gameManager.FinalNodeList.Count; i++)
        {
            path[i] = new Vector3(gameManager.FinalNodeList[i].x, gameManager.FinalNodeList[i].y, 0);
        }
        targetIndex = 0;

        // 찾은 목표 위치를 리스트에서 제거
        if (closestTargetIndex != -1)
        {
            Debug.Log("Reached target position: " + closestEndPos); // 목표 위치에 도달한 것을 로그로 출력
            // 해당 목표 위치를 배열에서 제거
            targetObjects = targetObjects.Where((source, index) => index != closestTargetIndex).ToArray();
        }

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
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex], Time.deltaTime * moveSpeed);

        // 다음 위치에 도착하면 다음 인덱스로 이동
        if (Vector3.Distance(transform.position, path[targetIndex]) < 0.1f)
            targetIndex++;
    }
}
