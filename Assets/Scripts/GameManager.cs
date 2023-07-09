using Kulami.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Kulami
{
    public class GameManager : MonoBehaviour
    {
        enum State
        {
            MainMenu,
            GeneratingBoard,
            PlacingMarbleP1,
            PlacingMarbleP2,
            GameOver
        }

        struct GameState
        {
            public State State { get; private set; }
            public int Round { get; private set; }
            public int Player1Score { get; }
            public int Player2Score { get; }
            public Player CurrentPlayer { get; }

            public GameState(State state, int round, int player1Score, int player2Score, Player currentPlayer)
            {
                State = state;
                Round = round;
                Player1Score = player1Score;
                Player2Score = player2Score;
                CurrentPlayer = currentPlayer;
            }
        }


        public static GameManager Instance { get; private set; }

        [SerializeField]
        private GameDrawer _gameDrawer;

        [SerializeField]
        private int _numberOfTile6 = 4;
        [SerializeField]
        private int _numberOfTile4 = 5;
        [SerializeField]
        private int _numberOfTile3 = 4;
        [SerializeField]
        private int _numberOfTile2 = 4;

        [SerializeField]
        private int _maxStraightLine = 10;

        [SerializeField]
        private int _numberOfMarbles = 28;

        private int _round = 0;

        private Socket[] _sockets;
        private Tile[] _tiles;

        private List<Vector2Int> _occupiedPositions = new List<Vector2Int>();

        private Tile _lastPlacedTile = null;

        private int _minX;
        private int _maxX;
        private int _minY;
        private int _maxY;

        private List<Vector2Int> _possibleMoves;

        public Player CurrentPlayer { get; private set; } = Player.One;

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

        private float GetPositionWeight(Vector2Int position)
        {
            float distance = (float)Math.Sqrt(Math.Pow(position.x, 4) + Math.Pow(position.y, 4));

            return 1f / (distance + 1); // weight is inverse square of distance
        }

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            SubscribeToEvents();
        }

        private void Update()
        {
            if (_onGameOverScreen)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    HandleSpaceBarPressed();
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    HandleSpaceBarReleased();
                }
            }

        }

        private void SubscribeToEvents()
        {
            GameEvents.Instance.OnSocketClicked += OnSocketClicked;
        }

        private void DrawTiles()
        {
            foreach (var tile in _tiles)
            {
                _gameDrawer.DrawGameTile(tile);
            }

            _gameDrawer.DrawSockets(_sockets);
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
                    return new TilePositionResult(true, rotation);
                }
            }

            return new TilePositionResult(false);
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
                return false;
            }


            _possibleMoves = _occupiedPositions.ToList();

            return true;
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

            _tiles = new Tile[totalNumberOfPlayableTiles];

            int index = 0;

            for (int i = 0; i < _numberOfTile2; i++)
            {
                _tiles[index++] = new Tile2();
            }

            for (int i = 0; i < _numberOfTile3; i++)
            {
                _tiles[index++] = new Tile3();
            }

            for (int i = 0; i < _numberOfTile4; i++)
            {
                _tiles[index++] = new Tile4();
            }

            for (int i = 0; i < _numberOfTile6; i++)
            {
                _tiles[index++] = new Tile6();
            }

            int socketIndex = 0;

            foreach (var tile in _tiles)
            {
                // Assign unit tiles to playable tiles
                int number = tile.Number;

                Socket[] socketArray = new Socket[number];

                for (int i = 0; i < number; i++)
                {
                    _sockets[socketIndex] = new Socket(tile);
                    socketArray[i] = _sockets[socketIndex];

                    socketIndex++;
                }

                tile.InitializeTile(socketArray);
            }

            _tiles.Shuffle();
        }

        private void InitializeSockets()
        {
            int totalNumberOfUnitTiles =
                              _numberOfTile2 * 2
                            + _numberOfTile3 * 3
                            + _numberOfTile4 * 4
                            + _numberOfTile6 * 6;

            _sockets = new Socket[totalNumberOfUnitTiles];
        }

        public void OnSocketClicked(Socket clickedSocket)
        {
            //Debug.Log("Tile clicked: " + clickedSocket.Position);

            if (ClickedSocketIsNotAllowed(clickedSocket))
                return;

            HandleAllowedClickedSocket(clickedSocket);
        }

        private void HandleAllowedClickedSocket(Socket clickedSocket)
        {
            PlaceMarble(clickedSocket);
            CountPoints();

            CalculateNextPossibleMoves(clickedSocket);

            TriggerDrawingEvents(clickedSocket);

            EndRound(clickedSocket);
        }

        private void EndRound(Socket clickedSocket)
        {
            _lastPlacedTile = clickedSocket.ParentTile;
            CurrentPlayer = CurrentPlayer.Switch();

            _round++;

            if (IsGameEnded())
            {
                EndGame();
            }
        }

        private bool IsGameEnded()
        {
            return _round >= _numberOfMarbles * 2 || _possibleMoves.Count == 0;
        }

        private void EndGame()
        {
            ShowGameOverScreen();
            StopShowingPossibleMovesOrPreviews();
        }

        bool _onGameOverScreen = false;

        private void ShowGameOverScreen()
        {
            _gameDrawer.ShowGameOverScreen(Winner, _playerOnePoints, _playerTwoPoints);
            _onGameOverScreen = true;
        }

        private int _playerOnePoints = 0;
        private int _playerTwoPoints = 0;

        private Player? Winner
        {
            get
            {
                if (_playerOnePoints > _playerTwoPoints)
                    return Player.One;
                else if (_playerOnePoints < _playerTwoPoints)
                    return Player.Two;
                else
                    return null;

            }
        }

        private void CountPoints()
        {
            _playerOnePoints = 0;
            _playerTwoPoints = 0;

            foreach (var tile in _tiles)
            {
                var owner = tile.GetOwner();

                if (owner == null)
                    continue;

                if (owner == Player.One)
                    _playerOnePoints += tile.Number;
                else
                    _playerTwoPoints += tile.Number;
            }

            Debug.Log("Player one points: " + _playerOnePoints);
            Debug.Log("Player two points: " + _playerTwoPoints);
        }

        private void CalculateNextPossibleMoves(Socket clickedSocket)
        {
            _possibleMoves = new List<Vector2Int>();

            foreach (var socket in _sockets)
            {
                if (socket.Owner != null)
                {
                    continue;
                }

                if (socket.ParentTile == clickedSocket.ParentTile || socket.ParentTile == _lastPlacedTile)
                {
                    continue;
                }

                if (socket.Position.x == clickedSocket.Position.x || socket.Position.y == clickedSocket.Position.y)
                {
                    _possibleMoves.Add(socket.Position);
                }
            }
        }

        private void TriggerDrawingEvents(Socket clickedSocket)
        {
            GameEvents.Instance.TriggerSetPlayerLastMoveEvent(CurrentPlayer, clickedSocket.Position);

            // Clear previous possible moves
            GameEvents.Instance.TriggerClearPossibleMovesEvent();

            // Show possible moves
            GameEvents.Instance.TriggerPossibleMovesBroadcastEvent(_possibleMoves);
        }

        private bool ClickedSocketIsNotAllowed(Socket clickedSocket)
        {
            if (clickedSocket.Owner != null)
            {
                return true;
            }

            if (!_possibleMoves.Contains(clickedSocket.Position))
            {
                return true;
            }

            return false;
        }

        private void PlaceMarble(Socket tile)
        {
            tile.Owner = CurrentPlayer;
            _gameDrawer.DrawMarble(CurrentPlayer, tile.Position);
        }

        public void NewGame()
        {
            _gameDrawer.Initialize();

            bool isBoardGenerated = false;

            while (!isBoardGenerated)
            {
                InitializeVariables();
                InitializeSockets();
                InitializePlayableTiles();
                isBoardGenerated = GenerateBoard();
            }

            DrawTiles();

            HideStartMenu();
        }

        private void InitializeVariables()
        {
            _round = 0;
            CurrentPlayer = Player.One;
            _lastPlacedTile = null;

            _occupiedPositions = new List<Vector2Int>();
        }

        public void BackToStartMenu()
        {
            _onGameOverScreen = false;
            ClearBoard();
            ShowStartMenu();
        }

        private void ClearBoard()
        {
            _gameDrawer.ClearAllGameComponents();
        }

        private void HideStartMenu()
        {
            _gameDrawer.HideStartMenu();
        }

        private void ShowStartMenu()
        {
            _gameDrawer.ShowStartMenu();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void HandleSpaceBarPressed()
        {
            _gameDrawer.HideGameOverScreen();
        }

        private void HandleSpaceBarReleased()
        {
            _gameDrawer.ShowGameOverScreen();
        }

        private void StopShowingPossibleMovesOrPreviews()
        {
            GameEvents.Instance.TriggerClearPossibleMovesEvent();
            _gameDrawer.StopShowingPreviews();
        }
    }
}
