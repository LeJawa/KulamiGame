using UnityEngine;

namespace Assets.Scripts
{
    public class UnitTile
    {
        public TileStatus Status { get; set; } = TileStatus.Empty;

        public Vector2Int Position { get; set; }

        public Tile ParentTile { get; private set; } = null;

        public UnitTile(Tile parentTile)
        {
            ParentTile = parentTile;
        }


    }
}