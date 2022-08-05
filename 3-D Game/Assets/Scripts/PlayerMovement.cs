using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] new Transform camera;
    [SerializeField] public WallRun wallRun;
    [SerializeField] private Animator animLeft;
    [SerializeField] private Animator animRight;
    [SerializeField] private ParticleSystem speedlines;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float accelaration = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float groundDrag = 3f;
    [SerializeField] float airDrag = 0.4f;
    [SerializeField] float counterForce;

    Vector3 direction;
    Vector3 slopeDirection;
    float moveSpeed;
    float moveHorizontal;
    float moveVertical;

    [Header("Inputs")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    
    //Jump Bob Prototype
    bool transfer = false;
    bool isSine = false;
    float inter = 0f;
    Vector3 max;
    Vector3 startPos;

    [Header("Ground Detection")]
    [SerializeField] LayerMask Ground;
    [SerializeField] Transform groundDetection;
    public float jumpGravityMultiplier = 100f;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public Rigidbody rb;

    [Header("Camera Settings")]
    [SerializeField] Camera cameraMain;
    [SerializeField] float fov;
    [SerializeField] float fovIncrease;
    [SerializeField] float fovTime;

    RaycastHit slopeHit;
    float groundCheckRadius = 0.5f;
    float verticalFOV;

    [SerializeField] bool enableJumpBob = false;
    
    private void Awake()
    {
        verticalFOV = Camera.HorizontalToVerticalFieldOfView(fov, cameraMain.aspect);
        cameraMain.fieldOfView = verticalFOV;

        rb = GetComponent<Rigidbody>();

        startPos = camera.localPosition;
        rb.freezeRotation = true;
        moveSpeed = walkSpeed;

        //max = startPos - new Vector3(0f, 1f, 0f);
        max = new Vector3(0f, 1f, 0f);
        inter = startPos.y;

        speedlines.Stop();
    }

    private void Update()
    {
        if (rb.velocity.magnitude >= 60f)
        {
            cameraMain.fieldOfView = Mathf.Lerp(cameraMain.fieldOfView, verticalFOV + Camera.HorizontalToVerticalFieldOfView(fovIncrease, cameraMain.aspect), fovTime * Time.deltaTime);
            speedlines.Play();
        }
        else
        {
            speedlines.Stop();
            cameraMain.fieldOfView = Mathf.Lerp(cameraMain.fieldOfView, verticalFOV, fovTime * Time.deltaTime);
        }

        CalculateWalk(); //Calculated the direction in which the player should move
        CheckSlope();    //Checks if the player is on a slope
        ControlSpeed();  //Controls speed of player
        Drag();          //Adds Drag to player depending on whether the player is in air or on ground

        isGrounded = Physics.CheckSphere(groundDetection.position, groundCheckRadius, Ground);

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        if(transfer && isGrounded && enableJumpBob)
        {
            if (isSine)
            {
                camera.localPosition = Vector3.Lerp(camera.localPosition, max, inter);
                inter -= 10f * Time.deltaTime;
            }
            else
            {
                camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, inter);
                inter += 10f * Time.deltaTime;
                if (inter >= startPos.y)
                {
                    Debug.Log("inter value: " + inter + "  min value: " + startPos.y + "  Camera y position: " + camera.position.y);
                    transfer = false;
                }
            }

            if (inter <= max.y)
            {
                Debug.Log("inter value: " + inter + "  max value: " + max.y + "  Camera y position: " + camera.position.y);
                isSine = false;
            }
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

        if ((moveHorizontal != 0 || moveVertical != 0) && !isSprinting)
        {
            animLeft.SetBool("Walk", true);
            animRight.SetBool("Walk", true);
        }
        else
        {
            animLeft.SetBool("Walk", false);
            animRight.SetBool("Walk", false);
        }

        direction = orientation.forward * moveVertical + orientation.right * moveHorizontal;
        slopeDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal);
    }

    private void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded && (moveHorizontal != 0 || moveVertical != 0))
        {
            animLeft.SetBool("Sprint", true);
            animRight.SetBool("Sprint", true);
            isSprinting = true;
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, accelaration * Time.deltaTime);
        }
        else
        {
            animLeft.SetBool("Sprint", false);
            animRight.SetBool("Sprint", false);
            isSprinting = false;
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
            isSine = true;
            transfer = true;
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
}
