using System.Collections;
using UnityEngine;

/// <summary>
/// Meant for weapons that can only shoot once before reloading (E.g rpg)
/// </summary>
[CreateAssetMenu(fileName = "NewSingleWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/Single", order = 4)]
public class SingleWeaponShootStrategy : WeaponShootStrategy
{
    private bool _canShoot = true;
    private bool _canReload = true;
    public override void Shoot(WeaponBehaviourScript weapon)
    {
        if (_canShoot && weapon.CurrentMagazineAmmunition >= 0)
        {
            _canShoot = false;
            weapon.StartCoroutine(WaitForShot(weapon));
        }
    }

    private IEnumerator WaitForShot(WeaponBehaviourScript weapon)
    {
        _canShoot = false;
        _canReload = false;
        SpawnProjectile(weapon);
        yield return new WaitForSeconds(0.1f);
        _canReload = true;
    }
    
    public override void Reload(WeaponBehaviourScript weapon)
    {
        if (_canReload && weapon.CurrentMagazineAmmunition <= 0 && weapon.CurrentMagazineAmmunition >= 1)
        {
            weapon.StartCoroutine(WaitForReload(weapon));
        }
    }

    private IEnumerator WaitForReload(WeaponBehaviourScript weapon)
    {
        _canReload = false;
        _canShoot = false;
        yield return new WaitForSeconds(weapon.WeaponData.ReloadTime);
        weapon.CurrentMagazineAmmunition += 1;
        weapon.CurrentTotalAmmunition -= 1;
        _canShoot = true;
        _canReload = true;
    }
    
    protected override void SpawnProjectile(WeaponBehaviourScript weapon)
    {
        //todo direction
        GameObject projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData, Vector2.right);
        projectile.SetActive(true);
    }
}