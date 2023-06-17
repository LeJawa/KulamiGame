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
            throw new System.NotImplementedException();
        }
    }
}
