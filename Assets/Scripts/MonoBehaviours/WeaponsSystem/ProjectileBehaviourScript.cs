using System.Collections;
using ScriptableObjects.WeaponsSystem;
using UnityEngine;

namespace MonoBehaviours.WeaponsSystem
{
    /// <summary>
    /// Controls the behavior of a projectile
    /// </summary>
    public class ProjectileBehaviourScript : MonoBehaviour
    {
        [SerializeField] private int aliveTime;
    
        private ProjectileData _projectileData;
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        ///  Initializes parameters of the script. MAKE SURE THE ProjectileData instance IS KINEMATIC
        /// </summary>
        /// <param name="projectileData"></param> The projectile's data (Scriptable object instance)
        /// <param name="spawnDirection"></param> The direction of the projectile (right or left)
        public void Init(ProjectileData projectileData, Vector2 spawnDirection)
        {
            _projectileData = projectileData;
            _direction = spawnDirection;
        
            _rigidbody.isKinematic = _projectileData.IsKinematic;
            _spriteRenderer.sprite = _projectileData.ProjectileSprite;
        }
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (!_projectileData.IsKinematic)
            {
                _rigidbody.AddForce(_projectileData.ProjectileSpeed * _direction, ForceMode2D.Impulse);
            }

            StartCoroutine(WaitForDestroy());
        }

        private void Update()
        {
            if (_projectileData.IsKinematic)
            {
                transform.Translate(_projectileData.ProjectileSpeed * Time.deltaTime * _direction);
            }
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Projectile " + _projectileData.name + " Collided with " + other.name);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

        private IEnumerator WaitForDestroy()
        {
            yield return new WaitForSeconds(aliveTime);
            Destroy(gameObject);
        }
    }
}