using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    GameObject bullet;
    public float bulletSpeed = 5192132f;
    public Transform bulletSpawnPoint;
    public int ammoInPockets;
    public int currentAmmo;
    public int maxAmmoInWeapon;
    public int damage = 1;
    PlayerActions playerActions;

    private void Awake()
    {
        ammoInPockets = 10;
        currentAmmo = 10;
        playerActions = GameObject.FindWithTag("Player").GetComponent<PlayerActions>();
    }


    public void Shoot()
    {
        bulletSpawnPoint = GameObject.FindWithTag("Weapon").transform;
        bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        Vector3 force = bulletSpawnPoint.forward * bulletSpeed;

        bullet.transform.GetChild(0).GetComponent<Rigidbody>().velocity = force;
        bullet.transform.GetChild(0).transform.localScale = bullet.transform.GetChild(0).transform.localScale * 4;
    }

    IEnumerator Reload()
    {
        if (playerActions.isReloading && ammoInPockets > 0 && currentAmmo < 10)
        {
            while (currentAmmo < 11 && ammoInPockets > 0)
            {
                yield return new WaitForSeconds(1.5f);
                currentAmmo++;
                ammoInPockets--;
                playerActions.isReloading = false;
            }
        }

    }
}
