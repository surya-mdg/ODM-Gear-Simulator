using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    public Transform[] Waypoints; //List of all the points that form the NPC's path
    public Animator anim;
    public Rigidbody2D rb;

    [HideInInspector]
    public bool canMove = true;

    public float idleTime = 5f; //The amount of time the NPC has to wait before it goes to it's next path
    public float npcSpeed = 5f;

    GameObject player;
    readonly string[] IdleAnimations = { "Up", "Down", "Right", "Left" };
    Vector2 direction;
    int nextWayPoint;
    int currentWayPoint = 0;
    float distance;
    float animTime = 0f;
    float idleTimeReduce; //A variable to which idleTime will be assigned so that you cannot see the idleTime keep changing in the Unity Inspector

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = Waypoints[currentWayPoint].position; //Sets the position of the NPC to the first waypoint of it's path
        idleTimeReduce = idleTime;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            anim.SetBool("Chatting", false);
            distance = Vector2.Distance(rb.position, Waypoints[currentWayPoint].position);

            if (distance <= 0.1f)
            {
                anim.SetBool("Moving", false);
                idleTimeReduce -= Time.deltaTime;
                animTime -= Time.deltaTime;

                anim.SetFloat("Horizontal", 0f);
                anim.SetFloat("Vertical", 0f);

                if (animTime <= 0f)
                {
                    anim.Play("Trent_Idle_" + IdleAnimations[Random.Range(0, IdleAnimations.Length)]);
                    animTime = 2f;
                }
            }
            else
            {
                direction = (Waypoints[currentWayPoint].position - transform.position);
                anim.SetBool("Moving", true);
                anim.SetFloat("Horizontal", direction.x);
                anim.SetFloat("Vertical", direction.y);
                transform.position = Vector2.MoveTowards(transform.position, Waypoints[currentWayPoint].position, npcSpeed * Time.deltaTime);
                
            }

            if (idleTimeReduce < 0f) //Chooses a ramdom point to move towards
            {
                do
                {
                    nextWayPoint = Random.Range((currentWayPoint - 1), (currentWayPoint + 2));

                    if (nextWayPoint == -1)
                        nextWayPoint = (Waypoints.Length - 1);

                    else if (nextWayPoint == Waypoints.Length)
                        nextWayPoint = 0;

                } while (currentWayPoint == nextWayPoint);

                currentWayPoint = nextWayPoint;
                idleTimeReduce = idleTime;
            }
        }
        else
        {
            direction = (player.transform.position - transform.position);
            anim.SetBool("Chatting", true);
            anim.SetFloat("Horizontal", direction.x);
            anim.SetFloat("Vertical", direction.y);
        }
    }
}
