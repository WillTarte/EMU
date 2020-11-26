using System;
using UnityEngine;

namespace Interactables
{
    public class InteractOnGroundTriggerScript : MonoBehaviour
    {
        private GameObject _promptPrefab = null;
        private GameObject _promptInstance = null;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;
        private Vector2 _promptHitboxSize;
        private int _promptTextSize;

        public void Init(GameObject promptPrefab, Vector2 promptHitboxSize, int promptTextSize)
        {
            _promptPrefab = promptPrefab;
            _promptHitboxSize = promptHitboxSize;
            _promptTextSize = promptTextSize;
        }

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _boxCollider.isTrigger = true;
            _boxCollider.size = _promptHitboxSize;
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
                _promptInstance = Instantiate(_promptPrefab, transform.parent.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
                _promptInstance.GetComponent<TextMesh>().fontSize = _promptTextSize;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Destroy(_promptInstance);
            }
        }
    }
}