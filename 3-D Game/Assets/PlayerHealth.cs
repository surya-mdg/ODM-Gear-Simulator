using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    [SerializeField] private float health = 10f;
    [SerializeField] private float bulletDamage = 10f;

    private void Awake()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
            healthBar.value -= bulletDamage;
    }
}
