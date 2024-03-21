using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}


public class GameManager : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;
    public int acheivement = 0;
    public GameObject[] targetObjects; // ���� ��ǥ ������Ʈ�� ������ �迭
    private HashSet<GameObject> usedTargets = new(); // ��� ���� �������� �����ϱ� ���� HashSet


    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public void PathFinding()
    {
        // NodeArray�� ũ�� �����ְ�, isWall, x, y ����
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // ��ǥ ��忡 �����ϸ� Ž�� ����
            if (CurNode == TargetNode)
            {
                TracePath();
                return;
            }

            // �̿� ��带 �˻��Ͽ� ���� ����Ʈ�� �߰�
            ExploreNeighborNodes(CurNode);
        }
    }

    // �̿� ��带 �˻��Ͽ� ���� ����Ʈ�� �߰�
    private void ExploreNeighborNodes(Node currentNode)
    {
        // �֢آע� �밢 ������ ����� ��
        if (allowDiagonal)
        {
            OpenListAdd(currentNode.x + 1, currentNode.y + 1);
            OpenListAdd(currentNode.x - 1, currentNode.y + 1);
            OpenListAdd(currentNode.x - 1, currentNode.y - 1);
            OpenListAdd(currentNode.x + 1, currentNode.y - 1);
        }

        // �� �� �� �� �밢 ������ ���� ��
        OpenListAdd(currentNode.x, currentNode.y + 1);
        OpenListAdd(currentNode.x + 1, currentNode.y);
        OpenListAdd(currentNode.x, currentNode.y - 1);
        OpenListAdd(currentNode.x - 1, currentNode.y);
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // �밢�� ����, �� ���̷� ��� �ȵ�
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // �ڳʸ� �������� ���� ������, �̵� �߿� �������� ��ֹ��� ������ �ȵ�
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14); //(1:1:


            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }
    // ���� ��θ� �����մϴ�.
    private void TracePath()
    {
        Node targetCurNode = TargetNode;
        while (targetCurNode != StartNode)
        {
            FinalNodeList.Add(targetCurNode);
            targetCurNode = targetCurNode.ParentNode;
        }
        FinalNodeList.Add(StartNode);
        FinalNodeList.Reverse();
    }

    void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }

    // ��� ���� �������� ������Ʈ�մϴ�.
    public void UpdateUsedTargets(GameObject[] newTargets)
    {
        usedTargets.Clear();
        foreach (var target in newTargets)
        {
            if (usedTargets.Contains(target))
                continue;

            usedTargets.Add(target);
        }
    }

    public GameObject GetRandomUnusedTargetObject()
    {
        List<GameObject> unusedTargets = new List<GameObject>();
        foreach (var target in targetObjects)
        {
            if (!usedTargets.Contains(target))
            {
                unusedTargets.Add(target);
            }
        }

        if (unusedTargets.Count == 0)
        {
            Debug.LogError("All target objects are already in use.(GameManager)");
            return null;
        }

        int randomIndex = Random.Range(0, unusedTargets.Count);
        return unusedTargets[randomIndex];
    }
}