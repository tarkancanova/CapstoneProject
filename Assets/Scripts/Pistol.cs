using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject bulletPrefab;
    private PlayerInventory _inventory;
    GameObject bullet;
    public float bulletSpeed = 300f;
    public Transform bulletSpawnPoint;
    public int ammoInPockets;
    public int currentAmmo;
    public int maxAmmoInWeapon;
    public int damage = 1;
    PlayerActions playerActions;
    [SerializeField] private GameObject muzzleFlash;
    private Transform _muzzleFlashPosition;
    private GameObject _muzzle;
    private Vector3 _originalRotation;
    [SerializeField] private Vector3 _upRecoil;

    private void Awake()
    {
        _inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        currentAmmo = 10;
        playerActions = GameObject.FindWithTag("Player").GetComponent<PlayerActions>();
        bulletSpawnPoint = GameObject.FindWithTag("Weapon").transform;
        _muzzleFlashPosition = transform.GetChild(3);
    }

    private void Start()
    {
        _originalRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        ammoInPockets = _inventory.ammoInPockets;
    }


    public void Shoot()
    {
        
        bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        _muzzle = Instantiate(muzzleFlash, _muzzleFlashPosition.position, _muzzleFlashPosition.rotation);
        Destroy(_muzzle, 0.1f);
        Vector3 force = bulletSpawnPoint.forward * bulletSpeed;

        bullet.transform.GetChild(0).GetComponent<Rigidbody>().velocity = force;
        bullet.transform.GetChild(0).transform.localScale = bullet.transform.GetChild(0).transform.localScale * 4;
        Destroy(bullet, 3f);
        currentAmmo -= 1;
    }

    public void Reload()
    {
        if (playerActions.isReloading && ammoInPockets > 0 && currentAmmo < 10)
        {
            int ammoNeeded = 10 - currentAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, _inventory.ammoInPockets);

            currentAmmo += ammoToReload;
            _inventory.ammoInPockets -= ammoToReload;
        }
    }

    public void StartRecoil()
    {
        
        transform.localEulerAngles += _upRecoil;
    }

    public void StopRecoil()
    {
        transform.localEulerAngles = _originalRotation;
    }
}
