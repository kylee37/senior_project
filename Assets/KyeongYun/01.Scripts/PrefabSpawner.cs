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

    public int rate1 = 20, rate2 = 10, rate3 = 10, visit = 60; // 각 종족별 선호도
    public int visitation = 60; // 방문율
    public int finalRate1;
    public int finalRate2;
    public int finalRate3;
    private TimeManager timeManager;

    public int myVariable = 0;

    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        var sum = rate1 + rate2 + rate3;
        var totalPreference = visit + (rate1 + rate2 + rate3) / 10;
        finalRate1 = rate1 * (totalPreference / (sum / 10)) / 10;
        finalRate2 = rate2 * (totalPreference / (sum / 10)) / 10;
        finalRate3 = rate3 * (totalPreference / (sum / 10)) / 10;

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
        if(timeManager.timeSeconds >= 3) // 5초마다 스폰
        {
            timeManager.timeSeconds = 0f;
            Spawn();
            Debug.Log("Spawn");
        }
    }
    public void UpdateRate(string rateName, int newValue)
    {
        switch (rateName)
        {
            case "rate1":
                rate1 = newValue;
                break;
            case "rate2":
                rate2 = newValue;
                break;
            case "rate3":
                rate3 = newValue;
                break;
            default:
                Debug.LogError("Invalid rate name: " + rateName);
                break;
        }
    }

    public void Calculate()
    {
        var sum = rate1 + rate2 + rate3;
        var totalPreference = visit + (rate1 + rate2 + rate3) / 10;
        finalRate1 = rate1 * (totalPreference / (sum / 10)) / 10;
        finalRate2 = rate2 * (totalPreference / (sum / 10)) / 10;
        finalRate3 = rate3 * (totalPreference / (sum / 10)) / 10;
        int randomNumber = Random.Range(1, 100);
        switch (randomNumber)
        {
            case int n when (n > 0 && n <= finalRate1):
                prefabToSpawn = prefabs[0];
                Debug.Log("1 방문");
                break;
            case int n when (n > finalRate1 && n <= finalRate1 + finalRate2):
                prefabToSpawn = prefabs[1];
                Debug.Log("2 방문");
                break;
            case int n when (n > finalRate1 + finalRate2 && n <= finalRate1 + finalRate2 + finalRate3):
                prefabToSpawn = prefabs[2];
                Debug.Log("3 방문");
                break;
            case int n when (n > finalRate1 + finalRate2 + finalRate3 && n <= 100):
                Debug.Log("아무도 방문하지 않음");
                break;
            default:
                Debug.Log("No event occurred.");
                break;
        }
    }

    public void Spawn()
    {
        Calculate();
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