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

        // ������ 0���� ũ�� ������, ������ ����
        if (direction.x > 0)
        {
            animator.SetFloat("MoveX", 1); // ������ �ִϸ��̼� ���
            animator.SetFloat("MoveY", 0);
        }
        else if (direction.x < 0)
        {
            animator.SetFloat("MoveX", -1); // ���� �ִϸ��̼� ���
            animator.SetFloat("MoveY", 0);
        }
        else if (direction.y > 0)
        {
            animator.SetFloat("MoveY", 1); // ���� �ִϸ��̼� ���
            animator.SetFloat("MoveX", 0);
        }
        else if (direction.y < 0)
        {
            animator.SetFloat("MoveY", -1); // �Ʒ��� �ִϸ��̼� ���
            animator.SetFloat("MoveX", 0);
        }
    }
}
