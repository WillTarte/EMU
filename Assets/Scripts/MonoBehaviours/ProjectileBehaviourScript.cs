using UnityEngine;
public class ProjectileBehaviourScript : MonoBehaviour
{
    private ProjectileData _projectileData;
    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    public void Init(ProjectileData projectileData, Vector2 spawnDirection)
    {
        _projectileData = projectileData;
        _direction = spawnDirection;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.isKinematic = _projectileData.IsKinematic;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (!_projectileData.IsKinematic)
        {
            _rigidbody.AddForce(_projectileData.ProjectileSpeed * _direction, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if (_projectileData.IsKinematic)
        {
            transform.Translate(_projectileData.ProjectileSpeed * Time.deltaTime * _direction);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Make the enemy take damage
        }
        else if (other.gameObject.CompareTag("Breakable"))
        {
            // make the othe break/take damage
        }
        else
        {
            // projectile breaks
        }
    }
}