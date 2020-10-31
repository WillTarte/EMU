using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Meant for weapons that shoot bursts of bullets at one time. (shot at once, or in a row) (E.g shotgun / other) 
/// </summary>
[CreateAssetMenu(fileName = "NewBurstWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/Burst", order = 1)]
public class BurstWeaponShootStrategy: WeaponShootStrategy
{
    private const bool DefaultCanShootValue = true;
    private const bool DefaultCanReloadValue = true;
    
    [SerializeField] private float spread;
    [SerializeField] private int numProjectiles;
    [SerializeField] private bool canReload = true;
    [SerializeField] private bool canShoot = true;

    public override void Shoot(WeaponBehaviourScript weapon)
    {
        if (canShoot && weapon.CurrentMagazineAmmunition >= numProjectiles)
        {
            weapon.StartCoroutine(WaitForShot(weapon));
        }
    }

    private IEnumerator WaitForShot(WeaponBehaviourScript weapon)
    {
            canShoot = false;
            canReload = false;
            SpawnProjectile(weapon);
            weapon.CurrentMagazineAmmunition -= numProjectiles;
            yield return new WaitForSeconds(1.0f / weapon.WeaponData.FireRate);
            canReload = true;
            canShoot = true; 
    }
    

    public override void Reload(WeaponBehaviourScript weapon)
    {
        if (canReload && weapon.CurrentMagazineAmmunition < weapon.WeaponData.MagazineCapacity && weapon.CurrentTotalAmmunition >= 1)
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
        //todo if spread is 0 (straight line) then bullets should have delay between each other
        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = -(spread / 2) + i * (spread / (numProjectiles - 1));
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
            GameObject projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.WeaponSpriteEndPosition, Quaternion.identity);
            Vector2 projDir =
                Vector2.ClampMagnitude(new Vector2(weapon.Direction.x, (float) Math.Tan(angle) * weapon.Direction.x),1.0f);
            projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData, projDir);
            projectile.SetActive(true);
        }
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