using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] new Camera camera;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float accelaration = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float groundDrag = 3f;
    [SerializeField] float airDrag = 0.4f;

    Vector3 direction;
    Vector3 slopeDirection;
    float moveSpeed;
    float moveHorizontal;
    float moveVertical;

    [Header("Inputs")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    Rigidbody rb;

    [Header("Ground Detection")]
    [SerializeField] LayerMask Ground;
    [SerializeField] Transform groundDetection;
    [SerializeField] float jumpGravityMultiplier = 100f;

    RaycastHit slopeHit;
    float groundCheckRadius = 0.1f;
    bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        CalculateWalk(); //Calculated the direction in which the player should move
        CheckSlope();    //Checks if the player is on a slope
        ControlSpeed();  //Controls speed of player
        Drag();          //Adds Drag to player depending on whether the player is in air or on ground

        isGrounded = Physics.CheckSphere(groundDetection.position, groundCheckRadius, Ground);

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

    }

    private void FixedUpdate()
    {
        Move(); //Adds force to move the player
    }

    private void Move()
    {
        if (isGrounded && !CheckSlope())
        {
            rb.AddForce(direction.normalized * moveSpeed * 10f, ForceMode.Acceleration);
        }
        else if(isGrounded && CheckSlope())
        {
            rb.AddForce(slopeDirection.normalized * moveSpeed * 10f, ForceMode.Acceleration);
        }
        else if(!isGrounded)
        {
            rb.AddForce(direction.normalized * (moveSpeed/3f) * 10f, ForceMode.Acceleration);
        }
    }

    private void CalculateWalk()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        direction = orientation.forward * moveVertical + orientation.right * moveHorizontal;
        slopeDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal);
    }

    private void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, accelaration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, accelaration * Time.deltaTime);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
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
            rb.AddForce(Vector3.down * jumpGravityMultiplier, ForceMode.Force);
        }
    }

    private bool CheckSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, transform.localScale.y + 0.5f, Ground))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
