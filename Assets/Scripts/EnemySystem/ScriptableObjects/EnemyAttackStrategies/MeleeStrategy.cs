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
        
        public override bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided)
        {
            if (player.transform != null &&
                Vector2.Distance(emu.transform.position, player.transform.position) < rangeOfMeleeAttack)
            {
                var playerController = player.GetComponent<Controller>();
                playerController.LoseHitPoints(damageGiven);
                return false;
            }

            return hasCollided;
        }
    }
}

// public override void Move(Transform emuTransform, Transform playerTransform)
// {
// if (playerTransform != null &&
// (Vector2.Distance(emuTransform.position, playerTransform.position) < followRange  ||
// emuTransform.gameObject.GetComponent<EnemyController>().gotHit()))
// {
//     emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
//     emuTransform.position =
//         Vector2.MoveTowards(emuTransform.position, playerTransform.position, followSpeed * Time.deltaTime);
// }
// else
// {
//     emuTransform.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
// }
// }
