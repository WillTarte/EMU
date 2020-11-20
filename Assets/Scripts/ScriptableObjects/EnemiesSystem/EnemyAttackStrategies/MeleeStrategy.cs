using UnityEngine;
using Player;

namespace ScriptableObjects.EnemiesSystem.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "NewMeleeAttackStrategy", menuName = "ScriptableObjects/EnemyAttack/Melee", order = 3)]
    public class MeleeStrategy : EnemyAttackStrategy
    {
        public override void Attack(Collider2D other, int damageGiven)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var playerController = other.GetComponent<Controller>();
                playerController.LoseHitPoints(damageGiven);
                Debug.Log("Dealt damage");
            }
        }
    }
}
