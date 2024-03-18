using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject prefabToSpawn;

    public int rate1 = 20, rate2 = 10, rate3 = 10, visit = 60; // 각 종족별 방문율
    public int visitation = 60; // 방문율
    public int finalRate1;
    public int finalRate2;
    public int finalRate3;

    private void Start()
    {
        var sum = rate1 + rate2 + rate3;
        var totalPreference = visit + (rate1 + rate2 + rate3) / 10;
        finalRate1 = rate1 * (totalPreference / (sum / 10)) / 10;
        finalRate2 = rate2 * (totalPreference / (sum / 10)) / 10;
        finalRate3 = rate3 * (totalPreference / (sum / 10)) / 10;
    }

    public void Calculate()
    {
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
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Prefab to spawn is null.");
        }
    }
}
