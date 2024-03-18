using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabToSpawn;
    public void NPCSpawn()
    {
        Instantiate(npcPrefabToSpawn[0],transform.position,Quaternion.identity);
    }
}
