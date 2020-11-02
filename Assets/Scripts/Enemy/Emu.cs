using System;
using UnityEngine;

namespace Enemy
{
    public class Emu : MonoBehaviour
    {
        #region Interface Variables

        [SerializeField] public GameObject mPlayer;
        [SerializeField] public SpriteRenderer mSpriteRenderer;

        #endregion

        void Update()
        {
            IsFacingPlayer();
        }

        private void IsFacingPlayer()
        {
            var emuPosition = gameObject.transform.position;
            var playerPosition = mPlayer.transform.position;

            mSpriteRenderer.flipX = (emuPosition.x - playerPosition.x < 0);
        }
    }
}