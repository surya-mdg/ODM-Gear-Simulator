using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traget : MonoBehaviour, IDamageble
{
    [SerializeField] float maxHealth = 100f;

    public void Damage(float damage)
    {
        maxHealth -= damage;

        if (maxHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
