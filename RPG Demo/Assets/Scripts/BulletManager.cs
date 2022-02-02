using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float arrowForce = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Vector2 direction = (collision.transform.position - this.transform.position).normalized;
            Vector2 force = direction * arrowForce * Time.deltaTime;
            collision.GetComponent<EnemyHealth>().TakeDamage(force);
        }
        Destroy(gameObject);
    }
}
