using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyList;
    public Animator anim;
    public float attackRange = 0.5f;
    public float attackImpact = 20f;
    public float attackRate = 2f;

    float attackTime = 0f;

    void Update()
    {
        attackTime -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            if(attackTime<=0f)
            {
                Attack();
                attackTime = attackRate;
            }
        }
    }

    void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyList);

        foreach(Collider2D enemy in enemies)
        {
            Vector2 direction = (enemy.transform.position - this.transform.position).normalized;
            Vector2 force = direction * attackImpact * Time.deltaTime;
            enemy.GetComponent<EnemyHealth>().TakeDamage(force);

            anim.SetFloat("Attack_Horizontal", direction.x);
            anim.SetFloat("Attack_Vertical", direction.y);
            anim.SetTrigger("Attack");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
