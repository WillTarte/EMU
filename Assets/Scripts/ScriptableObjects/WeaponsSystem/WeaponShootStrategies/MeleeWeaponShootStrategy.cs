using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MonoBehaviours.WeaponsSystem;
using Player;
using UnityEngine;
using Color = UnityEngine.Color;

namespace ScriptableObjects.WeaponsSystem.WeaponShootStrategies
{
    // TODO: when we have animations, tweak the values of every instance
    // TODO: when we have more layers/tags to check, some refactor needed
    [CreateAssetMenu(fileName = "NewMeleeWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/Melee", order = 5)]
    public class MeleeWeaponShootStrategy : WeaponShootStrategy
    {
        private const bool DefaultCanAttackValue = true;
    
        [SerializeField] private MeleeShapeType meleeShapeType;
        [SerializeField] private float attackRate;
        [SerializeField] private int baseAttackDamage;
        [SerializeField] private bool canAttack;

        [SerializeField] private Vector2 size;
        [SerializeField] private Vector2 direction = Vector2.zero;
        [SerializeField] private float distance = 0.0f;
        [SerializeField] private float angle;
        [SerializeField] private float radius;

        public override void Shoot(WeaponBehaviourScript weapon)
        {
            if (canAttack)
            {
                weapon.StartCoroutine(WaitForAttack(weapon));
            }
        }

        private IEnumerator WaitForAttack(WeaponBehaviourScript weapon)
        {
            canAttack = false;
            int layerMask = GetLayerMask(weapon);
            switch (meleeShapeType)
            {
                case MeleeShapeType.BoxCast:
                    ProcessRayCastHits(Physics2D.BoxCastAll(weapon.WeaponShootLocation, size, angle, direction.Equals(Vector2.zero) ? weapon.Direction : new Vector2(direction.x * weapon.Direction.x, direction.y), distance == 0.0f ? Mathf.Infinity : distance, layerMask), weapon);
                    break;
                case MeleeShapeType.CircleCast:
                    ProcessRayCastHits(Physics2D.CircleCastAll(weapon.WeaponShootLocation, radius, direction.Equals(Vector2.zero) ? weapon.Direction : direction, distance == 0.0f ? Mathf.Infinity : distance, layerMask), weapon);
                    break;
                case MeleeShapeType.RayCast:
                    ProcessRayCastHits(Physics2D.RaycastAll(weapon.WeaponShootLocation, direction.Equals(Vector2.zero) ? weapon.Direction : direction, distance == 0.0f ? Mathf.Infinity : distance, layerMask), weapon);
                    break;
                case MeleeShapeType.OverlapCircle:
                    ProcessColliderHits(Physics2D.OverlapCircleAll(weapon.WeaponShootLocation, radius, layerMask), weapon);
                    break;
                case MeleeShapeType.OverlapBox:
                    ProcessColliderHits(Physics2D.OverlapBoxAll(weapon.WeaponShootLocation, size, angle, layerMask), weapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield return new WaitForSeconds(1.0f / attackRate);
            canAttack = true;
        }

        private void ProcessRayCastHits(IEnumerable<RaycastHit2D> raycastHits, WeaponBehaviourScript weapon)
        {
            foreach (var hit in raycastHits)
            {
                Debug.Log("Hit " + hit.transform.gameObject.name);
                
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    // todo Make the enemy take damage
                }
                else if (hit.transform.gameObject.CompareTag("Breakable"))
                {
                    // todo make the other break/take damage
                }
                else if (hit.transform.gameObject.CompareTag("Player"))
                {
                    var playerController = hit.transform.gameObject.GetComponent<Controller>();
                    playerController.LoseHitPoints(baseAttackDamage);
                }
            }
        }

        private void ProcessColliderHits(IEnumerable<Collider2D> colliderHits, WeaponBehaviourScript weapon)
        {
            foreach (var hit in colliderHits)
            {
                Debug.Log("Hit " + hit.gameObject.name);
                
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    //todo Make the enemy take damage
                }
                else if (hit.gameObject.CompareTag("Breakable"))
                {
                    // todo make the other break/take damage
                }
                else if (hit.transform.gameObject.CompareTag("Player"))
                {
                    var playerController = hit.transform.gameObject.GetComponent<Controller>();
                    playerController.LoseHitPoints(baseAttackDamage);
                }
            }
        }
        
        private static int GetLayerMask(WeaponBehaviourScript weapon)
        {
            var weaponOwnerLayer = LayerMask.LayerToName(weapon.transform.parent.gameObject.layer);
            var layersToSkip = new[] {weaponOwnerLayer};
            return ~LayerMask.GetMask(layersToSkip);
        }
        
        private void Awake()
        {
            canAttack = DefaultCanAttackValue;
        }

        private void OnEnable()
        {
            canAttack = DefaultCanAttackValue;
        }

        private void OnDisable()
        {
            canAttack = DefaultCanAttackValue;
        }

        private void OnDestroy()
        {
            canAttack = DefaultCanAttackValue;
        }
    
        public override void Reload(WeaponBehaviourScript weapon)
        {
        }

        protected override void SpawnProjectile(WeaponBehaviourScript weapon)
        {
        }

        private enum MeleeShapeType
        {
            BoxCast,
            CircleCast,
            RayCast,
            OverlapCircle,
            OverlapBox
        }
    }
}