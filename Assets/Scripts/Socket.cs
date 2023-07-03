using UnityEngine;

namespace Assets.Scripts
{
    public class Socket
    {
        public Player? Owner { get; set; } = null;

        public Vector2Int Position { get; set; }

        public Tile ParentTile { get; private set; } = null;

        public Socket(Tile parentTile)
        {
            ParentTile = parentTile;
        }


    }
}