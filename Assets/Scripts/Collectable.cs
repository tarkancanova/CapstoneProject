using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Weapon weapon;


    private void Awake()
    {
        weapon = GameObject.FindWithTag("Weapon").GetComponent<Weapon>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (this.gameObject.CompareTag("Magazine"))
            {
                if (other.CompareTag("Player"))
                {
                    weapon.ammoInPockets += 10;
                    Destroy(this.gameObject);
                }
            }
            if (this.gameObject.CompareTag("Grenade"))
            {
                Destroy(this.gameObject);
                //grenade.grenadeAmount += 1;
            }
        }
    }
}
