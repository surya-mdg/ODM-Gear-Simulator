using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float playerHealth = 100f;
    public float damageTimeRate = 4f;
    public float enemyDamage = 20f;

    public HealthBar hb;

    float timeCounter = 0f;
    bool isColliding = false;

    private void Start()
    {
        hb.MaxHealth(playerHealth);
    }

    private void Update()
    {
        timeCounter -= Time.deltaTime;
        if ((timeCounter <= 0f) && (isColliding))
        {
            hb.ReduceHealth(enemyDamage);
            timeCounter = damageTimeRate;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isColliding = false;
        }
    }
}
