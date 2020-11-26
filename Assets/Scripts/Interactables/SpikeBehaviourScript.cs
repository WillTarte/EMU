using System;
using Player;
using UnityEngine;

namespace Interactables
{
    public class SpikeBehaviourScript : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Controller>().LoseHitPoints(1);
                other.rigidbody.AddForce(Vector2.up * 2.0f, ForceMode2D.Impulse);
            }
        }
    }
}
