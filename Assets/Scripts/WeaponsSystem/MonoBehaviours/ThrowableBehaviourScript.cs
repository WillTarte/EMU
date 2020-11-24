using System.Collections;
using EnemySystem.Monobehaviours;
using Interactables;
using Player;
using UnityEngine;
using WeaponsSystem.ScriptableObjects;

namespace WeaponsSystem.MonoBehaviours
{
    /// <summary>
    /// This Monobehaviour models the behaviour of a projectile that is thrown (i.e. physics)
    /// </summary>
    public class ThrowableBehaviourScript : MonoBehaviour
    {
        [SerializeField] private float aliveTime;
        [SerializeField] private int explosionRadius;
        [SerializeField] private Vector2 force;

        private Vector2 _direction;
        private ProjectileData _projectileData;
        private Rigidbody2D _rigidbody;
        private CircleCollider2D _circleCollider;
        private SpriteRenderer _spriteRenderer;
        private bool _forceApplied = false;

        public void Init(ProjectileData projectileData, Vector2 direction)
        {
            _projectileData = projectileData;
            _direction = direction;
            
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _spriteRenderer.sprite = _projectileData.ProjectileSprite;
            _circleCollider.radius = _spriteRenderer.sprite.bounds.extents.x * 1.5f;
        }

        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (!_forceApplied)
            {
                _forceApplied = true;
                force.x *= _direction.x;
                _rigidbody.AddForce(force, ForceMode2D.Impulse);
                StartCoroutine(WaitForExplosion());
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                DoExplosion();
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

        private IEnumerator WaitForExplosion()
        {
            yield return new WaitForSeconds(aliveTime);
            DoExplosion();
        }

        private void DoExplosion()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    hit.gameObject.GetComponent<EnemyController>()?.ReceiveDamage(_projectileData.ProjectileBaseDamage);
                }
                else if (hit.CompareTag("Player"))
                {
                    var playerController = hit.GetComponent<Controller>();
                    playerController.LoseHitPoints(_projectileData.ProjectileBaseDamage);
                }
                else
                {
                    hit.GetComponent<IBreakable>()?.Break();
                }
            }
            Destroy(gameObject);
        }
    }
}