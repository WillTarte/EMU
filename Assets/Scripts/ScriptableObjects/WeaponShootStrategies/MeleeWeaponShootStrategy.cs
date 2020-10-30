using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeaponShootStrategy", menuName = "ScriptableObjects/WeaponShootStrategy/Melee", order = 5)]
public class MeleeWeaponShootStrategy : WeaponShootStrategy
{
    private const bool DefaultCanAttackValue = true;
    
    [SerializeField] private MeleeShapeType meleeShapeType;
    [SerializeField] private float attackRate;
    [SerializeField] private bool canAttack;

    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 direction = Vector2.zero;
    [SerializeField] private float distance = 0.0f;
    [SerializeField] private float angle;
    [SerializeField] private float radius;

    private Func<Vector2, Vector2, float, Vector2, RaycastHit2D[]> _boxCast = (o, s, a, d) => Physics2D.BoxCastAll(o, s, a, d);
    private Func<Vector2, float, Vector2, RaycastHit2D[]> _circleCast = (o, r, d) => Physics2D.CircleCastAll(o, r, d);
    private Func<Vector2, Vector2, RaycastHit2D[]> _rayCast = (o, d) => Physics2D.RaycastAll(o, d);
    private Func<Vector2, Vector2, float, Collider2D[]> _overlapBox = (o, s, a) => Physics2D.OverlapBoxAll(o, s, a);
    private Func<Vector2, float, Collider2D[]> _overlapCircle = (o, r) => Physics2D.OverlapCircleAll(o, r);
        
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
        switch (meleeShapeType)
        {
            case MeleeShapeType.BoxCast:
                ProcessRayCastHits(Physics2D.BoxCastAll(weapon.transform.position, size, angle, direction.Equals(Vector2.zero) ? weapon.Direction : direction, distance == 0.0f ? Mathf.Infinity : distance));
                break;
            case MeleeShapeType.CircleCast:
                ProcessRayCastHits(Physics2D.CircleCastAll(weapon.transform.position, radius, direction.Equals(Vector2.zero) ? weapon.Direction : direction, distance == 0.0f ? Mathf.Infinity : distance));
                break;
            case MeleeShapeType.RayCast:
                ProcessRayCastHits(Physics2D.RaycastAll(weapon.transform.position, direction.Equals(Vector2.zero) ? weapon.Direction : direction, distance == 0.0f ? Mathf.Infinity : distance));
                break;
            case MeleeShapeType.OverlapCircle:
                ProcessColliderHits(Physics2D.OverlapCircleAll(weapon.transform.position, radius));
                break;
            case MeleeShapeType.OverlapBox:
                ProcessColliderHits(Physics2D.OverlapBoxAll(weapon.transform.position, size, angle));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        yield return new WaitForSeconds(1.0f / attackRate);
        canAttack = true;
    }

    private static void ProcessRayCastHits(IEnumerable<RaycastHit2D> raycastHits)
    {
        //todo
        foreach (var hit in raycastHits)
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                // Make the enemy take damage
            }
            else if (hit.transform.gameObject.CompareTag("Breakable"))
            {
                // make the other break/take damage
            }
            else
            {
            }
        }
    }

    private static void ProcessColliderHits(IEnumerable<Collider2D> colliderHits)
    {
        //todo
        foreach (var hit in colliderHits)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                // Make the enemy take damage
            }
            else if (hit.gameObject.CompareTag("Breakable"))
            {
                // make the other break/take damage
            }
            else
            {
            }
        }
    }
    
    private void Awake()
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