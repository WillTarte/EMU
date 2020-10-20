using System.Collections;
using UnityEngine;

/// <summary>
/// Meant for weapons that shoot bursts of bullets at one time. (shot at once, or in a row) (E.g shotgun / other) 
/// </summary>
[CreateAssetMenu(fileName = "NewBurstWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/Burst", order = 1)]
public class BurstWeaponShootStrategy: WeaponShootStrategy
{
    [SerializeField] private float spread;
    [SerializeField] private int numProjectiles;
    private bool _canReload = true;
    private bool _canShoot = true;

    public override void Shoot(WeaponBehaviourScript weapon)
    {
        if (_canShoot && weapon.CurrentMagazineAmmunition >= numProjectiles)
        {
            weapon.StartCoroutine(WaitForShot(weapon));
        }
    }

    private IEnumerator WaitForShot(WeaponBehaviourScript weapon)
    {
            _canShoot = false;
            _canReload = false;
            SpawnProjectile(weapon);
            weapon.CurrentMagazineAmmunition -= numProjectiles;
            yield return new WaitForSeconds(weapon.WeaponData.FireRate);
            _canReload = true;
            _canShoot = true; 
    }
    

    public override void Reload(WeaponBehaviourScript weapon)
    {
        if (_canReload && weapon.CurrentMagazineAmmunition < weapon.WeaponData.MagazineCapacity && weapon.CurrentTotalAmmunition >= 1)
        {
            weapon.StartCoroutine(WaitForReload(weapon));
        }
    }

    private IEnumerator WaitForReload(WeaponBehaviourScript weapon)
    {
        _canReload = false;
        _canShoot = false;
        int reloadAmount =
            weapon.CurrentTotalAmmunition < weapon.WeaponData.MagazineCapacity - weapon.CurrentMagazineAmmunition
                ? weapon.CurrentTotalAmmunition
                : weapon.WeaponData.MagazineCapacity - weapon.CurrentMagazineAmmunition;
        yield return new WaitForSeconds(weapon.WeaponData.ReloadTime);
        weapon.CurrentTotalAmmunition -= reloadAmount;
        weapon.CurrentMagazineAmmunition += reloadAmount;
        _canShoot = true;
        _canReload = true;
    }

    protected override void SpawnProjectile(WeaponBehaviourScript weapon)
    {
        //todo if spread is 0 (straight line) then bullets should have delay between each other
        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = -(spread / 2) + i * (spread / numProjectiles);
            //todo
            /*if (weapon.FacingDirection == Vector2.left)
            {
                if (angle >= 0.0f)
                {
                    angle = 180.0f - angle;
                }
                else
                {
                    angle = 180.0f + angle;
                }
            }*/
            GameObject projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.transform.position, Quaternion.Euler(angle, 0.0f, 0.0f));
            projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData, Vector2.right);
            projectile.SetActive(true);
        }
    }
}