using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour, IExplosive
{
    private int _grenadeRadius = 5;
    public int grenadeDamage = 10;
    private PlayerActions _playerActions;
    private PlayerInventory _inventory;
    private Collider[] _hitColliders;
    Rigidbody rb;
    public bool explosionReady;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _playerActions = GameObject.FindWithTag("Player").GetComponent<PlayerActions>();
    }

    private void Update()
    {
        if (explosionReady)
        {
            BombThrown();
        }
    }

    public void Explode()
    {
        int maxColliders = 105;
        _hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _grenadeRadius, _hitColliders);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                if (_hitColliders[i].TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
                {
                    healthSystem.health -= 10;
                }
            }
        }

        ActivateExplosions();

    }


    private void ActivateExplosions()
    {
        transform.GetChild(3).gameObject.SetActive(true);
    }

    private void DeactivateExplosions()
    {
        transform.GetChild(3).gameObject.SetActive(false);
    }

    private void StopBomb()
    {
        rb.velocity = Vector3.zero;
    }

    private void FalsifyBombWaiting()
    {
        _playerActions.bombWaiting = false;
    }

    private void BombThrown()
    {
        Invoke("Explode", 1.5f);
        Invoke("DeactivateExplosions", 1.7f);
        Invoke("StopBomb", 1.5f);
        Invoke("FalsifyBombWaiting", 1.5f);
        explosionReady = false;
        Destroy(gameObject, 3f);
        
    }
}
