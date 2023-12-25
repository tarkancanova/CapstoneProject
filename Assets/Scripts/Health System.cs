using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HealthSystem : MonoBehaviour, IDamageable
{
    public int health;
    private const int _PLAYERHEALTH = 20;
    private const int _ENEMYHEALTH = 3;
    private const int _BARRELHEALTH = 1;
    public bool alive = true;
    private Pistol _weapon;
    private Loot _loot;


    private void Awake()
    {
        SetHealth();
        _weapon = GameObject.FindWithTag("Weapon").GetComponent<Pistol>();
        if (!CompareTag("Player"))
        {
            _loot = GetComponent<Loot>();
        }
        
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
        else if (gameObject.CompareTag("Barrel"))
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
            {
                Destroy(GameObject.FindWithTag("Player"), 0.5f);
                SceneManager.LoadScene(1);
            }
            else if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Barrel"))
            {
                Destroy(gameObject);
                _loot.LootDrop();
            }
        }
    }
}
