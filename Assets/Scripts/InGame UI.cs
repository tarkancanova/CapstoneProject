using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _UIText;
    private HealthSystem _healthSystem;
    private Grenade _grenade;
    private Pistol _pistol;
    private PlayerInventory _inventory;
    private void Awake()
    {
        _pistol = GameObject.FindWithTag("Weapon").GetComponent<Pistol>();
        _healthSystem = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();
        _inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        _UIText.text = ("Weapon ammo: " + _pistol.currentAmmo + "/" + _inventory.ammoInPockets + "\n" + "Grenade: " + _inventory.grenadeAmount + "\n" + "Player Health: " + _healthSystem.health);
    }
}
