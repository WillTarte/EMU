using UnityEngine;
using Player;

namespace ScriptableObjects.EnemiesSystem.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "NewMeleeAttackStrategy", 
        menuName = "ScriptableObjects/EnemyAttack/Melee", order = 3)]
    public class MeleeStrategy : EnemyAttackStrategy
    {
        public override bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided)
        {
            if (player.CompareTag("Player") && hasCollided)
            {
                var playerController = player.GetComponent<Controller>();
                playerController.LoseHitPoints(damageGiven);
                //Debug.Log("Dealt damage");
                return false;
            }

            return hasCollided;
        }
    }
}
