using System.Collections;
using ScriptableObjects.WeaponsSystem;
using UnityEngine;

namespace MonoBehaviours.WeaponsSystem
{
    /// <summary>
    /// Controls the behavior of a projectile
    /// </summary>
    // todo: Environment needs to be on different layers, and projectiles only interact with some of the environment layers
    public class ProjectileBehaviourScript : MonoBehaviour
    {
        [SerializeField] private int aliveTime;
    
        private ProjectileData _projectileData;
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        ///  Initializes parameters of the script.
        /// </summary>
        /// <param name="projectileData"></param> The projectile's data (Scriptable object instance)
        /// <param name="spawnDirection"></param> The direction of the projectile (right or left)
        public void Init(ProjectileData projectileData, Vector2 spawnDirection)
        {
            _projectileData = projectileData;
            _direction = spawnDirection;

            _rigidbody.isKinematic = true;
            _spriteRenderer.sprite = _projectileData.ProjectileSprite;
        }
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(WaitForDestroy());
        }

        private void Update()
        {
            transform.Translate(_projectileData.ProjectileSpeed * Time.deltaTime * _direction);
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