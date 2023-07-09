using System.Collections.Generic;
using UnityEngine;

namespace Kulami
{
    public class Tile2 : Tile
    {
        public override List<Vector2Int[]> GetAllRotationsBasedOnInitialPosition(Vector2Int initialPosition)
        {
            // Tile2 is a 2x1 tile and has 4 different rotations/positions

            var rotation1 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.right };
            var rotation2 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.down };
            var rotation3 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.left };
            var rotation4 = new Vector2Int[] { initialPosition, initialPosition + Vector2Int.up };

            return new List<Vector2Int[]> { rotation1, rotation2, rotation3, rotation4 };
        }

        public override Vector2Int[] GetNewPossiblePositions()
        {
            Vector2Int[] tilePositions = GetTilePositions();

            Vector2Int[] newPossiblePositions = new Vector2Int[6];

            // check if the tile is horizontal
            if (tilePositions[0].x == tilePositions[1].x)
            {
                newPossiblePositions[0] = tilePositions[0] + Vector2Int.down;
                newPossiblePositions[1] = tilePositions[0] + Vector2Int.up;
                newPossiblePositions[2] = tilePositions[1] + Vector2Int.down;
                newPossiblePositions[3] = tilePositions[1] + Vector2Int.up;

                if (tilePositions[0].x < tilePositions[1].x)
                {
                    newPossiblePositions[4] = tilePositions[0] + Vector2Int.left;
                    newPossiblePositions[5] = tilePositions[1] + Vector2Int.right;
                }
                else
                {
                    newPossiblePositions[4] = tilePositions[0] + Vector2Int.right;
                    newPossiblePositions[5] = tilePositions[1] + Vector2Int.left;
                }
            }
            else
            {
                newPossiblePositions[0] = tilePositions[0] + Vector2Int.left;
                newPossiblePositions[1] = tilePositions[0] + Vector2Int.right;
                newPossiblePositions[2] = tilePositions[1] + Vector2Int.left;
                newPossiblePositions[3] = tilePositions[1] + Vector2Int.right;

                if (tilePositions[0].y < tilePositions[1].y)
                {
                    newPossiblePositions[4] = tilePositions[0] + Vector2Int.down;
                    newPossiblePositions[5] = tilePositions[1] + Vector2Int.up;
                }
                else
                {
                    newPossiblePositions[4] = tilePositions[0] + Vector2Int.up;
                    newPossiblePositions[5] = tilePositions[1] + Vector2Int.down;
                }
            }

            return newPossiblePositions;
        }

        protected override void SetNumber()
        {
            Number = 2;
        }
    }
}
