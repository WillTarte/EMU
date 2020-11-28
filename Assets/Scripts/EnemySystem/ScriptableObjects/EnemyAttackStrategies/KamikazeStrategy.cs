using EnemySystem.Monobehaviours;
using Interactables;
using Player;
using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    [CreateAssetMenu(fileName = "KamikazeAttackStrategy", 
        menuName = "ScriptableObjects/EnemyAttack/kamikaze", order = 3)]
    public class KamikazeStrategy : EnemyAttackStrategy
    {

        [SerializeField] private GameObject bloodEffect;
        [SerializeField] private int explosionRadius;

        public override bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided)
        {
            if (player.CompareTag("Player") && hasCollided)
            {
                DoExplosion(emu, damageGiven);
                Instantiate(bloodEffect, emu.transform.position, emu.transform.rotation);
                return false;
            }

            return hasCollided;
        }
        
        private void DoExplosion(GameObject emu, int damageGiven)
        {
            var hits = Physics2D.OverlapCircleAll(emu.transform.position, explosionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    hit.gameObject.GetComponent<EnemyController>()?.ReceiveDamage(damageGiven);
                }
                else if (hit.CompareTag("Player"))
                {
                    var playerController = hit.GetComponent<Controller>();
                    playerController.LoseHitPoints(damageGiven);
                }
                else
                {
                    hit.GetComponent<IBreakable>()?.Break();
                }
            }
            Destroy(emu);
        }
    }
}
