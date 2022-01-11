using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float targetNearbyDistance = 5f;
    public float speed=400f;
    public Animator animator;

    int currentWaypoint;
    float nextWaypointDistance = 0.5f;
    bool isNear = false;

    Seeker seeker;
    Rigidbody2D rb;
    Path path;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //seeker.StartPath(rb.position, target.position, OnPathComplete);
        InvokeRepeating("CreatePath", 0f, 0.5f);
    }

    private void Update()
    {
        float distance = Vector2.Distance(rb.position, target.position);

        if (distance <= targetNearbyDistance)
        {
            
            isNear = true;
            animator.SetBool("Moving", true);
        }
        else
        {
            isNear = false;
            animator.SetBool("Moving", false);
        }
    }
    void CreatePath()
    {
        if (isNear)
            AstarPath.active.Scan();
            seeker.StartPath(rb.position, target.position, OnPathComplete);

    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }

    }

    void FixedUpdate()
    {
        if (isNear)
        {

            if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                return;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 animDirection = ((Vector2)target.position - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);

            animator.SetFloat("Horizontal", animDirection.x);
            animator.SetFloat("Vertical", animDirection.y);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
    }
}
