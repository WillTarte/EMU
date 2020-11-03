using UnityEngine;

namespace ScriptableObjects.WeaponsSystem
{
    /// <summary>
    /// Represents a projectile's data. Every ProjectileData asset is a unique projectile type's data.
    /// </summary>
    [CreateAssetMenu(fileName = "NewProjectileData", menuName = "ScriptableObjects/ProjectileData", order = 2)]
    public class ProjectileData : ScriptableObject
    {
        [SerializeField] private Sprite projectileSprite;
        [SerializeField] private Sprite destroyedSprite; //todo particle effect
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileBaseDamage;
        [SerializeField] private bool isKinematic;
        [SerializeField] private GameObject projectilePrefab;

        public Sprite ProjectileSprite => projectileSprite;
        public Sprite DestroyedSprite => destroyedSprite;
        public float ProjectileSpeed => projectileSpeed;
        public float ProjectileBaseDamage => projectileBaseDamage;
        public bool IsKinematic => isKinematic;
        public GameObject ProjectilePrefab => projectilePrefab;
    }
}
