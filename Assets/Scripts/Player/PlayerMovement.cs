using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isWalking = movement != Vector2.zero;
        animator.SetBool("isWalking", isWalking);

        animator.SetBool("isSide", false);
        animator.SetBool("isBack", false);

        if (isWalking)
        {
            if (Mathf.Abs(movement.x) > 0)
            {
                animator.SetBool("isSide", true);
                spriteRenderer.flipX = movement.x < 0;
            }
            else if (movement.y > 0)
            {
                animator.SetBool("isBack", true);
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * moveSpeed;
    }
}