using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcterMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    float moveSpeed = 6f;
    float smoothDampTiming = 0.1f;
    float refAngle;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y + 180;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref refAngle, smoothDampTiming);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle - 180, 0f) * Vector3.forward;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y + 180, 0f);
        }
    }
}
