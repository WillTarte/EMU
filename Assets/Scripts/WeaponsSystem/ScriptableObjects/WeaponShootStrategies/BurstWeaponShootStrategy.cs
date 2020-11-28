using System;
using System.Collections;
using UnityEngine;
using WeaponsSystem.MonoBehaviours;

namespace WeaponsSystem.ScriptableObjects.WeaponShootStrategies
{
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
            if (canShoot && weapon.CurrentMagazineAmmunition >= 1)
            {
                weapon.StartCoroutine(WaitForShot(weapon));
                weapon.GetComponent<AudioSource>()?.PlayOneShot(weapon.WeaponData.ShootAudioClip);
            }
        }

        private IEnumerator WaitForShot(WeaponBehaviourScript weapon)
        {
            canShoot = false;
            canReload = false;
            SpawnProjectile(weapon);
            weapon.CurrentMagazineAmmunition -= 1;
            yield return new WaitForAndWhile(() => Input.GetKeyUp(KeyCode.K), 1.0f / weapon.WeaponData.FireRate);
            canReload = true;
            canShoot = true; 
        }
    

        public override void Reload(WeaponBehaviourScript weapon)
        {
            if (canReload && weapon.CurrentMagazineAmmunition < weapon.WeaponData.MagazineCapacity && weapon.CurrentTotalAmmunition >= 1)
            {
                weapon.StartCoroutine(WaitForReload(weapon));
                weapon.GetComponent<AudioSource>()?.PlayOneShot(weapon.WeaponData.ReloadAudioClip);
            }
        }

        private IEnumerator WaitForReload(WeaponBehaviourScript weapon)
        {
            canReload = false;
            canShoot = false;
            int reloadAmount = weapon.WeaponData.MagazineCapacity - weapon.CurrentMagazineAmmunition;
            reloadAmount = Math.Min(reloadAmount, weapon.CurrentTotalAmmunition);
            yield return new WaitForSeconds(weapon.WeaponData.ReloadTime);
            weapon.CurrentTotalAmmunition -= reloadAmount;
            weapon.CurrentMagazineAmmunition += reloadAmount;
            canShoot = true;
            canReload = true;
        }

        protected override void SpawnProjectile(WeaponBehaviourScript weapon)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                var angle = -(spread / 2) + i * (spread / (numProjectiles - 1));
                angle = (float) (Math.PI / 180) * angle;
                var projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.WeaponShootLocation, Quaternion.identity);
                var projDir =
                    Vector2.ClampMagnitude(new Vector2(weapon.Direction.x, (float) Math.Tan(angle) * weapon.Direction.x),1.0f);
                projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData, projDir, !weapon.transform.parent.CompareTag("Player"));
                projectile.SetActive(true);
            }
        }
    
        private void Awake()
        {
            canShoot = DefaultCanShootValue;
            canReload = DefaultCanReloadValue;
        }

        private void OnEnable()
        {
            canShoot = DefaultCanShootValue;
            canReload = DefaultCanReloadValue;
        }

        private void OnDisable()
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