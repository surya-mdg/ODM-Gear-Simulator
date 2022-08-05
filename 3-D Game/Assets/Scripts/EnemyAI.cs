
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform projectile;
    public Transform attackPoint;

    public Transform player;
    public MultiAimConstraintData aimR;

    public LayerMask whatIsGround, whatIsPlayer;
    public Animator anim;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private RaycastHit hit;

    private void Awake()
    {
        //player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        Physics.Raycast(transform.position, player.transform.position - transform.position,out hit,attackRange);
        if((hit.collider!=null) && hit.collider.tag=="Player")
        {
            playerInAttackRange = true;
        }
        else
        {
            playerInAttackRange = false;
        }

        anim.SetBool("running", playerInSightRange && !playerInAttackRange);
        anim.SetBool("Attack", playerInAttackRange);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        
        if (playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, attackRange);
        if (hit.collider.tag == "Player")
        {
            agent.SetDestination(transform.position);
        }

        Vector3 dir = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);
        Quaternion final = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, final, Time.deltaTime * 10f);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Transform bullet = Instantiate(projectile, attackPoint.position, projectile.rotation);
            bullet.GetComponent<Bullet>().Direction(dir);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
