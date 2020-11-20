using System.Collections;
using MonoBehaviours.WeaponsSystem;
using UnityEngine;

namespace ScriptableObjects.WeaponsSystem.WeaponShootStrategies
{
    [CreateAssetMenu(fileName = "NewThrowableShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/Throwable", order = 6)]
    public class ThrowableShootStrategy : WeaponShootStrategy
    {
        private const bool DefaultCanShootValue = true;
        
        [SerializeField] private bool canShoot = true;
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
            yield return new WaitForAndWhile(() => Input.GetKeyUp(KeyCode.G), 1.0f / weapon.WeaponData.FireRate); //todo what if we change the keybinding?
            canShoot = true;
        }

        public override void Reload(WeaponBehaviourScript weapon) { }

        protected override void SpawnProjectile(WeaponBehaviourScript weapon)
        {
            var throwable = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab, weapon.WeaponShootLocation, Quaternion.identity);
            var throwableScript = throwable.GetComponent<ThrowableBehaviourScript>();
            if (throwableScript == null)
            {
                Destroy(throwable);
                throw new MissingComponentException("Required a ThrowableBehaviourScript Monobehaviour, but didn't find one!");
            }
            throwableScript.Init(weapon.WeaponData.ProjectileData, weapon.Direction);
        }
        
        private void Awake()
        {
            canShoot = DefaultCanShootValue;
        }

        private void OnEnable()
        {
            canShoot = DefaultCanShootValue;
        }

        private void OnDisable()
        {
            canShoot = DefaultCanShootValue;
        }

        private void OnDestroy()
        {
            canShoot = DefaultCanShootValue;
        }
    }
}