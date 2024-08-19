using UnityEngine;
using UnityEngine.Tilemaps;

public class ClearEmptyTiles : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        BoundsInt boundsInt = tilemap.cellBounds;
        Bounds bounds = tilemap.localBounds;
        Debug.Log("Tilemap Bounds: " + bounds);
        Debug.Log("Tilemap Cell Bounds: " + boundsInt);

        ClearEmptyAreas();
    }

    void ClearEmptyAreas()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        foreach (var position in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            if (tile == null)
            {
                tilemap.SetTile(position, null); // Удаляем пустые тайлы
                Debug.Log("Удалён");
            }
        }
        tilemap.RefreshAllTiles();
    }
}
