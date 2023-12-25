using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loot : MonoBehaviour
{
    [SerializeField] private GameObject _magazine;
    [SerializeField] private GameObject _grenade;
    private bool _quitting = false;
    private HealthSystem _healthSystem;

    private void Start()
    {
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] barrelGameObjects = GameObject.FindGameObjectsWithTag("Barrel");

        foreach (GameObject go in enemyGameObjects)
        {
            go.AddComponent<Loot>();
        }
        foreach (GameObject go in barrelGameObjects)
            go.AddComponent<Loot>();
    }

    public void LootDrop()
    {
        if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Barrel"))
        {
            int dropDecider = Random.Range(1, 10);
            if (dropDecider < 3)
                Instantiate(_grenade, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            if (dropDecider >= 3 && dropDecider < 6)
                Instantiate(_magazine, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            else
                return;
        }
        else
            return;
    }
}
