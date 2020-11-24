using System;
using EnemySystem.Monobehaviours;
using Player;
using UnityEngine;

namespace Interactables
{
    public class ExplosiveBarrelBehaviour : MonoBehaviour, IBreakable
    {
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
                    hit.GetComponent<EnemyController>()?.ReceiveDamage(damageAmount);
                }
                else if (hit.CompareTag("Player"))
                {
                    var playerController = hit.GetComponent<Controller>();
                    playerController.LoseHitPoints(damageAmount);
                }
                else
                {
                    if (!hit.gameObject.Equals(gameObject))
                    {
                        hit.GetComponent<IBreakable>()?.Break();
                    }
                }
            }
            Destroy(gameObject);
        }

        public void Break()
        {
            DoExplosion();
        }
    }
}