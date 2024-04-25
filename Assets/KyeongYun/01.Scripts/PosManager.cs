using System.Collections.Generic;
using UnityEngine;

public class PosManager : MonoBehaviour
{
    private List<Vector3Int> reservedDestinations = new List<Vector3Int>();

    // 목적지를 예약하고 예약이 성공적으로 이루어지면 true를 반환합니다.
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

    // 목적지 예약을 취소합니다.
    public void CancelReservation(Vector3Int position)
    {
        reservedDestinations.Remove(position);
    }

    // 사용 가능한 모든 목적지를 반환합니다.
    public List<Vector3Int> GetAvailableDestinations()
    {
        List<Vector3Int> availableDestinations = new List<Vector3Int>();

        // 여기서 사용 가능한 모든 목적지를 찾아서 리스트에 추가
        // 예를 들어, 맵 전체의 모든 위치를 확인하고 사용되지 않은 위치를 찾을 수 있다.

        return availableDestinations;
    }
}
