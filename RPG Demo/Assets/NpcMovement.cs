using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    public Transform[] Waypoints; //List of all the points that form the NPC's path
    public Rigidbody2D rb;

    public float idleTime = 4f; //The amount of time the NPC has to wait before it goes to it's next path
    public float npcSpeed = 5f;

    int nextWayPoint;
    int currentWayPoint = 0;
    float distance;
    float idleTimeReduce; //A variable to which idleTime will be assigned so that you cannot see the idleTime keep changing in the Unity Inspector

    private void Start()
    {
        transform.position = Waypoints[currentWayPoint].position; //Sets the position of the NPC to the first waypoint of it's path
        idleTimeReduce = idleTime;
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, Waypoints[currentWayPoint].position);

        if (distance <= 0f)
        {
            idleTimeReduce -= Time.deltaTime;
        }
        else
        {
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
}
