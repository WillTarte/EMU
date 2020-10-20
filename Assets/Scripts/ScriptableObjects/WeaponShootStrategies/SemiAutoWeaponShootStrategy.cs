using System.Collections;
using UnityEngine;

/// <summary>
/// Meant for weapons that can only shoot 1 bullet per click. (E.g. sniper)
/// </summary>
[CreateAssetMenu(fileName = "NewSemiAutoWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/SemiAuto", order = 3)]
public class SemiAutoWeaponShootStrategy: WeaponShootStrategy
{
    private bool _canReload = true;
    private bool _canShoot = true;
    public override void Shoot(WeaponBehaviourScript weapon)
    {
        if (_canShoot && weapon.CurrentMagazineAmmunition >= 1)
        {
            weapon.StartCoroutine(WaitForShot(weapon));
        }
    }
    
    private IEnumerator WaitForShot(WeaponBehaviourScript weapon)
    {
        _canShoot = false;
        SpawnProjectile(weapon);
        yield return new WaitForSeconds(weapon.WeaponData.FireRate);
        weapon.CurrentMagazineAmmunition -= 1;
        _canShoot = true; 
    }

    public override void Reload(WeaponBehaviourScript weapon)
    {

        if (_canReload)
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
        //todo direction
        GameObject projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData, Vector2.right);
        projectile.SetActive(true);
    }
}