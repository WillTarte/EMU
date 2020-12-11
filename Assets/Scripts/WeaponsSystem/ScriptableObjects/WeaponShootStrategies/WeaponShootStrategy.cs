using UnityEngine;
using WeaponsSystem.MonoBehaviours;

namespace WeaponsSystem.ScriptableObjects.WeaponShootStrategies
{
    public abstract class WeaponShootStrategy : ScriptableObject, IWeaponShootStrategy
    {

        public abstract void Shoot(WeaponBehaviourScript weapon);
        public abstract void Reload(WeaponBehaviourScript weapon);

        protected virtual void SpawnProjectile(WeaponBehaviourScript weapon)
        {
            var projectile = Instantiate(weapon.WeaponData.ProjectileData.ProjectilePrefab,
                weapon.WeaponShootLocation, Quaternion.identity);

            // Make sure the weapon has a parent gameobject of this line is gonna cause a Nullptrexception
            projectile.GetComponent<ProjectileBehaviourScript>().Init(weapon.WeaponData.ProjectileData,
                weapon.Direction, !weapon.transform.parent.CompareTag("Player"));
            projectile.SetActive(true);
        }
    }
}