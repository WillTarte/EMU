#pragma warning disable 0649

using ScriptableObjects.WeaponsSystem.WeaponShootStrategies;
using UnityEngine;

namespace ScriptableObjects.WeaponsSystem
{
    /// <summary>
    /// A weapon's data. Every WeaponData asset is a unique weapon's data.
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private int magazineCapacity;
        [SerializeField] private int maxAmmunitionCount;
        /// <summary>
        /// Represents the number of projectiles fired per second
        /// </summary>
        [SerializeField] private float fireRate;
        [SerializeField] private float reloadTime;
        [SerializeField] private ProjectileData projectileData;
        [SerializeField] private WeaponShootStrategy shootStrategy;
        [SerializeField] private Sprite onGroundSprite;
        [SerializeField] private Sprite inInventorySprite;

        public int MagazineCapacity => magazineCapacity;
        public int MaxAmmunitionCount => maxAmmunitionCount;
        public float FireRate => fireRate;
        public float ReloadTime => reloadTime;
        public ProjectileData ProjectileData => projectileData;
        public WeaponShootStrategy ShootStrategy => shootStrategy;
        public Sprite OnGroundSprite => onGroundSprite;
        public Sprite InInventorySprite => inInventorySprite;
    }
}

