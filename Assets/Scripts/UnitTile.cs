using Assets.Scripts;
using UnityEngine;

public class UnitTile
{
    public TileStatus Status { get; set; } = TileStatus.Empty;

    public Vector2Int Position { get; set; }
}
