using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float playerHealth = 100f;
    public float damageTimeRate = 4f;
    public float enemyDamage = 20f;

    public HealthBar hb;

    private GameObject gm;

    float currentPlayerHealth;
    float timeCounter = 0f;
    bool isColliding = false;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Game Manager");

        hb.MaxHealth(playerHealth);
        currentPlayerHealth = playerHealth;
    }

    private void Update()
    {
        timeCounter -= Time.deltaTime;
        if ((timeCounter <= 0f) && (isColliding))
        {
            hb.ReduceHealth(enemyDamage);
            currentPlayerHealth -= enemyDamage;
            timeCounter = damageTimeRate;
        }

        if (currentPlayerHealth <= 0f)
        {
            gm.GetComponent<GameMaster>().Kill(this.gameObject);
        }
    }

    public void Respawn()
    {
        hb.MaxHealth(playerHealth);
        currentPlayerHealth = playerHealth;
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
