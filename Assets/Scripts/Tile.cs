using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Tile
    {
        private Socket[] _sockets;
        public int Number { get; protected set; }

        public Tile()
        {
            SetNumber();
        }

        protected abstract void SetNumber();

        public void InitializeTile(Socket[] sockets)
        {
            _sockets = sockets;
        }

        public abstract List<Vector2Int[]> GetAllRotationsBasedOnInitialPosition(Vector2Int initialPosition);
        public abstract Vector2Int[] GetNewPossiblePositions();

        public void SetSocketPositions(Vector2Int[] positions)
        {
            for (int i = 0; i < _sockets.Length; i++)
            {
                _sockets[i].Position = positions[i];
            }
        }

        public Vector2Int[] GetTilePositions()
        {
            Vector2Int[] positions = new Vector2Int[Number];
            for (int i = 0; i < Number; i++)
            {
                positions[i] = _sockets[i].Position;
            }

            return positions;
        }

        public int GetLeftMostX()
        {
            int leftMostX = int.MaxValue;
            foreach (Vector2Int point in GetTilePositions())
            {
                if (point.x < leftMostX)
                    leftMostX = point.x;
            }

            return leftMostX;
        }
        public int GetRightMostX()
        {
            int rightMostX = int.MinValue;
            foreach (Vector2Int point in GetTilePositions())
            {
                if (point.x > rightMostX)
                    rightMostX = point.x;
            }

            return rightMostX;
        }

        public int GetTopMostY()
        {
            int topMostY = int.MinValue;
            foreach (Vector2Int point in GetTilePositions())
            {
                if (point.y > topMostY)
                    topMostY = point.y;
            }
            return topMostY;
        }

        public int GetBottomMostY()
        {
            int bottomMostY = int.MaxValue;
            foreach (Vector2Int point in GetTilePositions())
            {
                if (point.y < bottomMostY)
                    bottomMostY = point.y;
            }
            return bottomMostY;
        }

        public Player? GetOwner()
        {
            int owner = 0; // <0 = player 1, >0 = player 2, 0 = no owner

            foreach (var socket in _sockets)
            {
                if (socket.Owner == Player.One)
                    owner--;
                else if (socket.Owner == Player.Two)
                    owner++;
            }

            if (owner == 0)
                return null;
            else if (owner < 0)
                return Player.One;
            else
                return Player.Two;
        }

    }
}
