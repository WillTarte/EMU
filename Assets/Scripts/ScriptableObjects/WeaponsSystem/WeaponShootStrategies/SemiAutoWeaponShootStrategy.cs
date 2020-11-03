using System.Collections;
using MonoBehaviours.WeaponsSystem;
using UnityEngine;

namespace ScriptableObjects.WeaponsSystem.WeaponShootStrategies
{
    /// <summary>
    /// Meant for weapons that can only shoot 1 bullet per click. (E.g. sniper)
    /// </summary>
    [CreateAssetMenu(fileName = "NewSemiAutoWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/SemiAuto", order = 3)]
    public class SemiAutoWeaponShootStrategy: WeaponShootStrategy
    {
        private const bool DefaultCanShootValue = true;
        private const bool DefaultCanReloadValue = true;
        private const bool DefaultPressedValue = false;
    
        [SerializeField] private bool canReload = true;
        [SerializeField] private bool canShoot = true;
        [SerializeField] private bool pressed = false;
    
        public override void Shoot(WeaponBehaviourScript weapon)
        {
            if (canShoot && !pressed && weapon.CurrentMagazineAmmunition >= 1)
            {
                pressed = true;
                weapon.StartCoroutine(WaitForShot(weapon));
                weapon.StartCoroutine(WaitForRelease());
            }
        }

        private IEnumerator WaitForRelease()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            pressed = false;
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
            GameObject projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.WeaponSpriteEndPosition, Quaternion.identity);
            projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData, weapon.Direction);
            projectile.SetActive(true);
        }
    
        private void Awake()
        {
            canShoot = DefaultCanShootValue;
            canReload = DefaultCanReloadValue;
            pressed = DefaultPressedValue;
        }

        private void OnDestroy()
        {
            canShoot = DefaultCanShootValue;
            canReload = DefaultCanReloadValue;
            pressed = DefaultPressedValue;
        }
    }
}