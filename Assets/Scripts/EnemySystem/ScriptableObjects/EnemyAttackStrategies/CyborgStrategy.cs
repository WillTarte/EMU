using System;
using Player;
using UnityEngine;
using WeaponsSystem.MonoBehaviours;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "CyborgAttackStrategy",
        menuName = "ScriptableObjects/EnemyAttack/Cyborg", order = 3)]
    public class CyborgStrategy : EnemyAttackStrategy
    {
        private const float RangeOfMeleeAttack = 2f;
        private Vector2 _initialPosition = new Vector2(0, 0);
        private Vector2 _lastPosition = new Vector2(0, 0);
        private bool _onStart = true;

        public override void Attack(GameObject player, GameObject emu, int damageGiven)
        {
            if (_onStart)
            {
                _initialPosition = emu.transform.position;
                _lastPosition = _initialPosition;
                _onStart = false;
            }
            
            SquashAttack(player, emu, damageGiven);
            ShootAttack(emu);
           
        }

        private void SquashAttack(GameObject player, GameObject emu, int damageGiven)
        {
            if (player.transform != null &&
                Vector2.Distance(emu.transform.position, player.transform.position) < RangeOfMeleeAttack)
            {
                var playerController = player.GetComponent<Controller>();
                //emu.gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
                playerController.LoseHitPoints(damageGiven);
            }
            else
            {
               // emu.gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
            }
        }

        private void ShootAttack(GameObject emu)
        {
            var emuPos = emu.transform.position;
            
            var isImmobile = Math.Abs(_lastPosition.x - emuPos.x) < 0.1 &&
                             Math.Abs(_lastPosition.y - emuPos.y) < 0.1;
            var isGrounded = Math.Abs(_initialPosition.y - emuPos.y) < 0.3;

            if (isImmobile && isGrounded)
            {
                var weapon = emu.GetComponentInChildren<WeaponBehaviourScript>();
                if (weapon.CurrentMagazineAmmunition == 0)
                {
                    weapon.WeaponData.ShootStrategy.Reload(weapon);
                }
                weapon.WeaponData.ShootStrategy.Shoot(weapon);
            }
            else
            {
                _lastPosition = emu.transform.position;
            }
        }
    }
}
