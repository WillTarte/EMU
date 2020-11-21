using Player;
using UnityEngine;

namespace Interactables
{
    public class HealthPickupBehaviour : MonoBehaviour, IAutoPickup
    {
        private const string GameObjectName = "HealthPickupTrigger";

        [SerializeField] private Vector2 pickupTriggerSize;
        [SerializeField] private int amount;

        private Rigidbody2D _rigidbody;
        private GameObject _trigger;
        private bool _forceApplied;

        public void Init(int amt)
        {
            this.amount = amt;
        }

        private void Awake()
        {
            _trigger = new GameObject {layer = LayerMask.NameToLayer("Trigger"), name = GameObjectName};
            _trigger.transform.parent = transform;
            _trigger.tag = "InteractTrigger";
            _trigger.AddComponent<BoxCollider2D>();
            _trigger.AddComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
            _trigger.AddComponent<AutoPickupTriggerScript>().Init(pickupTriggerSize);
            
            _trigger.SetActive(true);

            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_forceApplied)
            {
                _rigidbody.AddForce(new Vector2(Random.Range(0, 5), Random.Range(0, 5)), ForceMode2D.Impulse);
                _forceApplied = true;
            }
        }

        public void Pickup(Controller playerController)
        {
            if (playerController.IsFullHp || _rigidbody.velocity.y != 0)
            {
                return;
            }
            playerController.RestoreHitPoints(amount);
            Destroy(gameObject);
        }
    }
}
