using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float coyoteTime = 0.2f;
    public float maxSpeed = 5f;
    public LayerMask groundLayer;
    public Transform respawnPoint;
    public Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private bool isGrounded;
    private bool canJump;
    private int jumpCount;
    private float timeSinceGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);

       if (isGrounded)
    {
        canJump = true;
        jumpCount = 0;
        timeSinceGrounded = 0f; // resetear el tiempo
        animator.SetBool("IsJumping", false);
    }
    else
    {
        timeSinceGrounded += Time.deltaTime; // actualizar el tiempo
    }

        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            float direction = Mathf.Sign(moveInput);
            if (Mathf.Abs(rb.velocity.x) < maxSpeed)
            {
                rb.AddForce(new Vector2(moveSpeed * direction * acceleration * Time.deltaTime, 0f), ForceMode2D.Impulse);
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x * (1f - deceleration * Time.deltaTime), rb.velocity.y);
        }

        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            if (isGrounded)
            {
                animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            }
            
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump && jumpCount < 2)
        {
            Jump();
            canJump = false;
            jumpCount++;
        }

        // Check if player fell off the map and respawn at the last checkpoint
        if (transform.position.y < -10f)
        {
            transform.position = respawnPoint.position;
        }
    }

    private void Jump()
    {
        if (canJump || timeSinceGrounded < coyoteTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            canJump = false;
            jumpCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            other.GetComponent<Caja>().Break();
        }
    }
}
