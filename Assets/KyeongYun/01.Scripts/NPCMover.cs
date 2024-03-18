using UnityEngine;

public class NPCMover : MonoBehaviour
{
    GameManager gameManager;
    Vector3[] path;
    private int targetIndex;
    private float moveSpeed = 5f;
    private GameObject targetObject; // NPC의 목적지를 저장할 변수

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager 객체 참조
        targetObject = gameManager.GetRandomTargetObject(); // 랜덤한 목적지 설정
        FindPath(); // 경로 찾기
    }

    void FindPath()
    {
        if (targetObject == null) return;

        Vector3 startPos = transform.position;
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
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex], Time.deltaTime * moveSpeed);

        // 다음 위치에 도착하면 다음 인덱스로 이동
        if (Vector3.Distance(transform.position, path[targetIndex]) < 0.1f)
            targetIndex++;
    }
}
