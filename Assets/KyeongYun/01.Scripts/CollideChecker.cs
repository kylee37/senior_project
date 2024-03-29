using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class CollideChecker : MonoBehaviour
{
    [HideInInspector]
    public static bool isColliding;
    public StateManager stateManager;
    private SpriteRenderer spriteRenderer;
    private Color originColor;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (stateManager.currentState == State.Spawned && collision.CompareTag("SpawnedObject"))
        {
            isColliding = true;
            spriteRenderer.color = Color.red;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (stateManager.currentState == State.Spawned && collision.CompareTag("SpawnedObject"))
        {
            isColliding = false;
            spriteRenderer.color = originColor;
        }
    }
}
