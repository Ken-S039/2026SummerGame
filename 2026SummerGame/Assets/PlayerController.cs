using UnityEngine;

//ŒªŒŞì‚Ì‰¼‚ÌƒvƒŒƒCƒ„[‚Ì“®‚«‚È‚Ì‚ÅÁ‚·‚Â‚à‚è
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;
    }
}