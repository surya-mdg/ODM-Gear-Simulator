using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] Transform Orientation;

    Rigidbody rb;

    [Header("Wall Run Settings")]
    [SerializeField] float wallRunGravity = 1f;
    [SerializeField] float wallCheckDistance = 1f;
    [SerializeField] float minJumpHeight = 1.5f;
    [SerializeField] float wallRunJumpForce = 10f;
    [SerializeField] LayerMask Ground;

    [Header("Camera Settings")]
    [SerializeField] new Camera camera;
    [SerializeField] float fov;
    [SerializeField] float wallRunFovIncrease;
    [SerializeField] float wallRunFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    [HideInInspector] public float tilt { get; private set; }
    [HideInInspector] public bool isWallRunning { get; private set; }

    bool checkWallLeft = false;
    bool checkWallRight = false;
    float verticalFOV;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        isWallRunning = false;
        
        verticalFOV = Camera.HorizontalToVerticalFieldOfView(fov, camera.aspect);
        camera.fieldOfView = verticalFOV;
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (checkWallLeft)
            {
                StartWallRun();
            }
            else if (checkWallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
        
    }

    private void CheckWall()
    {
        checkWallLeft = Physics.Raycast(transform.position, -Orientation.right, out leftWallHit, wallCheckDistance, Ground);
        checkWallRight = Physics.Raycast(transform.position, Orientation.right, out rightWallHit, wallCheckDistance, Ground);
    }

    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, Ground);
    }

    private void StartWallRun()
    {
        isWallRunning = true;
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, verticalFOV + Camera.HorizontalToVerticalFieldOfView(wallRunFovIncrease, camera.aspect), wallRunFovTime * Time.deltaTime);

        if (checkWallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if(checkWallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (checkWallLeft)
            {
                Vector3 wallRunForce = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunForce * wallRunJumpForce * 100f, ForceMode.Force);
            }
            else if (checkWallRight)
            {
                Vector3 wallRunForce = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunForce * wallRunJumpForce * 100f, ForceMode.Force);
            }
        }
    }

    private void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, verticalFOV, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
