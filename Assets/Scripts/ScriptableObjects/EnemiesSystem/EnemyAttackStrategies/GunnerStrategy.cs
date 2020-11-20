using System;
using MonoBehaviours.WeaponsSystem;
using ScriptableObjects.WeaponsSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.EnemiesSystem.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "GunnerAttackStrategy",
        menuName = "ScriptableObjects/EnemyAttack/Gunner", order = 3)]
    public class GunnerStrategy : EnemyAttackStrategy
    {
        #region Interface Variables

        [SerializeField] private float shootDistanceRangeMax = 50;
        [SerializeField] private float shootHeightRangeMax = 0.1f;

        #endregion

        /**
         * Shoot the target if they are a certain distance away
         * In this case, we don't need the hasCollided bool, so we will return false
         */
        public override bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided)
        {
            if (Math.Abs(emu.transform.position.x - player.transform.position.x) < shootDistanceRangeMax
                && Math.Abs(emu.transform.position.y - player.transform.position.y) < shootHeightRangeMax)
            {
                var weapon = emu.GetComponent<WeaponBehaviourScript>();
                emu.GetComponent<WeaponData>().ShootStrategy.Shoot(weapon);
            }

            return false;
        }
    }
}