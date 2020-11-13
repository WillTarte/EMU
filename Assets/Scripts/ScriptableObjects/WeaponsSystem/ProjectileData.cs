using UnityEngine;

namespace ScriptableObjects.WeaponsSystem
{
    /// <summary>
    /// Represents a projectile's data. Every ProjectileData asset is a unique projectile type's data.
    /// </summary>
    [CreateAssetMenu(fileName = "NewProjectileData", menuName = "ScriptableObjects/ProjectileData", order = 2)]
    public class ProjectileData : ScriptableObject
    {
        //todo particle effect when colliding
        [SerializeField] private Sprite projectileSprite;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private int projectileBaseDamage;
        [SerializeField] private GameObject projectilePrefab;

        public Sprite ProjectileSprite => projectileSprite;
        public float ProjectileSpeed => projectileSpeed;
        public int ProjectileBaseDamage => projectileBaseDamage;
        public GameObject ProjectilePrefab => projectilePrefab;
    }
}
