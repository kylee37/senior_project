using UnityEngine;

public class NPCMover : MonoBehaviour
{
    GameManager gameManager;
    Vector3[] path;
    private int targetIndex;
    private float moveSpeed = 5f;
    private GameObject targetObject; // NPC�� �������� ������ ����

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager ��ü ����
        targetObject = gameManager.GetRandomTargetObject(); // ������ ������ ����
        FindPath(); // ��� ã��
    }

    void FindPath()
    {
        if (targetObject == null) return;

        Vector3 startPos = transform.position;
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
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex], Time.deltaTime * moveSpeed);

        // ���� ��ġ�� �����ϸ� ���� �ε����� �̵�
        if (Vector3.Distance(transform.position, path[targetIndex]) < 0.1f)
            targetIndex++;
    }
}
