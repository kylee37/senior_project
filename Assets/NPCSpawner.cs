using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabsToSpawn;
    public void NPCSpawn()
    {
        Instantiate(npcPrefabsToSpawn[0], transform.position, Quaternion.identity);
    }
}
