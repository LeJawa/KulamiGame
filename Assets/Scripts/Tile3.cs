using System.Collections.Generic;
using UnityEngine;

namespace Kulami
{
    public class Tile3 : Tile
    {
        public override List<Vector2Int[]> GetAllRotationsBasedOnInitialPosition(Vector2Int initialPosition)
        {
            // Tile3 is a 3x1 tile and has 6 different rotations/positions

            var rotation1 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.right, initialPosition + Vector2Int.right + Vector2Int.right };
            var rotation2 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.down, initialPosition + Vector2Int.down + Vector2Int.down };
            var rotation3 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.left, initialPosition + Vector2Int.left + Vector2Int.left };
            var rotation4 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.up, initialPosition + Vector2Int.up + Vector2Int.up };
            var rotation5 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.right, initialPosition + Vector2Int.left };
            var rotation6 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.down, initialPosition + Vector2Int.up };

            return new List<Vector2Int[]> { rotation1, rotation2, rotation3, rotation4, rotation5, rotation6 };
        }

        public override Vector2Int[] GetNewPossiblePositions()
        {
            Vector2Int[] tilePositions = GetTilePositions();

            Vector2Int[] newPossiblePositions = new Vector2Int[8];

            // check if the tile is horizontal
            if (tilePositions[0].x == tilePositions[1].x)
            {
                newPossiblePositions[0] = tilePositions[0] + Vector2Int.down;
                newPossiblePositions[1] = tilePositions[0] + Vector2Int.up;
                newPossiblePositions[2] = tilePositions[1] + Vector2Int.down;
                newPossiblePositions[3] = tilePositions[1] + Vector2Int.up;
                newPossiblePositions[4] = tilePositions[2] + Vector2Int.down;
                newPossiblePositions[5] = tilePositions[2] + Vector2Int.up;

                var leftMostPosition = new Vector2Int(GetLeftMostX(), tilePositions[0].y);
                var rightMostPosition = new Vector2Int(GetRightMostX(), tilePositions[0].y);

                newPossiblePositions[6] = leftMostPosition + Vector2Int.left;
                newPossiblePositions[7] = rightMostPosition + Vector2Int.right;
            }
            else
            {
                newPossiblePositions[0] = tilePositions[0] + Vector2Int.left;
                newPossiblePositions[1] = tilePositions[0] + Vector2Int.right;
                newPossiblePositions[2] = tilePositions[1] + Vector2Int.left;
                newPossiblePositions[3] = tilePositions[1] + Vector2Int.right;
                newPossiblePositions[4] = tilePositions[2] + Vector2Int.left;
                newPossiblePositions[5] = tilePositions[2] + Vector2Int.right;

                var bottomMostPosition = new Vector2Int(tilePositions[0].x, GetBottomMostY());
                var topMostPosition = new Vector2Int(tilePositions[0].x, GetTopMostY());

                newPossiblePositions[6] = bottomMostPosition + Vector2Int.down;
                newPossiblePositions[7] = topMostPosition + Vector2Int.up;
            }

            return newPossiblePositions;
        }

        protected override void SetNumber()
        {
            Number = 3;
        }
    }
}
