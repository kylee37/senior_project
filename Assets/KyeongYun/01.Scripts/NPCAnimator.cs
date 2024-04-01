using UnityEngine;

public class NPCAnimator : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 direction = currentPosition - lastPosition;
        lastPosition = currentPosition;

        // 방향이 0보다 크면 오른쪽, 작으면 왼쪽
        if (direction.x > 0)
        {
            animator.SetFloat("MoveX", 1); // 오른쪽 애니메이션 재생
            animator.SetFloat("MoveY", 0);
        }
        else if (direction.x < 0)
        {
            animator.SetFloat("MoveX", -1); // 왼쪽 애니메이션 재생
            animator.SetFloat("MoveY", 0);
        }
        else if (direction.y > 0)
        {
            animator.SetFloat("MoveY", 1); // 위쪽 애니메이션 재생
            animator.SetFloat("MoveX", 0);
        }
        else if (direction.y < 0)
        {
            animator.SetFloat("MoveY", -1); // 아래쪽 애니메이션 재생
            animator.SetFloat("MoveX", 0);
        }
    }
}
