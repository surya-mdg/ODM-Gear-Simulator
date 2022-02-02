using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform bulletPrefab;
    public Transform firePoint;

    public float bulletSpeed = 0.1f;

    public void Shoot()
    {
        Transform bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);
        Destroy(bullet.gameObject, 5f);
    }
}
