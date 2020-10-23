using System.Collections;
using UnityEngine;

/// <summary>
/// Meant for weapons that can only shoot 1 bullet per click. (E.g. sniper)
/// </summary>
[CreateAssetMenu(fileName = "NewSemiAutoWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/SemiAuto", order = 3)]
public class SemiAutoWeaponShootStrategy: WeaponShootStrategy
{
    private const bool DefaultCanShootValue = true;
    private const bool DefaultCanReloadValue = true;
    
    [SerializeField] private bool canReload = true;
    [SerializeField] private bool canShoot = true;
    
    // TODO: fix the behavior
    public override void Shoot(WeaponBehaviourScript weapon)
    {
        if (canShoot && weapon.CurrentMagazineAmmunition >= 1)
        {
            weapon.StartCoroutine(WaitForShot(weapon));
        }
    }
    
    private IEnumerator WaitForShot(WeaponBehaviourScript weapon)
    {
        canShoot = false;
        SpawnProjectile(weapon);
        weapon.CurrentMagazineAmmunition -= 1;
        yield return new WaitForSeconds(1.0f / weapon.WeaponData.FireRate);
        canShoot = true; 
    }

    public override void Reload(WeaponBehaviourScript weapon)
    {

        if (canReload)
        {
            weapon.StartCoroutine(WaitForReload(weapon));
        }
    }
    
    private IEnumerator WaitForReload(WeaponBehaviourScript weapon)
    {
        canReload = false;
        canShoot = false;
        int reloadAmount =
            weapon.CurrentTotalAmmunition < weapon.WeaponData.MagazineCapacity - weapon.CurrentMagazineAmmunition
                ? weapon.CurrentTotalAmmunition
                : weapon.WeaponData.MagazineCapacity - weapon.CurrentMagazineAmmunition;
        yield return new WaitForSeconds(weapon.WeaponData.ReloadTime);
        weapon.CurrentTotalAmmunition -= reloadAmount;
        weapon.CurrentMagazineAmmunition += reloadAmount;
        canShoot = true;
        canReload = true;
    }
    
    protected override void SpawnProjectile(WeaponBehaviourScript weapon)
    {
        //todo direction
        GameObject projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData, Vector2.right);
        projectile.SetActive(true);
    }
    
    private void Awake()
    {
        canShoot = DefaultCanShootValue;
        canReload = DefaultCanReloadValue;
    }

    private void OnDestroy()
    {
        canShoot = DefaultCanShootValue;
        canReload = DefaultCanReloadValue;
    }
}