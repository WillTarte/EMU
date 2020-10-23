using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
public class ProjectileBehaviourScript : MonoBehaviour
{
    [SerializeField] private int aliveTime;
    
    private ProjectileData _projectileData;
    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

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

    private void OnDestroy()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(aliveTime);
        Destroy(this.gameObject);
    }
}