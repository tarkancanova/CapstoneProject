using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthSystem : MonoBehaviour, IDamageable
{
    public int health;
    private const int _PLAYERHEALTH = 20;
    private const int _ENEMYHEALTH = 5;
    private const int _BARRELHEALTH = 1;
    public bool alive = true;
    private Pistol _weapon;
    private int _grenadeDamage = 10;

    private void Awake()
    {
        SetHealth();
        _weapon = GameObject.FindWithTag("Weapon").GetComponent<Pistol>();
        
    }

    private void Update()
    {
        Die();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            TakeDamage(_weapon.damage);
            Debug.Log("ok");
        }
    }



    private void SetHealth()
    {
        if (gameObject.CompareTag("Player"))
            health = _PLAYERHEALTH;
        else if (gameObject.CompareTag("Enemy"))
            health = _ENEMYHEALTH;
        else
            health = _BARRELHEALTH;
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
    }

    private void Die()
    {
        if (health <= 0)
        {
            if (gameObject.CompareTag("Player"))
                Debug.Log("Player died.");
            else if (gameObject.CompareTag("Enemy"))
                Destroy(gameObject);
        }
    }
}
