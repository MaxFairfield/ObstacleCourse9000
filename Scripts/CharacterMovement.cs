using System;
using Unity.Mathematics;
using UnityEngine;

public class ChracterMovement : MonoBehaviour
{
    public float groundSpeed = 5f;
    public float airSpeed = 3f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public CircleCollider2D groundCheckCollider;

    private Rigidbody2D rb;
    private GameObject groundObject;

    private bool jump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheckCollider = GetComponent<CircleCollider2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        Vector2 circlePosition = (Vector2)groundCheckCollider.transform.position + groundCheckCollider.offset;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(circlePosition, groundCheckCollider.radius, groundLayer);
        
        groundObject = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                groundObject = collider.gameObject;
                break;
            }
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (groundObject != null)
        {
            Rigidbody2D groundRb = groundObject.GetComponent<Rigidbody2D>();

            rb.velocity = new Vector2(groundRb.velocity.x + (moveInput * groundSpeed), rb.velocity.y);

            if (jump)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            float newVelocity = rb.velocity.x + (moveInput/5);

            if (Mathf.Abs(newVelocity) < Mathf.Abs(rb.velocity.x) || Math.Abs(newVelocity) <= airSpeed)
            {
                rb.velocity = new Vector2(newVelocity, rb.velocity.y);
            }
        } 

        jump = false;       
    }
}
