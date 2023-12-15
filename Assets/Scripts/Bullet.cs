using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Pistol _weapon;
    public bool isShot = false;

    private void Awake()
    {
        _weapon = GameObject.FindWithTag("Weapon").GetComponent<Pistol>();
    }
}
