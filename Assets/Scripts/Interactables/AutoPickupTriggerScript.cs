using System;
using Player;
using UnityEngine;

namespace Interactables
{
    public class AutoPickupTriggerScript : MonoBehaviour
    {
        private Vector2 _triggerSize;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;
        private IAutoPickup _parentScript;

        public void Init(Vector2 triggerSize)
        {
            _triggerSize = triggerSize;
        }

        public void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _parentScript = GetComponentInParent<IAutoPickup>();
        }
        
        private void Start()
        {
            _boxCollider.isTrigger = true;
            _boxCollider.size = _triggerSize;
            _rigidbody.isKinematic = true;
        }
        
        private void Update()
        {
            transform.position = transform.parent.position;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _parentScript.Pickup(other.GetComponent<Controller>());
            }
        }
    }
}