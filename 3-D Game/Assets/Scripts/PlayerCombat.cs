using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator animLeft;
    [SerializeField] private Animator animRight;

    [Header("Controls")]
    [SerializeField] private KeyCode attackButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode holdButton = KeyCode.X;

    [Header("values")]
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool hold = false;
    [SerializeField] private float attackCooldown = 0.2f;
    [SerializeField] private float attackRadius = 3f;

    void Update()
    {
        
        if(Input.GetKeyDown(attackButton) && canAttack && !hold)
        {
            animLeft.SetTrigger("Attack");
            animRight.SetTrigger("Attack");
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius);
            foreach(Collider i in colliders)
            {
                if(i.gameObject.tag=="Enemy")
                    i.GetComponent<Enemy>().die = true;
            }
            canAttack = false;
            StartCoroutine(Cooldown());
        }

        if(hold)
        {
            if(rb.velocity.magnitude>60f)
            {
                Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius);
                foreach (Collider i in colliders)
                {
                    if (i.gameObject.tag == "Enemy")
                        i.GetComponent<Enemy>().die = true;
                }
            }
        }

        if(Input.GetKeyDown(holdButton))
        {
            hold = true;
            animLeft.SetTrigger("Attack");
            animRight.SetTrigger("Attack");
            StartCoroutine(Hold());
        }
        else if(Input.GetKeyUp(holdButton))
        {
            hold = false;
            animLeft.SetBool("Hold", false);
            animRight.SetBool("Hold", false);
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator Hold()
    {
        yield return new WaitForSeconds(0.3f);

        animLeft.SetBool("Hold", true);
        animRight.SetBool("Hold", true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }
}
