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
    public int poolSize;

    private FameManager fameManager;
    private TimeManager timeManager;

    public int humanRate;
    public int dwarfRate;
    public int elfRate;
    public int visit;

    public Transform spawnPoint;  // ���� ��ġ�� ������ ����

    private void Start()
    {
        fameManager = FameManager.instance;

        humanRate = fameManager.sumHuman;
        dwarfRate = fameManager.sumDwarf;
        elfRate = fameManager.sumElf;
        visit = fameManager.visitation;

        timeManager = FindObjectOfType<TimeManager>();

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
        if (timeManager.timeSeconds >= 3)
        {
            int randomNum = Random.Range(1, 100);
            timeManager.timeSeconds = 0f;
            Calculate();
            Spawn();
            if (randomNum > 50)
            {
                Debug.Log("����");
                Invoke(nameof(Spawn), 0.25f);
            }
        }
    }

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
                if (spawnPoint != null)
                {
                    obj.transform.position = spawnPoint.position;
                }
                else
                {
                    obj.transform.position = transform.position;  // ���� ��ġ�� �������� ���� ���, PrefabSpawner�� ��ġ ���
                }
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
