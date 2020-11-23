using UnityEngine;
using UnityEngine.Tilemaps;

namespace Interactables.ScriptableTiles
{
    [CreateAssetMenu(fileName = "NewScriptableTile", menuName = "ScriptableObjects/ScriptableTiles/ScriptableTile", order = 1)]
    public class ScriptableTile : Tile
    {
        public Sprite TileSprite;
        public GameObject TileAssociatedPrefab;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
     
            //This prevents rogue prefab objects from appearing when the Tile palette is present
            #if UNITY_EDITOR
            if (go != null)
            {
                if (go.scene.name == null || go.scene.name == "Preview Scene")
                {
                    DestroyImmediate(go);
                }
            }
            #endif

            return true;
        }
     
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            //Always show sprite when we are not running
            tileData.sprite = TileSprite;

            if (TileAssociatedPrefab && tileData.gameObject == null)
            {
                tileData.gameObject = TileAssociatedPrefab;
            }
     
            tileData.flags = TileFlags.InstantiateGameObjectRuntimeOnly;
            tileData.colliderType = ColliderType.Sprite;
        }
    }
}