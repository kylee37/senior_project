using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMove : MonoBehaviour
{
    private Transform[] pos;
    private int posIndex = 0;
    private Vector2 direction;
    private WaypointManager waypointManager;
    private NPCMover npcMover;
    private bool pathComplete = false;
    private Animator animator;

    private bool _isRollingDice = false;

    void Start()
    {
        waypointManager = FindObjectOfType<WaypointManager>();
        if (waypointManager != null)
        {
            pos = waypointManager.GetWaypoints();
            if (pos.Length > 0)
            {
                transform.position = pos[posIndex].transform.position;
            }
        }

        npcMover = GetComponent<NPCMover>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (pos != null && pos.Length > 0 && !pathComplete)
        {
            MovePath();
        }
    }

    void MovePath()
    {
        if(!pathComplete && !_isRollingDice)
        {
            _isRollingDice = true;
            if(Random.value < 0.5f)
            {
                Debug.Log("¹Ù·ÎÅ½»ö");
                npcMover.FindPathFromCurrentPosition();
                return;
            }
        }

        if (posIndex >= pos.Length)
        {
            pathComplete = true;
            if (npcMover != null)
            {
                npcMover.FindPathFromCurrentPosition();
            }
            return;
        }

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = pos[posIndex].transform.position;

        direction = (targetPosition - currentPosition).normalized;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, npcMover.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            posIndex++;
        }

        UpdateAnimation(direction);
    }

    void UpdateAnimation(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float moveX = direction.x > 0 ? 1 : direction.x < 0 ? -1 : 0;
            float moveY = direction.y > 0 ? 1 : direction.y < 0 ? -1 : 0;

            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
        }
    }
}
