using System.Collections.Generic;
using UnityEngine;

public class PosManager : MonoBehaviour
{
    private List<Vector3Int> reservedDestinations = new List<Vector3Int>();

    // �������� �����ϰ� ������ ���������� �̷������ true�� ��ȯ�մϴ�.
    public bool ReserveDestination(Vector3Int position, GameObject requester)
    {
        if (reservedDestinations.Contains(position))
        {
            Debug.Log("Destination already reserved.");
            return false;
        }

        reservedDestinations.Add(position);
        return true;
    }

    // ������ ������ ����մϴ�.
    public void CancelReservation(Vector3Int position)
    {
        reservedDestinations.Remove(position);
    }

    // ��� ������ ��� �������� ��ȯ�մϴ�.
    public List<Vector3Int> GetAvailableDestinations()
    {
        List<Vector3Int> availableDestinations = new List<Vector3Int>();

        // ���⼭ ��� ������ ��� �������� ã�Ƽ� ����Ʈ�� �߰�
        // ���� ���, �� ��ü�� ��� ��ġ�� Ȯ���ϰ� ������ ���� ��ġ�� ã�� �� �ִ�.

        return availableDestinations;
    }
}
