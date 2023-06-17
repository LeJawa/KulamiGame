using Assets.Scripts;
using UnityEngine;

public class UnitTile : MonoBehaviour
{
    public TileStatus Status { get; set; } = TileStatus.Empty;

    public Vector2Int Position { get; set; }
}
