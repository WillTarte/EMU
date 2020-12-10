using System;
using EnemySystem.Monobehaviours;
using Player;
using UnityEngine;
using WeaponsSystem.MonoBehaviours;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "CyborgAttackStrategy",
        menuName = "ScriptableObjects/EnemyAttack/Cyborg", order = 3)]
    public class CyborgStrategy : EnemyAttackStrategy
    {
        private const float RangeOfMeleeAttack = 3f;
        private Vector2 _initialPosition = new Vector2(0, 0);
        private Vector2 _lastPosition = new Vector2(0, 0);
        private bool _onStart = true;
        private int _thirdFrame = 2;
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        public override void Attack(GameObject player, GameObject emu, int damageGiven)
        {
            // get the initial height at the third frame to let emu2 reach the floor if the collider
            // is a little bit in the air
            if (player != null && emu.GetComponent<BossController>().battleStarted())
            {
                if (_onStart && _thirdFrame == 0)
                {
                    _initialPosition = emu.transform.position;
                    _lastPosition = _initialPosition;
                    _onStart = false;
                }

                _thirdFrame--;

                SquashAttack(player, emu, damageGiven);
                ShootAttack(emu);
            }
        }

        private void SquashAttack(GameObject player, GameObject emu, int damageGiven)
        {
            if (player.transform != null &&
                Vector2.Distance(emu.transform.position, player.transform.position) < RangeOfMeleeAttack)
            {
                var playerController = player.GetComponent<Controller>();
                emu.gameObject.GetComponent<Animator>().SetBool(Attacking, true);
                playerController.LoseHitPoints(damageGiven);
            }
            else
            {
               emu.gameObject.GetComponent<Animator>().SetBool(Attacking, false);
            }
        }

        private void ShootAttack(GameObject emu)
        {
            var emuPos = emu.transform.position;
            
            var isImmobile = Math.Abs(_lastPosition.x - emuPos.x) < 0.1 &&
                             Math.Abs(_lastPosition.y - emuPos.y) < 0.1;
            var isGrounded = Math.Abs(_initialPosition.y - emuPos.y) < 1;

            if (isImmobile && isGrounded && !emu.GetComponent<BossController>().isMoving())
            {
                var weapon = emu.GetComponentInChildren<WeaponBehaviourScript>();
                if (weapon.CurrentMagazineAmmunition == 0)
                {
                    weapon.WeaponData.ShootStrategy.Reload(weapon);
                }
                emu.gameObject.GetComponent<Animator>().SetBool(Attacking, true);
                weapon.WeaponData.ShootStrategy.Shoot(weapon);
            }
            else
            {
                _lastPosition = emuPos;
                emu.gameObject.GetComponent<Animator>().SetBool(Attacking, false);
            }
        }
    }
}
