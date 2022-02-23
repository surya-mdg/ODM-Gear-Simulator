using System.Collections;
using System.Collections.Generic;
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

    [Header("Camera Settings")]
    [SerializeField] new Camera camera;
    [SerializeField] float fov;
    [SerializeField] float wallRunFov;
    [SerializeField] float wallRunFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    [HideInInspector] public float tilt { get; private set; }

    bool checkWallLeft = false;
    bool checkWallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        
    }

    private void CheckWall()
    {
        checkWallLeft = Physics.Raycast(transform.position, -Orientation.right, out leftWallHit, wallCheckDistance);
        checkWallRight = Physics.Raycast(transform.position, Orientation.right, out rightWallHit, wallCheckDistance);
    }

    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight);
    }

    private void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if (checkWallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }

        if (checkWallLeft && Input.GetKey(KeyCode.Space))
        {
            Vector3 wallRunForce = transform.up + leftWallHit.normal;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunForce * wallRunJumpForce * 100f , ForceMode.Force);
        }
        else if (checkWallRight && Input.GetKey(KeyCode.Space))
        {
            Vector3 wallRunForce = transform.up + rightWallHit.normal;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunForce * wallRunJumpForce * 100f, ForceMode.Force);
        }
    }

    private void StopWallRun()
    {
        rb.useGravity = true;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
