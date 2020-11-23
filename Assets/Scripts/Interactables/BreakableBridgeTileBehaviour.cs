using System;
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
            var tilePos = new Vector3Int((int) Math.Floor(transform.position.x),
                (int) Math.Floor(transform.position.y), 0);

            // Believe it or not, this destroys this tile, yet no documentation mentions this ¯\_(ツ)_/¯
            _tilemap.SetTile(tilePos,null);
        }
    }
}