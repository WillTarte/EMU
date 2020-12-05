using Player;
using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "BabeAttackStrategy", 
        menuName = "ScriptableObjects/EnemyAttack/Babe", order = 3)]
    public class BabeAttackStrategy : EnemyAttackStrategy
    {
        private float rangeOfMeleeAttack = 1.5f;
        private float waitTime = 2;

        public override void Attack(GameObject player, GameObject emu, int damageGiven)
        {
            if (player.transform != null &&
                Vector2.Distance(emu.transform.position, player.transform.position) < rangeOfMeleeAttack)
            {
                var playerController = player.GetComponent<Controller>();
                emu.gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
                playerController.LoseHitPoints(damageGiven);
            }
            else
            {
                emu.gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
            }
        }
    }
}
