using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Interactables.ScriptableTiles
{
    [CreateAssetMenu]
    public class SpikeTile : Tile
    {
        
        public Sprite TileSprite;
        public GameObject TileAssociatedPrefab;
     
        public float PrefabLocalZOffset = 0f;
     
        public bool UseAbsoluteZOffset = false;
        public float PrefabAbsoluteZOffset = 0f;
     
        public bool TileIsEditorObjectOnly = false;
     
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
     
        #if UNITY_EDITOR
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            //Always show sprite when we are not running
            if (!Application.isPlaying)
                tileData.sprite = TileSprite;
            else
            {
                //Remove sprite if we intend the sprite to be in-editor only!
                tileData.sprite = TileIsEditorObjectOnly ? tileData.sprite : TileSprite;
            }
     
            if (TileAssociatedPrefab && tileData.gameObject == null)
            {
                tileData.gameObject = TileAssociatedPrefab;
            }
     
            tileData.flags = TileFlags.InstantiateGameObjectRuntimeOnly;
        }
        #endif
     
        #if !UNITY_EDITOR
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            //Remove sprite if we intend the sprite to be in-editor only!
            tileData.sprite = TileIsEditorObjectOnly ? tileData.sprite : TileSprite;
     
            if (TileAssociatedPrefab && tileData.gameObject == null)
            {
                tileData.gameObject = TileAssociatedPrefab;
            }
        }
        #endif
            
    }
}
