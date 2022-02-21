using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera cam;

    public float xSensitivity = 1f;
    public float ySensitivity = 1f;

    float multiplier = 0.01f;

    float mouseXPos;
    float mouseYPos;
    float xRotation;
    float yRotation;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CursorTracker();

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
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
