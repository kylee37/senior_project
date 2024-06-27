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
    public int poolSize;                        // 풀 사이즈

    private FameManager fameManager;            // FameManager 인스턴스를 저장할 변수
    private TimeManager timeManager;

    // 인스펙터 창에서 숨기기?
     public int humanRate;
     public int dwarfRate;
     public int elfRate;

     public int visit;

    private void Start()
    {
        fameManager = FameManager.instance;     // FameManager에서 초기 선호도 값 가져오기

        humanRate = fameManager.sumHuman;
        dwarfRate = fameManager.sumDwarf;
        elfRate = fameManager.sumElf;
        visit = fameManager.visitation;

        timeManager = FindObjectOfType<TimeManager>();

        // 풀 초기화
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
        if(timeManager.timeSeconds >= 3) // 3초마다 스폰
        {
            int randomNum = Random.Range(1, 100);
            timeManager.timeSeconds = 0f;
            Calculate();
            Spawn();
            if(randomNum > 80) // 동시 방문 확률이 n% 일 때 계산식: [value(randomNum) = 100 - n]
            {
                Debug.Log("동행");
                Invoke(nameof(Spawn), 0.25f);
            }
        }
    }

    // rateName(NPC이름) 값이 다음과 같을 때 각각 NPC 선호도 증감
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

    // 각 NPC 선호도에 따른 방문률 계산 + 각 방문률에 따른 방문할 NPC 도출
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
                Debug.Log("1 방문");
                break;
            case int n when (n > humanRate && n <= humanRate + dwarfRate):
                prefabToSpawn = prefabs[1];
                Debug.Log("2 방문");
                break;
            case int n when (n > humanRate + dwarfRate && n <= humanRate + dwarfRate + elfRate):
                prefabToSpawn = prefabs[2];
                Debug.Log("3 방문");
                break;
            case int n when (n > humanRate + dwarfRate + elfRate && n <= 100):
                Debug.Log("아무도 방문하지 않음");
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