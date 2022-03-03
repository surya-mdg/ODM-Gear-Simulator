using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] GunData gun;
    [SerializeField] Transform cam;

    float timeSinceLastShot = 0f;
    float currentAmmo;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

        currentAmmo = gun.currentAmmo;
    }

    private void OnDisable()
    {
        gun.reloading = false;
    }

    private void StartReload()
    {
        if (!gun.reloading && this.gameObject.activeSelf)
        {
            StartCoroutine("Reload");
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reloading");

        gun.reloading = true;
        yield return new WaitForSeconds(gun.reloadTime);

        currentAmmo = gun.magSize;
        gun.reloading = false;
    }

    private bool CanShoot() => !gun.reloading && timeSinceLastShot >= (1f / (gun.fireRate / 60f));

    private void Shoot()
    {
        Debug.Log("Shot Gun");
        if (currentAmmo > 0)
        {
            if (CanShoot())
            {
                if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gun.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name);

                    IDamageble damageble = hitInfo.transform.GetComponent<IDamageble>();
                    damageble?.Damage(gun.damage);
                }

                currentAmmo--;
                timeSinceLastShot = 0f;
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(cam.position, cam.forward * gun.maxDistance);
    }
}
