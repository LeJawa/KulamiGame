using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Game
{
    public class Tile4 : Tile
    {
        public override List<Vector2Int[]> GetAllRotationsBasedOnInitialPosition(Vector2Int initialPosition)
        {
            // Tile4 has 4 different rotations/positions

            var rotation1 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.right, initialPosition + Vector2Int.up, initialPosition + Vector2Int.one };
            var rotation2 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.right, initialPosition + Vector2Int.down, initialPosition + Vector2Int.right + Vector2Int.down };
            var rotation3 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.left, initialPosition + Vector2Int.down, initialPosition + Vector2Int.left + Vector2Int.down };
            var rotation4 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.left, initialPosition + Vector2Int.up, initialPosition + Vector2Int.left + Vector2Int.up };

            return new List<Vector2Int[]> { rotation1, rotation2, rotation3, rotation4 };
        }

        public override Vector2Int[] GetNewPossiblePositions()
        {
            Vector2Int[] squarePositions = GetTilePositions();

            // Find the bounds of the square
            int minX = GetLeftMostX(), minY = GetBottomMostY();
            int maxX = GetRightMostX(), maxY = GetTopMostY();

            Vector2Int[] newPossiblePositions = new Vector2Int[8];

            newPossiblePositions[0] = new Vector2Int(minX - 1, minY);
            newPossiblePositions[1] = new Vector2Int(minX - 1, maxY);
            newPossiblePositions[2] = new Vector2Int(maxX + 1, minY);
            newPossiblePositions[3] = new Vector2Int(maxX + 1, maxY);
            newPossiblePositions[4] = new Vector2Int(minX, minY - 1);
            newPossiblePositions[5] = new Vector2Int(maxX, minY - 1);
            newPossiblePositions[6] = new Vector2Int(minX, maxY + 1);
            newPossiblePositions[7] = new Vector2Int(maxX, maxY + 1);

            return newPossiblePositions;
        }

        protected override void SetNumber()
        {
            Number = 4;
        }
    }
}
