using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;
    public GameObject clearText;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool isCleared = false;

    private Vector3 startPosition;

    public Transform groundCheck;
public float groundCheckRadius = 0.2f;
public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
    groundCheck.position,
    groundCheckRadius,
    groundLayer
);
        if (isCleared)
        {
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (transform.position.y < -10f)
        {
            Respawn();
        }
    }

    void FixedUpdate()
    {
        if (isCleared)
        {
            return;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            isCleared = true;

            rb.velocity = Vector2.zero;

            clearText.SetActive(true);
        }
    }

    void Respawn()
    {
        transform.position = startPosition;
        rb.velocity = Vector2.zero;
    }
}