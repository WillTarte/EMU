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

        [SerializeField] private GameObject explosionEffect;
        [SerializeField] private int explosionRadius;
        
        private float rangeOfMeleeAttack = 1.5f;

        public override void Attack(GameObject player, GameObject emu, int damageGiven)
        {
            if (player.CompareTag("Player") &&
                Vector2.Distance(emu.transform.position, player.transform.position) < rangeOfMeleeAttack)
            {
                DoExplosion(emu, damageGiven);
                var blood = Instantiate(explosionEffect, emu.transform.position, emu.transform.rotation);
                Destroy(blood, 0.5f);
            }
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
