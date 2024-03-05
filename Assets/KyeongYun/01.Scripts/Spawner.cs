using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // �ν����Ϳ��� ���� �������� �Ҵ��� ����

    // �� �޼���� Inspector���� ��ư�� OnClick �̺�Ʈ�� ����
    public void SpawnPrefab()
    {
        if (prefabToSpawn != null)
        {
            // �Ҵ�� �������� �����Ͽ� ��ȯ
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("�������� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
