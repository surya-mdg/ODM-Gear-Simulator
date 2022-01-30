using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Camera cam;

    public float moveSpeed = 5f;

    [HideInInspector]
    public bool canMove = true;

    Vector2 mousePos;
    Vector2 movement;
    bool activateAim = false;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire2"))
        {
            activateAim = true;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            activateAim = false;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (!activateAim)
            {
                rb.MovePosition(rb.position + (moveSpeed * Time.fixedDeltaTime * movement));

                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Speed", movement.magnitude);
            }
            else
            {
                rb.MovePosition(rb.position + ((moveSpeed/2) * Time.fixedDeltaTime * movement));

                Vector2 lookdir = mousePos - rb.position;
                float angle = Mathf.Atan2(lookdir.x, lookdir.y) * Mathf.Rad2Deg;
                rb.rotation = angle;
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}
