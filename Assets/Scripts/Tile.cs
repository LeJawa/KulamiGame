using System.Collections.Generic;
using System.Drawing;
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

    }
}
