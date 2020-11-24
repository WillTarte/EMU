using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Interactables
{
    public class BreakableBridgeTileBehaviour : MonoBehaviour, IBreakable
    {
        private Tilemap _tilemap;

        private void Start()
        {
            _tilemap = GetComponentInParent<Tilemap>();
            _tilemap.CompressBounds();
        }

        public void Break()
        {
            StartCoroutine(DestroyOnNextFrame());
        }

        private IEnumerator DestroyOnNextFrame()
        {
            var tilePos = new Vector3Int((int) Math.Floor(transform.position.x),
                (int) Math.Floor(transform.position.y), 0);
            yield return new WaitForEndOfFrame();
            // Believe it or not, this destroys this tile (gameobject), yet no documentation mentions this ¯\_(ツ)_/¯
            _tilemap.SetTile(tilePos,null);
        }
    }
}