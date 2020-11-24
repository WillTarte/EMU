using Player;
using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "KamikazeAttackStrategy", 
        menuName = "ScriptableObjects/EnemyAttack/kamikaze", order = 3)]
    public class KamikazeStrategy : EnemyAttackStrategy
    {

        [SerializeField] private GameObject bloodEffect;
        
        public override bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided)
        {
            if (player.CompareTag("Player") && hasCollided)
            {
                var playerController = player.GetComponent<Controller>();
                playerController.LoseHitPoints(damageGiven);
                Instantiate(bloodEffect, emu.transform.position, emu.transform.rotation);
                Destroy(emu);
                return false;
            }

            return hasCollided;
        }
    }
}
