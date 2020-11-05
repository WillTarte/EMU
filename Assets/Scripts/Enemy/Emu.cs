using System;
using UnityEngine;

namespace Enemy
{
    public class Emu : MonoBehaviour
    {
        #region private Variables

        private GameObject _player;
        
        #endregion

        void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }
        
        void Update()
        {
            IsFacingPlayer();
        }

        private void IsFacingPlayer()
        {
            var emuPosition = gameObject.transform.position;
            var playerPosition = _player.transform.position;

            gameObject.GetComponent<SpriteRenderer>().flipX = (emuPosition.x - playerPosition.x < 0);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Projectile"))
            {
                Destroy(col.gameObject);
                Destroy(gameObject);
            }
        }
    }
}