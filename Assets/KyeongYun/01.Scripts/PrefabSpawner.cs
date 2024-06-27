using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
public class PrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject prefabToSpawn;
    public Queue<GameObject>[] objectPool;
    public int poolSize;                        // Ǯ ������

    private FameManager fameManager;            // FameManager �ν��Ͻ��� ������ ����
    private TimeManager timeManager;

    // �ν����� â���� �����?
     public int humanRate;
     public int dwarfRate;
     public int elfRate;

     public int visit;

    private void Start()
    {
        fameManager = FameManager.instance;     // FameManager���� �ʱ� ��ȣ�� �� ��������

        humanRate = fameManager.sumHuman;
        dwarfRate = fameManager.sumDwarf;
        elfRate = fameManager.sumElf;
        visit = fameManager.visitation;

        timeManager = FindObjectOfType<TimeManager>();

        // Ǯ �ʱ�ȭ
        objectPool = new Queue<GameObject>[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            objectPool[i] = new Queue<GameObject>();
            for (int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(prefabs[i]);
                obj.SetActive(false);
                objectPool[i].Enqueue(obj);
            }
        }
    }
    private void Update()
    {
        if(timeManager.timeSeconds >= 3) // 3�ʸ��� ����
        {
            int randomNum = Random.Range(1, 100);
            timeManager.timeSeconds = 0f;
            Calculate();
            Spawn();
            if(randomNum > 80) // ���� �湮 Ȯ���� n% �� �� ����: [value(randomNum) = 100 - n]
            {
                Debug.Log("����");
                Invoke(nameof(Spawn), 0.25f);
            }
        }
    }

    // rateName(NPC�̸�) ���� ������ ���� �� ���� NPC ��ȣ�� ����
    public void UpdateRate(string rateName, int newValue)
    {
        switch (rateName)
        {
            case "humanRate":
                humanRate = newValue;
                break;
            case "dwarfRate":
                dwarfRate = newValue;
                break;
            case "elfRate":
                elfRate = newValue;
                break;
            default:
                Debug.LogError("Invalid rate name: " + rateName);
                break;
        }
    }

    // �� NPC ��ȣ���� ���� �湮�� ��� + �� �湮���� ���� �湮�� NPC ����
    public void Calculate()
    {
        var sum = humanRate + dwarfRate + elfRate;
        var totalPreference = visit + (humanRate + dwarfRate + elfRate) / 10;
        humanRate = humanRate * (totalPreference / (sum / 10)) / 10;
        dwarfRate = dwarfRate * (totalPreference / (sum / 10)) / 10;
        elfRate = elfRate * (totalPreference / (sum / 10)) / 10;
        int randomNumber = Random.Range(1, 100);
        switch (randomNumber)
        {
            case int n when (n > 0 && n <= humanRate):
                prefabToSpawn = prefabs[0];
                Debug.Log("1 �湮");
                break;
            case int n when (n > humanRate && n <= humanRate + dwarfRate):
                prefabToSpawn = prefabs[1];
                Debug.Log("2 �湮");
                break;
            case int n when (n > humanRate + dwarfRate && n <= humanRate + dwarfRate + elfRate):
                prefabToSpawn = prefabs[2];
                Debug.Log("3 �湮");
                break;
            case int n when (n > humanRate + dwarfRate + elfRate && n <= 100):
                Debug.Log("�ƹ��� �湮���� ����");
                break;
            default:
                Debug.Log("No event occurred.");
                break;
        }
    }

    public void Spawn()
    {
        if (prefabToSpawn != null)
        {
            int index = GetPrefabIndex(prefabToSpawn);
            if (index != -1)
            {
                GameObject obj = GetObjectFromPool(index);
                obj.transform.position = transform.position;
                obj.SetActive(true);
            }
            else
            {
                Debug.LogError("Prefab not found in pool.");
            }
        }
        else
        {
            Debug.Log("Prefab to spawn is null.");
        }
    }
    private int GetPrefabIndex(GameObject prefab)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] == prefab)
            {
                return i;
            }
        }
        return -1;
    }

    private GameObject GetObjectFromPool(int index)
    {
        if (objectPool[index].Count == 0)
        {
            GameObject obj = Instantiate(prefabs[index]);
            obj.SetActive(false);
            objectPool[index].Enqueue(obj);
        }
        return objectPool[index].Dequeue();
    }

    public void ReturnObjectToPool(GameObject obj, int index)
    {
        obj.SetActive(false);
        objectPool[index].Enqueue(obj);
    }
}