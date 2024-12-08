using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap
    public TileBase blockerTile; // The tile to use for blocking
    public Vector3Int[] tilePositions; // Positions to block

    private bool roomActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !roomActive)
        {
            roomActive = true;
            DrawBlockers();
        }
    }

    private void DrawBlockers()
    {
        foreach (Vector3Int position in tilePositions)
        {
            tilemap.SetTile(position, blockerTile);
        }
    }

    public void ClearBlockers()
    {
        foreach (Vector3Int position in tilePositions)
        {
            tilemap.SetTile(position, null);
        }
    }
}
