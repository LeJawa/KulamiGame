using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Tile
    {
        private UnitTile[] _unitTiles;
        public int Number { get; protected set; }

        public Tile()
        {
            SetNumber();
        }

        protected abstract void SetNumber();

        public void InitializeTile(UnitTile[] unitTiles)
        {
            _unitTiles = unitTiles;
        }

        public abstract List<Vector2Int[]> GetAllRotationsBasedOnInitialPosition(Vector2Int initialPosition);
        public abstract Vector2Int[] GetNewPossiblePositions();

        public void SetTilePositions(Vector2Int[] positions)
        {
            for (int i = 0; i < _unitTiles.Length; i++)
            {
                _unitTiles[i].Position = positions[i];
            }
        }

        public Vector2Int[] GetTilePositions()
        {
            Vector2Int[] squarePositions = new Vector2Int[Number];
            for (int i = 0; i < Number; i++)
            {
                squarePositions[i] = _unitTiles[i].Position;
            }

            return squarePositions;
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

    }
}
