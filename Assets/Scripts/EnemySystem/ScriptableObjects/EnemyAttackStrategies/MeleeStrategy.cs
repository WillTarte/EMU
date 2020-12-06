using Player;
using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "NewMeleeAttackStrategy", 
        menuName = "ScriptableObjects/EnemyAttack/Melee", order = 3)]
    public class MeleeStrategy : EnemyAttackStrategy
    {
        private float rangeOfMeleeAttack = 1.5f;
        private float waitTime = 2;
        
        public override void Attack(GameObject player, GameObject emu, int damageGiven)
        {
            if (player.transform != null &&
                Vector2.Distance(emu.transform.position, player.transform.position) < rangeOfMeleeAttack)
            {
                var playerController = player.GetComponent<Controller>();
                playerController.LoseHitPoints(damageGiven);
            }
        }
    }
}
