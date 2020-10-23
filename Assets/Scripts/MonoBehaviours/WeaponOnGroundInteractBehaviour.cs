using System;
using UnityEngine;

public class WeaponOnGroundInteractBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2 colliderSize;
    [SerializeField] private Vector2 promptOffset;
    [SerializeField] private Vector2 promptSize;
    private WeaponBehaviourScript _weapon;
    private Rigidbody2D _weaponRigidbody;
    private BoxCollider2D _boxCollider;
    private bool _playerInside = false;
    private Camera _camera;


    private void Start()
    {
        _camera = Camera.main;
    }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.size = colliderSize;
    }

    private void Update()
    {
        if (_weapon.WeaponStateProp == WeaponBehaviourScript.WeaponState.OnGround)
        {
            _weaponRigidbody.isKinematic = false;

            if (_playerInside)
            {
                DisplayPrompt();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;
        }
    }

    public void Init(WeaponBehaviourScript weaponBehaviour)
    {
        _weapon = weaponBehaviour;
        _weaponRigidbody = weaponBehaviour.GetComponent<Rigidbody2D>();
    }

    private void DisplayPrompt()
    {
        //todo is this fine to do?
        var position = _weapon.transform.position;
        Vector3 gameObjPos = _camera.WorldToScreenPoint(position);
        GUI.Box(new Rect(position.x + promptOffset.x, position.y + promptOffset.y, promptSize.x, promptSize.y), "Press E to pickup");
    }
}