using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 10f;
    public float groundDrag = 3f;
    public float airDrag = 0.4f;

    [Header("Inputs")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    Rigidbody rb;

    Vector3 distance;
    float moveHorizontal;
    float moveVertical;
    bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y + 0.1f);

        CalculateWalk();
        Drag();

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (isGrounded)
        {
            rb.AddForce(distance.normalized * moveSpeed * 10f, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(distance.normalized * (moveSpeed/10f) * 10f, ForceMode.Acceleration);
        }
    }

    void CalculateWalk()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        distance = transform.forward * moveVertical + transform.right * moveHorizontal;
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Drag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }
}
