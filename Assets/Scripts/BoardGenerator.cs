using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Kulami
{
    public class BoardGenerator
    {
        private struct WeightedPosition
        {
            public Vector2Int Position { get; private set; }
            public float Weight { get; private set; }

            public WeightedPosition(Vector2Int position, float weight)
            {
                Position = position;
                Weight = weight;
            }
        }

        private struct TilePositionResult
        {
            public bool CanBePlaced { get; private set; }
            public Vector2Int[] Positions { get; private set; }

            public TilePositionResult(bool canBePlaced, Vector2Int[] positions = null)
            {
                CanBePlaced = canBePlaced;
                Positions = positions;

                if (canBePlaced && positions == null)
                {
                    throw new ArgumentException("Positions must be provided if the tile can be placed");
                }
            }
        }

        private int _minX;
        private int _maxX;
        private int _minY;
        private int _maxY;
        private List<Vector2Int> _occupiedPositions;

        private Tile[] _tiles;

        public int MaxStraightLine { get; set; } = 10;

        private float GetPositionWeight(Vector2Int position)
        {
            float distance = (float)Math.Sqrt(Math.Pow(position.x, 4) + Math.Pow(position.y, 4));

            return 1f / (distance + 1); // weight is inverse square of distance
        }

        private List<WeightedPosition> GetOrderedListOfWeightedPositions()
        {
            List<WeightedPosition> weightedPositions = new List<WeightedPosition>();

            for (int i = -MaxStraightLine; i < MaxStraightLine; i++)
            {
                for (int j = -MaxStraightLine; j < MaxStraightLine; j++)
                {
                    var position = new Vector2Int(i, j);
                    var weight = GetPositionWeight(position);

                    weightedPositions.Add(new WeightedPosition(position, weight));
                }
            }

            var orderedPositions = weightedPositions.OrderByDescending(p => p.Weight).ToList();

            return orderedPositions;
        }

        private TilePositionResult TryToPlaceTileAtPosition(Tile tile, Vector2Int initialPosition)
        {
            var rotations = tile.GetAllRotationsBasedOnInitialPosition(initialPosition);
            rotations.Shuffle();

            foreach (var rotation in rotations)
            {
                bool canBePlaced = true;

                foreach (var position in rotation)
                {
                    if (_occupiedPositions.Contains(position))
                    {
                        canBePlaced = false;
                        break;
                    }
                }

                // Check for max straight line rule
                if (canBePlaced)
                {
                    if (_maxX - rotation.GetLeftMostX() > MaxStraightLine - 1)
                        canBePlaced = false;
                    else if (rotation.GetRightMostX() - _minX > MaxStraightLine - 1)
                        canBePlaced = false;
                    else if (_maxY - rotation.GetBottomMostY() > MaxStraightLine - 1)
                        canBePlaced = false;
                    else if (rotation.GetTopMostY() - _minY > MaxStraightLine - 1)
                        canBePlaced = false;
                }

                if (canBePlaced)
                {
                    return new TilePositionResult(true, rotation);
                }
            }

            return new TilePositionResult(false);
        }

        private void UpdateBounds(Vector2Int position)
        {
            if (position.x < _minX)
            {
                _minX = position.x;
            }
            else if (position.x > _maxX)
            {
                _maxX = position.x;
            }
            if (position.y < _minY)
            {
                _minY = position.y;
            }
            else if (position.y > _maxY)
            {
                _maxY = position.y;
            }
        }

        private void PlaceTileAtPosition(Tile tile, Vector2Int[] positions)
        {
            foreach (var position in positions)
            {
                _occupiedPositions.Add(position);

                UpdateBounds(position);
            }

            tile.SetSocketPositions(positions);
        }

        private static int GetNextPlayableTileIndex(List<int> placedTilesIndices)
        {
            int nextPlayableTileIndex = 0;
            while (placedTilesIndices.Contains(nextPlayableTileIndex))
            {
                nextPlayableTileIndex++;
            }

            return nextPlayableTileIndex;
        }

        public bool GenerateBoard(ref Tile[] tiles, ref List<Vector2Int> occupiedPositions)
        {
            _tiles = tiles;
            _occupiedPositions = occupiedPositions;

            List<WeightedPosition> orderedPositions = GetOrderedListOfWeightedPositions();

            int nextPlayableTileIndex = 0;
            var placedTilesIndices = new List<int>();
            int positionIndex = 0;

            while (placedTilesIndices.Count < _tiles.Length && positionIndex < orderedPositions.Count)
            {
                // if cannot place all tiles in one go, end method. If try to continue, might run into a wrong position.
                if (nextPlayableTileIndex != placedTilesIndices.Count)
                {
                    //Debug.Log($"Failed to generate board.");
                    return false;
                }

                var tile = _tiles[nextPlayableTileIndex];

                var position = orderedPositions[positionIndex].Position;

                //Debug.Log($"Trying tile {tile.Number} ({nextPlayableTileIndex}) at position {position}");

                var result = TryToPlaceTileAtPosition(tile, position);

                if (!result.CanBePlaced)
                {
                    //Debug.Log($"Tile {tile.Number} could not be placed at position {position}");

                    // If the tiles are not placed in one go, the following code yields an error
                    // ERROR STARTS HERE

                    // If the tile could not be placed, try the next tile
                    if (nextPlayableTileIndex < _tiles.Length - 1)
                    {
                        nextPlayableTileIndex++;
                        //Debug.Log($"Trying next tile: {nextPlayableTileIndex}");
                    }
                    else // If all tiles have been tried, try the next position
                    {
                        positionIndex++;
                        //Debug.Log($"Trying next position: {orderedPositions[positionIndex].Position}");
                    }
                    continue;

                    // ERROR ENDS HERE
                }

                // This is only reached if the tile can be placed
                PlaceTileAtPosition(tile, result.Positions);

                placedTilesIndices.Add(nextPlayableTileIndex);

                // nextPlayableTileIndex becomes the lowest index that is not in placedTilesIndices
                nextPlayableTileIndex = GetNextPlayableTileIndex(placedTilesIndices);

                // Remove all positions that are now occupied
                var newlyOccupiedPositions = tile.GetTilePositions();
                orderedPositions.RemoveAll(p => newlyOccupiedPositions.Contains(p.Position));

                // Reset positionIndex
                positionIndex = 0;
            }

            if (placedTilesIndices.Count < _tiles.Length)
            {
                throw new Exception("Not all tiles could be placed");
            }

            return true;
        }
    }
}
