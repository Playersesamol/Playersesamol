using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Transform vidaUIContainer; // Contenedor de las imágenes de vida en la UI
    public JumpButton jumpButton; // Referencia al script JumpButton
    public AudioClip fallAudioClip; // Clip de audio para cuando el jugador cae

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private bool isGrounded;
    private bool canJump;
    private int jumpCount;
    private float timeSinceGrounded;
    private int vidasIniciales = 3;
    private int vidasRestantes;
    private AudioSource audioSource;
    private bool moveLeft;
    private bool moveRight;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        vidasRestantes = vidasIniciales;
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

        float moveInput = GetMovementInput();
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

        if (jumpButton.IsJumpPressed() && canJump && jumpCount < 2) // Usamos el método IsJumpPressed() del JumpButton
        {
            Jump();
            canJump = false;
            jumpCount++;
        }

        // Check if player fell off the map and respawn at the last checkpoint
        if (transform.position.y < -10f)
        {
            ReducirVida();
            transform.position = respawnPoint.position;
            if (fallAudioClip != null)
            {
                audioSource.PlayOneShot(fallAudioClip); // Reproducir el sonido de caída
            }
        }
    }

    private float GetMovementInput()
    {
        float moveInput = 0f;
        if (moveLeft)
        {
            moveInput = -1f;
        }
        else if (moveRight)
        {
            moveInput = 1f;
        }
        return moveInput;
    }

    public void OnLeftButtonPointerDown()
    {
        moveLeft = true;
    }

    public void OnLeftButtonPointerUp()
    {
        moveLeft = false;
    }

    public void OnRightButtonPointerDown()
    {
        moveRight = true;
    }

    public void OnRightButtonPointerUp()
    {
        moveRight = false;
    }

    private void Jump()
    {
        if (canJump || timeSinceGrounded < coyoteTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            audioSource.Play();
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

    private void ReducirVida()
    {
        vidasRestantes--;

        if (vidasRestantes <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            // Destruir la imagen de vida correspondiente en la UI
            string vidaName = "vida" + vidasRestantes;
            Transform vidaUI = vidaUIContainer.Find(vidaName);
            if (vidaUI != null)
            {
                Destroy(vidaUI.gameObject);
            }
        }
    }
}
