using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHitCount = 3;
    public SpriteRenderer sr;

    Rigidbody2D rb;
    int hitCount = 0;
    float colorTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        colorTime -= Time.deltaTime;
        if(colorTime<=0f)
        {
            sr.color = Color.white;
        }
    }

    public void TakeDamage(Vector2 force)
    {
        colorTime = 0.2f;
        sr.color = Color.red;
        Debug.Log("Hit");
        hitCount++;
        rb.AddForce(force);
        if (hitCount >= maxHitCount)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Dead");
    }
}
