#pragma warning disable 0649

using UnityEngine;

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
    [SerializeField] private FireModeType fireMode; // Do we need this? (Defined by the strategy)
    [SerializeField] private WeaponShootStrategy shootStrategy;
    [SerializeField] private Sprite onGroundSprite;
    [SerializeField] private Sprite inInventorySprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    public int MagazineCapacity => magazineCapacity;
    public int MaxAmmunitionCount => maxAmmunitionCount;
    public float FireRate => fireRate;
    public float ReloadTime => reloadTime;
    public ProjectileData ProjectileData => projectileData;
    public FireModeType FireMode => fireMode;
    public WeaponShootStrategy ShootStrategy => shootStrategy;
    public Sprite OnGroundSprite => onGroundSprite;
    public Sprite InInventorySprite => inInventorySprite;
    public Sprite InactiveSprite => inactiveSprite;
    public Sprite ActiveSprite => activeSprite;

    public enum FireModeType
    {
        Automatic,
        SemiAutomatic,
        Burst,
        Single
    }
}

