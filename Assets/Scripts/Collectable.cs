using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private PlayerInventory _inventory;


    private void Awake()
    {
        _inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (this.gameObject.CompareTag("Magazine"))
            {
                if (other.CompareTag("Player"))
                {
                    _inventory.ammoInPockets += 10;
                    Destroy(this.gameObject);
                }
            }
            if (this.gameObject.CompareTag("Grenade Collectible"))
            {
                if (other.CompareTag("Player"))
                {
                    Destroy(this.gameObject);
                    _inventory.grenadeAmount += 1;
                }
            }
        }
    }
}
