using UnityEngine;

namespace Kulami
{
    public class Socket
    {
        private Player? _owner = null;
        public Player? Owner 
        { 
            get 
            { 
                return _owner; 
            } 
            set 
            {
                _owner = value;
                ParentTile.UpdateOwnership(_owner);
            } 
        }

        public Vector2Int Position { get; set; }

        public Tile ParentTile { get; private set; } = null;

        public Socket(Tile parentTile)
        {
            ParentTile = parentTile;
        }


    }
}