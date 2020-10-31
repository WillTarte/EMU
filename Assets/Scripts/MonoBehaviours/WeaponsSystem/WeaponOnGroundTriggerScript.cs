using UnityEngine;

namespace MonoBehaviours.WeaponsSystem
{
    public class WeaponOnGroundTriggerScript : MonoBehaviour
    {
        private GameObject _promptPrefab = null;
        private GameObject _promptInstance = null;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;
        private bool _playerInside = false;

        public void Init(GameObject promptPrefab)
        {
            _promptPrefab = promptPrefab;
        }

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _boxCollider.isTrigger = true;
            _boxCollider.size = new Vector2(5, 5);
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.isKinematic = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && _playerInside)
            {
                // TODO when character controller is done
                //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PickUp(transform.parent.gameObject);
                var parent = transform.parent;
                parent.gameObject.GetComponent<WeaponBehaviourScript>().WeaponStateProp = WeaponBehaviourScript.WeaponState.Active;
                Debug.Log("Picked up!!!");
                parent.parent = GameObject.FindGameObjectWithTag("Player").transform;

            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _promptInstance = Instantiate(_promptPrefab, transform.parent.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
                _playerInside = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Destroy(_promptInstance);
                _playerInside = false;
            }
        }
    }
}