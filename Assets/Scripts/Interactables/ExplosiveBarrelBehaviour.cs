using System;
using Player;
using UnityEngine;

namespace Interactables
{
    public class ExplosiveBarrelBehaviour : MonoBehaviour, IBreakable
    {
        private const string TriggerName = "ExplosiveBarrelTrigger";
        
        [SerializeField] private int explosionRadius;
        [SerializeField] private int damageAmount;

        private GameObject _trigger;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                DoExplosion();
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Projectile"))
            {
                Destroy(other.gameObject);
                DoExplosion();
            }
        }
        
        private void DoExplosion()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    // todo: do damage to enemy
                }
                else if (hit.CompareTag("Player"))
                {
                    var playerController = hit.GetComponent<Controller>();
                    playerController.LoseHitPoints(damageAmount);
                }
                // todo: interacts with breakable things
            }
            Destroy(gameObject);
        }

        public void Break()
        {
            DoExplosion();
        }
    }
}