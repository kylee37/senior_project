using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}


public class GameManager : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;
    public int acheivement = 0;
    public GameObject[] targetObjects; // 여러 목표 오브젝트를 저장할 배열
    private HashSet<GameObject> usedTargets = new(); // 사용 중인 목적지를 추적하기 위한 HashSet


    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public void PathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
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

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 목표 노드에 도달하면 탐색 중지
            if (CurNode == TargetNode)
            {
                TracePath();
                return;
            }

            // 이웃 노드를 검사하여 열린 리스트에 추가
            ExploreNeighborNodes(CurNode);
        }
    }

    // 이웃 노드를 검사하여 열린 리스트에 추가
    private void ExploreNeighborNodes(Node currentNode)
    {
        // ↗↖↙↘ 대각 움직임 허용할 시
        if (allowDiagonal)
        {
            OpenListAdd(currentNode.x + 1, currentNode.y + 1);
            OpenListAdd(currentNode.x - 1, currentNode.y + 1);
            OpenListAdd(currentNode.x - 1, currentNode.y - 1);
            OpenListAdd(currentNode.x + 1, currentNode.y - 1);
        }

        // ↑ → ↓ ← 대각 움직임 막을 시
        OpenListAdd(currentNode.x, currentNode.y + 1);
        OpenListAdd(currentNode.x + 1, currentNode.y);
        OpenListAdd(currentNode.x, currentNode.y - 1);
        OpenListAdd(currentNode.x - 1, currentNode.y);
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14); //(1:1:


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }
    // 최종 경로를 추적합니다.
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

    // 사용 중인 목적지를 업데이트합니다.
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