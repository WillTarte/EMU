using System.Collections;
using MonoBehaviours.WeaponsSystem;
using UnityEngine;

namespace ScriptableObjects.WeaponsSystem.WeaponShootStrategies
{
    /// <summary>
    /// Meant for weapons that can only shoot once before reloading (E.g rpg)
    /// </summary>
    [CreateAssetMenu(fileName = "NewSingleWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/Single", order = 4)]
    public class SingleWeaponShootStrategy : WeaponShootStrategy
    {
        private const bool DefaultCanShootValue = true;
        private const bool DefaultCanReloadValue = true;
    
        [SerializeField] private bool canReload = true;
        [SerializeField] private bool canShoot = true;
        public override void Shoot(WeaponBehaviourScript weapon)
        {
            if (canShoot && weapon.CurrentMagazineAmmunition >= 0)
            {
                canShoot = false;
                weapon.StartCoroutine(WaitForShot(weapon));
            }
        }

        private IEnumerator WaitForShot(WeaponBehaviourScript weapon)
        {
            canShoot = false;
            canReload = false;
            SpawnProjectile(weapon);
            weapon.CurrentMagazineAmmunition = 0;
            yield return new WaitForSeconds(0.1f);
            canReload = true;
        }
    
        public override void Reload(WeaponBehaviourScript weapon)
        {
            if (canReload && weapon.CurrentMagazineAmmunition <= 0 && weapon.CurrentTotalAmmunition >= 1)
            {
                weapon.StartCoroutine(WaitForReload(weapon));
            }
        }

        private IEnumerator WaitForReload(WeaponBehaviourScript weapon)
        {
            canReload = false;
            canShoot = false;
            yield return new WaitForSeconds(weapon.WeaponData.ReloadTime);
            weapon.CurrentMagazineAmmunition = 1;
            weapon.CurrentTotalAmmunition -= 1;
            canShoot = true;
            canReload = true;
        }
    
        protected override void SpawnProjectile(WeaponBehaviourScript weapon)
        {
            GameObject projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.WeaponSpriteEndPosition, Quaternion.identity);
            projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData, weapon.Direction);
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
}