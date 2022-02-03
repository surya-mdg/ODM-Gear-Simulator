using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHitCount = 3;
    public SpriteRenderer sr;

    private GameObject gm;

    Rigidbody2D rb;
    int hitCount = 0;
    float colorTime = 0f;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Game Manager");
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
        rb.AddForce(force, ForceMode2D.Impulse);
        if (hitCount >= maxHitCount)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Dead");
        gm.GetComponent<GameMaster>().Kill(this.gameObject);
    }
}
