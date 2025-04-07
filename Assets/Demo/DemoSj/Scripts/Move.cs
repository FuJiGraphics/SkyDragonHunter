using UnityEngine;

public class Move : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 10f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(h, v).normalized;

        // Animator �Ķ���� ����
        animator.SetFloat("MoveX", Mathf.Abs(moveDir.x));
        animator.SetFloat("MoveY", moveDir.y);

        // ����
        if (spriteRenderer != null && moveDir.x != 0)
        {
            spriteRenderer.flipX = moveDir.x < 0;
        }

        // ���� �̵�
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
}
