using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float airAcceleration = 1f;
    public float maxAirSpeed = 10f;


    public float jumpForce = 10f;
    public LayerMask groundLayer; // Layer mask to specify the ground layer

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isGrounded = false;
    public bool stunned = false;

    public GameObject checkpoint;

    public GameObject gameManager;
    private GameManager gm;

    public AudioClip oofSound;
    public AudioClip boingSound;
    public AudioClip punchSound;

    private AudioSource audioSource;

    public static float MaxMagnitude(float a, float b)
    {
        return Mathf.Abs(a) > Mathf.Abs(b) ? a : b;
    }

    public static float MinMagnitude(float a, float b)
    {
        return Mathf.Abs(a) < Mathf.Abs(b) ? a : b;
    }

    public IEnumerator Stun()
    {
        if (!stunned)
        {
            stunned = true;
            isGrounded = false;
            yield return new WaitForSeconds(0.5f);
            stunned = false;
            Die();
        }
    }

    void Die()
    {
        if (checkpoint)
        {
            transform.position = checkpoint.transform.Find("SpawnTransform").position;
            rb.velocity = new Vector3(0, 0, 0);
        }

        StartCoroutine(playSound());
        
    }

    public IEnumerator playSound()
    {
        yield return null;

        audioSource.Play();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (gameManager)
        {
            gm = gameManager.GetComponent<GameManager>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (isGrounded)
        {
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);
        }
        else
        {
            Vector3 inputForce = new Vector3(moveHorizontal * airAcceleration, 0, moveVertical * airAcceleration);
            Vector3 totalForce = rb.velocity + inputForce;

            Vector3 flatVelocity = new Vector3(totalForce.x, 0, totalForce.z);
            if (flatVelocity.magnitude > maxAirSpeed)
            {
                totalForce = totalForce.normalized * maxAirSpeed;
            }

            rb.velocity = new Vector3(totalForce.x, rb.velocity.y, totalForce.z);
        }

        if (transform.position.y < -5f)
        {
            Die();
        }

        if (stunned)
        {
            rb.velocity = new Vector3(0, 5, -100);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    void OnCollisionStay(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (!stunned) isGrounded = true;
        }

        if (collision.gameObject.transform.Find("SpawnTransform"))
        {
            if (checkpoint)
            {
                if (collision.gameObject.transform.position.x > checkpoint.transform.position.x)
                {
                    checkpoint = collision.gameObject;
                }
            }
            else
            {
                checkpoint = collision.gameObject;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (!stunned )isGrounded = false;
            gm.StartGame();
        }
    }
}
