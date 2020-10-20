using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// Controls the behavior of a weapon. 
/// </summary>
public class WeaponBehaviourScript : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private WeaponShootStrategy shootStrategy;
    private int _currentMagazineAmmunition;
    private int _currentTotalAmmunition;
    private float _timeSinceLastShot;
    private SpriteRenderer _spriteRenderer;
    private WeaponState _weaponState;
    private Rigidbody2D _rigidbody;
    
    public WeaponData WeaponData => weaponData;

    public int CurrentMagazineAmmunition
    {
        get => _currentMagazineAmmunition;
        set => _currentMagazineAmmunition = value;
    }

    public int CurrentTotalAmmunition
    {
        get => _currentTotalAmmunition;
        set => _currentTotalAmmunition = value;
    }

    public float TimeSinceLastShot
    {
        get => _timeSinceLastShot;
        set => _timeSinceLastShot = value;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
    }

    public void Shoot()
    {
        shootStrategy.Shoot(this);
    }

    private enum WeaponState
    {
        OnGround,
        InInventory,
        Inactive,
        Active
    }
}
