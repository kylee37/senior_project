using UnityEngine;
using System.Linq;

public class ObjectMovement : MonoBehaviour
{
    GameManager gameManager;
    Vector3[] path;
    private int targetIndex;

    public float moveSpeed = 5f; // �̵� �ӵ� ���� �߰�
    public GameObject[] targetObjects; // ���� ��ǥ ������Ʈ�� ������ �迭

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager ��ü ����
        FindPath(); // ��� ã��
    }

    void FindPath()
    {
        float closestDistance = Mathf.Infinity; // ���� ����� �Ÿ��� ������ ���� �ʱ�ȭ
        Vector3 closestEndPos = Vector3.zero; // ���� ����� ��ǥ ��ġ�� ������ ���� �ʱ�ȭ

        // ���� ��ġ
        Vector3 startPos = transform.position;

        // ���� ����� ��ǥ ��ġ ã��
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

        // ���� ����� ��ǥ ��ġ�� ���� �ε��� ã��
        int closestTargetIndex = -1;
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i].transform.position == closestEndPos)
            {
                closestTargetIndex = i;
                break;
            }
        }

        // ���� ����� ��ǥ ��ġ�� ���� ��� ã��
        gameManager.targetPos = new Vector2Int((int)closestEndPos.x, (int)closestEndPos.y); // ���� ����� ��ǥ ��ġ ����
        gameManager.PathFinding();
        path = new Vector3[gameManager.FinalNodeList.Count];
        for (int i = 0; i < gameManager.FinalNodeList.Count; i++)
        {
            path[i] = new Vector3(gameManager.FinalNodeList[i].x, gameManager.FinalNodeList[i].y, 0);
        }
        targetIndex = 0;

        // ã�� ��ǥ ��ġ�� ����Ʈ���� ����
        if (closestTargetIndex != -1)
        {
            Debug.Log("Reached target position: " + closestEndPos); // ��ǥ ��ġ�� ������ ���� �α׷� ���
            // �ش� ��ǥ ��ġ�� �迭���� ����
            targetObjects = targetObjects.Where((source, index) => index != closestTargetIndex).ToArray();
        }

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
