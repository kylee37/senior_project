using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // 인스펙터에서 직접 프리팹을 할당할 변수

    // 이 메서드는 Inspector에서 버튼의 OnClick 이벤트에 직접
    public void SpawnPrefab()
    {
        if (prefabToSpawn != null)
        {
            // 할당된 프리팹을 복제하여 소환
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("프리팹이 할당되지 않았습니다.");
        }
    }
}
