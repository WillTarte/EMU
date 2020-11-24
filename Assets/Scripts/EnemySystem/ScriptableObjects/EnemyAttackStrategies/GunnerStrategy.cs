using System;
using UnityEngine;
using WeaponsSystem.MonoBehaviours;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "GunnerAttackStrategy",
        menuName = "ScriptableObjects/EnemyAttack/Gunner", order = 3)]
    public class GunnerStrategy : EnemyAttackStrategy
    {
        #region Interface Variables

        [SerializeField] private float shootDistanceRangeMax = 25;
        [SerializeField] private float shootHeightRangeMax = 0.1f;

        #endregion

        /**
         * Shoot the target if they are a certain distance away
         * In this case, we don't need the hasCollided bool, so we will return false
         */
        public override bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided)
        {
            if ((Math.Abs(emu.transform.position.x) - Math.Abs(player.transform.position.x) < shootDistanceRangeMax)
                && (Math.Abs(emu.transform.position.y) - Math.Abs(player.transform.position.y) < shootHeightRangeMax))
            {
                var weapon = emu.GetComponentInChildren<WeaponBehaviourScript>();
                weapon.WeaponData.ShootStrategy.Shoot(weapon);
            }

            return false;
        }
    }
}