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
    [SerializeField] private float fireRate;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private FireModeType fireMode;
    [SerializeField] private Sprite onGroundSprite;
    [SerializeField] private Sprite inInventorySprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    public int MagazineCapacity => magazineCapacity;
    public int MaxAmmunitionCount => maxAmmunitionCount;
    public float FireRate => fireRate;
    public ProjectileData ProjectileData => projectileData;
    public FireModeType FireMode => fireMode;
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

