using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tile6 : Tile
    {
        protected override void SetNumber()
        {
            Number = 6;
        }

        public override List<Vector2Int[]> GetAllRotationsBasedOnInitialPosition(Vector2Int initialPosition)
        {
            // Tile6 is a 3x2 tile and has 12 different rotations/positions

            var rotation1 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.right, initialPosition + Vector2Int.right + Vector2Int.right,
                initialPosition + Vector2Int.down, initialPosition + Vector2Int.down + Vector2Int.right, initialPosition + Vector2Int.down + Vector2Int.right + Vector2Int.right };
            var rotation2 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.down, initialPosition + Vector2Int.down + Vector2Int.down,
                initialPosition + Vector2Int.right, initialPosition + Vector2Int.right + Vector2Int.down, initialPosition + Vector2Int.right + Vector2Int.down + Vector2Int.down };
            var rotation3 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.left, initialPosition + Vector2Int.left + Vector2Int.left,
                initialPosition + Vector2Int.down, initialPosition + Vector2Int.down + Vector2Int.left, initialPosition + Vector2Int.down + Vector2Int.left + Vector2Int.left };
            var rotation4 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.up, initialPosition + Vector2Int.up + Vector2Int.up,
                initialPosition + Vector2Int.right, initialPosition + Vector2Int.right + Vector2Int.up, initialPosition + Vector2Int.right + Vector2Int.up + Vector2Int.up };
            var rotation5 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.right, initialPosition + Vector2Int.left,
                initialPosition + Vector2Int.down, initialPosition + Vector2Int.down + Vector2Int.right, initialPosition + Vector2Int.down + Vector2Int.left };
            var rotation6 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.down, initialPosition + Vector2Int.up,
                initialPosition + Vector2Int.right, initialPosition + Vector2Int.right + Vector2Int.down, initialPosition + Vector2Int.right + Vector2Int.up };
            var rotation7 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.left, initialPosition + Vector2Int.right,
                initialPosition + Vector2Int.up, initialPosition + Vector2Int.up + Vector2Int.left, initialPosition + Vector2Int.up + Vector2Int.right };
            var rotation8 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.up, initialPosition + Vector2Int.down,
                initialPosition + Vector2Int.left, initialPosition + Vector2Int.left + Vector2Int.up, initialPosition + Vector2Int.left + Vector2Int.down };
            var rotation9 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.right, initialPosition + Vector2Int.right + Vector2Int.right,
                initialPosition + Vector2Int.up, initialPosition + Vector2Int.up + Vector2Int.right, initialPosition + Vector2Int.up + Vector2Int.right + Vector2Int.right };
            var rotation10 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.down, initialPosition + Vector2Int.down + Vector2Int.down,
                initialPosition + Vector2Int.left, initialPosition + Vector2Int.left + Vector2Int.down, initialPosition + Vector2Int.left + Vector2Int.down + Vector2Int.down };
            var rotation11 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.left, initialPosition + Vector2Int.left + Vector2Int.left,
                initialPosition + Vector2Int.up, initialPosition + Vector2Int.up + Vector2Int.left, initialPosition + Vector2Int.up + Vector2Int.left + Vector2Int.left };
            var rotation12 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.up, initialPosition + Vector2Int.up + Vector2Int.up,
                initialPosition + Vector2Int.left, initialPosition + Vector2Int.left + Vector2Int.up, initialPosition + Vector2Int.left + Vector2Int.up + Vector2Int.up };

            return new List<Vector2Int[]> { rotation1, rotation2, rotation3, rotation4, rotation5, rotation6, rotation7, rotation8, rotation9, rotation10, rotation11, rotation12 };
        }

        public override Vector2Int[] GetNewPossiblePositions()
        {
            Vector2Int[] tilePositions = GetTilePositions();

            // Find the bounds of the tile
            int minX = GetLeftMostX(), minY = GetBottomMostY();
            int maxX = GetRightMostX(), maxY = GetTopMostY();

            Vector2Int[] newPossiblePositions = new Vector2Int[10];

            // check if the tile is horizontal
            if (maxX - minX == 2)
            {
                newPossiblePositions[0] = new Vector2Int(minX - 1, minY);
                newPossiblePositions[1] = new Vector2Int(minX - 1, maxY);
                newPossiblePositions[2] = new Vector2Int(maxX + 1, minY);
                newPossiblePositions[3] = new Vector2Int(maxX + 1, maxY);
                newPossiblePositions[4] = new Vector2Int(minX, minY - 1);
                newPossiblePositions[5] = new Vector2Int(maxX, minY - 1);
                newPossiblePositions[6] = new Vector2Int(minX, maxY + 1);
                newPossiblePositions[7] = new Vector2Int(maxX, maxY + 1);
                newPossiblePositions[8] = new Vector2Int(minX + 1, minY - 1);
                newPossiblePositions[9] = new Vector2Int(minX + 1, maxY + 1);
            }
            else
            {
                newPossiblePositions[0] = new Vector2Int(minX, minY - 1);
                newPossiblePositions[1] = new Vector2Int(maxX, minY - 1);
                newPossiblePositions[2] = new Vector2Int(minX, maxY + 1);
                newPossiblePositions[3] = new Vector2Int(maxX, maxY + 1);
                newPossiblePositions[4] = new Vector2Int(minX - 1, minY);
                newPossiblePositions[5] = new Vector2Int(minX - 1, maxY);
                newPossiblePositions[6] = new Vector2Int(maxX + 1, minY);
                newPossiblePositions[7] = new Vector2Int(maxX + 1, maxY);
                newPossiblePositions[8] = new Vector2Int(minX - 1, minY + 1);
                newPossiblePositions[9] = new Vector2Int(maxX + 1, minY + 1);
            }

            return newPossiblePositions;
        }
    }
}
