using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;
    public WallRun wallRun;

    [SerializeField] float xSensitivity = 1f;
    [SerializeField] float ySensitivity = 1f;

    float multiplier = 0.01f;

    float mouseXPos;
    float mouseYPos;
    float xRotation;
    float yRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CursorTracker();

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void CursorTracker()
    {
        mouseXPos = Input.GetAxisRaw("Mouse X");
        mouseYPos = Input.GetAxisRaw("Mouse Y");

        xRotation -= mouseYPos * ySensitivity * multiplier;
        yRotation += mouseXPos * xSensitivity * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
