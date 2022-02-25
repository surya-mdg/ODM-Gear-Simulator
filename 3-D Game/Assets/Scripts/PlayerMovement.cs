using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] new Camera camera;
    [SerializeField] WallRun wallRun;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float accelaration = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float groundDrag = 3f;
    [SerializeField] float airDrag = 0.4f;
    [SerializeField] float counterForce;


    Vector3 counterDirection;
    Vector3 direction;
    Vector3 slopeDirection;
    float moveSpeed;
    float moveHorizontal;
    float moveVertical;
    float counterHorizontal;
    float counterVertical;

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
        CounterMovement(); //Adds couner movement to make controls feel little sharp

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

    void CounterMovement()
    {
        
        if (direction.magnitude!=0)
        {
            counterDirection = direction;
            
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            rb.AddForce(orientation.right * counterForce * counterDirection.magnitude, ForceMode.Impulse);
            Debug.Log("X: " + rb.velocity.x + "   Z: " + rb.velocity.z+"    direction: "+counterDirection.magnitude);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            rb.AddForce(-orientation.right * counterForce * counterDirection.magnitude, ForceMode.Impulse);
            Debug.Log("X: " + rb.velocity.x + "   Z: " + rb.velocity.z);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            rb.AddForce(-orientation.forward * counterForce * counterDirection.magnitude, ForceMode.Impulse);
            Debug.Log("X: " + rb.velocity.x + "   Z: " + rb.velocity.z);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            rb.AddForce(orientation.forward * counterForce * counterDirection.magnitude, ForceMode.Impulse);
            Debug.Log("X: " + rb.velocity.x + "   Z: " + rb.velocity.z);
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
            if (!wallRun.isWallRunning)
            {
                rb.AddForce(Vector3.down * jumpGravityMultiplier, ForceMode.Force);
            }
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

    Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
}
