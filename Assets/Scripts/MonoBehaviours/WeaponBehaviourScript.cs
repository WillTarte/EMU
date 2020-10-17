using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// Controls the behavior of a weapon. 
/// </summary>
public class WeaponBehaviourScript : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject projectilePrefab;
    private int _currentMagazineAmmunition;
    private int _currentTotalAmmunition;
    private bool _canShoot = false;
    private Vector2 _facingDirection = Vector2.right;
    private SpriteRenderer _spriteRenderer;
    private WeaponState _weaponState;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButton("Fire1") && !_canShoot)
        {
            StartCoroutine(ShootCoroutine());
        }


    }

    private IEnumerator ShootCoroutine()
    {
        _canShoot = true;
        Shoot();
        yield return new WaitForSeconds(weaponData.FireRate);
        _canShoot = false;

    }

    private void Shoot()
    {
        switch (weaponData.FireMode)
        {
            case WeaponData.FireModeType.Automatic:
                // shoot as long as the player holds down the trigger
                Debug.Log(WeaponData.FireModeType.Automatic.ToString());
                break;
            case WeaponData.FireModeType.SemiAutomatic:
                // shoot every time the player pulls the trigger
                Debug.Log(WeaponData.FireModeType.SemiAutomatic.ToString());

                break;
            case WeaponData.FireModeType.Burst:
                // variant of semi-automatic: shoot multiple projectiles every time player pulls the trigger
                Debug.Log(WeaponData.FireModeType.Burst.ToString());
                break;
            case WeaponData.FireModeType.Single:
                // player can only shoot once, then has to reload
                Debug.Log(WeaponData.FireModeType.Single.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SpawnProjectiles(int numProjectiles)
    {
        for (var i = 1; i <= numProjectiles; i++)
        {
            var projectile =
                Instantiate(projectilePrefab); //todo maybe spawn the projectile at position based on weapon sprite
            projectile.GetComponent<ProjectileBehaviourScript>().Init(weaponData.ProjectileData, _facingDirection);
            projectile.SetActive(true);
        }
    }


    private enum WeaponState
    {
        OnGround,
        InInventory,
        Inactive,
        Active
    }
}
