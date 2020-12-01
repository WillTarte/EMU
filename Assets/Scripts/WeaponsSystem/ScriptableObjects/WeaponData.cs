#pragma warning disable 0649

using UnityEngine;
using WeaponsSystem.ScriptableObjects.WeaponShootStrategies;

namespace WeaponsSystem.ScriptableObjects
{
    /// <summary>
    /// A weapon's data. Every WeaponData asset is a unique weapon's data.
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private WeaponName weaponName;
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
        [SerializeField] private AudioClip shootAudioClip;
        [SerializeField] private AudioClip reloadAudioClip;

        public WeaponData Copy()
        {
            var copied = CreateInstance<WeaponData>();
            copied.weaponName = weaponName;
            copied.magazineCapacity = magazineCapacity;
            copied.maxAmmunitionCount = maxAmmunitionCount;
            copied.fireRate = fireRate;
            copied.reloadTime = reloadTime;
            copied.projectileData = projectileData;
            copied.shootStrategy = Instantiate(shootStrategy);
            copied.onGroundSprite = onGroundSprite;
            copied.inInventorySprite = inInventorySprite;
            copied.shootAudioClip = shootAudioClip;
            copied.reloadAudioClip = reloadAudioClip;

            return copied;
        }
        
        public WeaponName WeaponName => weaponName;
        public int MagazineCapacity => magazineCapacity;
        public int MaxAmmunitionCount => maxAmmunitionCount;
        public float FireRate => fireRate;
        public float ReloadTime => reloadTime;
        public ProjectileData ProjectileData => projectileData;
        public WeaponShootStrategy ShootStrategy => shootStrategy;
        public Sprite OnGroundSprite => onGroundSprite;
        public Sprite InInventorySprite => inInventorySprite;
        public AudioClip ShootAudioClip => shootAudioClip;
        public AudioClip ReloadAudioClip => reloadAudioClip;
    }

    public enum WeaponName
    {
        Shotgun,
        AssaultRifle,
        Sniper,
        Knife,
        Grenade
    }
}

