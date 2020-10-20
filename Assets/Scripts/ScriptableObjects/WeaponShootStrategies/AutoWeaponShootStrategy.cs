using System.Collections;
using UnityEngine;

/// <summary>
/// Meant for weapons that you can keep shooting as long as you have ammo. (Ex. ak47)
/// </summary>
[CreateAssetMenu(fileName = "NewAutoWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/Auto", order = 2)]
public class AutoWeaponShootStrategy: WeaponShootStrategy
{
    private bool _canReload = true;
    private bool _canShoot = true;
    public override void Shoot(WeaponBehaviourScript weapon)
    {
        if (_canShoot && weapon.CurrentMagazineAmmunition > 0)
        {
            //todo is this the right way to do full auto
            weapon.StartCoroutine(WaitForShot(weapon));
        }
    }
    
    private IEnumerator WaitForShot(WeaponBehaviourScript weapon)
    {
        SpawnProjectile(weapon);
        weapon.CurrentMagazineAmmunition -= 1;
        yield return new WaitForSeconds(weapon.WeaponData.FireRate);
    }

    public override void Reload(WeaponBehaviourScript weapon)
    {
        if (_canReload && weapon.CurrentTotalAmmunition >= 1 && weapon.CurrentMagazineAmmunition < weapon.WeaponData.MagazineCapacity)
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