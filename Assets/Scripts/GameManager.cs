using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField]
        private GameDrawer _gameDrawer;

        [SerializeField]
        private int _numberOfTile6 = 4;
        [SerializeField]
        private int _numberOfTile4 = 4;
        [SerializeField]
        private int _numberOfTile3 = 4;
        [SerializeField]
        private int _numberOfTile2 = 4;

        [SerializeField]
        private int _maxStraightLine = 10;

        private const int BOARD_SIZE = 20;


        private Socket[] _individualTiles;
        private Tile[] _playableTiles;

        private Vector2Int _startingPosition = Vector2Int.zero;

        private List<Vector2Int> _occupiedPositions = new List<Vector2Int>();

        private int _minX;
        private int _maxX;
        private int _minY;
        private int _maxY;

        public Player CurrentPlayer { get; private set; } = Player.One;

        public struct WeightedPosition
        {
            public Vector2Int Position { get; private set; }
            public float Weight { get; private set; }

            public WeightedPosition(Vector2Int position, float weight)
            {
                Position = position;
                Weight = weight;
            }
        }

        private float GetPositionWeight(Vector2Int position)
        {
            float distance = (float) Math.Sqrt(Math.Pow(position.x, 4) + Math.Pow(position.y, 4));

            return 1f / (distance + 1); // weight is inverse square of distance
        }

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            InitializeIndividualTiles();
            InitializePlayableTiles();

            GenerateBoard();

            DrawTiles();

        }

        private void DrawTiles()
        {
            foreach (var tile in _playableTiles)
            {
                _gameDrawer.DrawGameTile(tile);
            }

            _gameDrawer.DrawUnitTiles(_individualTiles);
        }

        private bool TryToSetTileAtPosition(Tile tile, Vector2Int initialPosition)
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
                    if (_maxX - rotation.GetLeftMostX() > _maxStraightLine - 1)
                        canBePlaced = false;
                    else if (rotation.GetRightMostX() - _minX > _maxStraightLine - 1)
                        canBePlaced = false;
                    else if (_maxY - rotation.GetBottomMostY() > _maxStraightLine - 1)
                        canBePlaced = false;
                    else if (rotation.GetTopMostY() - _minY > _maxStraightLine - 1)
                        canBePlaced = false;
                }


                if (canBePlaced)
                {
                    foreach (var position in rotation)
                    {
                        _occupiedPositions.Add(position);

                        UpdateBounds(position);
                    }

                    tile.SetTilePositions(rotation);

                    return true;
                }
            }

            return false;
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

        private bool GenerateBoard()
        {
            List<WeightedPosition> orderedPositions = GetOrderedListOfWeightedPositions();
            
            int nextPlayableTileIndex = 0;
            var placedTilesIndices = new List<int>();
            int positionIndex = 0;

            while (placedTilesIndices.Count < _playableTiles.Length && positionIndex < orderedPositions.Count)
            {
                var tile = _playableTiles[nextPlayableTileIndex];
                var position = orderedPositions[positionIndex].Position;

                if (!TryToSetTileAtPosition(tile, position))
                {
                    // If the tile could not be placed, try the next tile
                    if (nextPlayableTileIndex < _playableTiles.Length - 1)
                    {
                        nextPlayableTileIndex++;
                    }
                    else // If all tiles have been tried, try the next position
                    {
                        positionIndex++;
                    }
                    continue;
                }

                // This is only reached if the tile was placed
                placedTilesIndices.Add(nextPlayableTileIndex);

                // nextPlayableTileIndex becomes the lowest index that is not in placedTilesIndices
                nextPlayableTileIndex = 0;
                while (placedTilesIndices.Contains(nextPlayableTileIndex))
                {
                    nextPlayableTileIndex++;
                }

                // Remove all positions that are now occupied
                var newlyOccupiedPositions = tile.GetTilePositions();
                orderedPositions.RemoveAll(p => newlyOccupiedPositions.Contains(p.Position));

                // Reset positionIndex
                positionIndex = 0;
            }

            if (placedTilesIndices.Count < _playableTiles.Length)
            {
                return false;
            }

            return true;

        }

        private List<WeightedPosition> GetOrderedListOfWeightedPositions()
        {
            List<WeightedPosition> weightedPositions = new List<WeightedPosition>();

            for (int i = -_maxStraightLine; i < _maxStraightLine; i++)
            {
                for (int j = -_maxStraightLine; j < _maxStraightLine; j++)
                {
                    var position = new Vector2Int(i, j);
                    var weight = GetPositionWeight(position);

                    weightedPositions.Add(new WeightedPosition(position, weight));
                }
            }

            var orderedPositions = weightedPositions.OrderByDescending(p => p.Weight).ToList();

            return orderedPositions;
        }

        private void InitializePlayableTiles()
        {
            int totalNumberOfPlayableTiles =
                              _numberOfTile2
                            + _numberOfTile3
                            + _numberOfTile4
                            + _numberOfTile6;

            _playableTiles = new Tile[totalNumberOfPlayableTiles];

            int index = 0;

            for (int i = 0; i < _numberOfTile2; i++)
            {
                _playableTiles[index++] = new Tile2();
            }

            for (int i = 0; i < _numberOfTile3; i++)
            {
                _playableTiles[index++] = new Tile3();
            }

            for (int i = 0; i < _numberOfTile4; i++)
            {
                _playableTiles[index++] = new Tile4();
            }

            for (int i = 0; i < _numberOfTile6; i++)
            {
                _playableTiles[index++] = new Tile6();
            }

            int individualTilesIndex = 0;

            foreach (var tile in _playableTiles)
            {
                // Assign unit tiles to playable tiles
                int number = tile.Number;

                Socket[] unitTileArray = new Socket[number];

                for (int i = 0; i < number; i++)
                {
                    _individualTiles[individualTilesIndex] = new Socket(tile);
                    unitTileArray[i] = _individualTiles[individualTilesIndex];

                    individualTilesIndex++;
                }

                tile.InitializeTile(unitTileArray);
            }

            _playableTiles.Shuffle();
        }

        private void InitializeIndividualTiles()
        {
            int totalNumberOfUnitTiles =
                              _numberOfTile2 * 2
                            + _numberOfTile3 * 3
                            + _numberOfTile4 * 4
                            + _numberOfTile6 * 6;

            _individualTiles = new Socket[totalNumberOfUnitTiles];
        }

        public void GameTileClickedEvent(Socket tile)
        {
            if (tile.Owner != null)
            {
                return;
            }

            PlaceBall(tile);

            Debug.Log("Tile clicked: " + tile.Position);
        }

        private void PlaceBall(Socket tile)
        {
            tile.Owner = CurrentPlayer;

            _gameDrawer.DrawBall(CurrentPlayer, tile.Position);

            CurrentPlayer = CurrentPlayer.Switch();
        }
    }
}
