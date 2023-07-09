﻿using Kulami.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Kulami
{
    public class GameManager : MonoBehaviour
    {
        enum GameState
        {
            MainMenu,
            GeneratingBoard,
            PlacingMarbleP1,
            BetweenTurns,
            PlacingMarbleP2,
            GameOver
        }

        struct GameStateInfo
        {
            public GameState State { get; }
            public int Round { get; }
            public int Player1Score { get; }
            public int Player2Score { get; }
            public Player? Winner { get; }

            public GameStateInfo(GameState state, int round, int player1Score, int player2Score, Player? winner)
            {
                State = state;
                Round = round;
                Player1Score = player1Score;
                Player2Score = player2Score;
                Winner = winner;
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

        private List<Vector2Int> _possibleMoves;

        public Player CurrentPlayer { get; private set; } = Player.One;

        private GameState _state = GameState.MainMenu;
        private bool _stateChanged = false;

        private BoardGenerator _boardGenerator = new BoardGenerator();

        private GameState State
        {
            get => _state;
            set
            {
                _state = value;
                _stateChanged = true;
            }
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
            if (_stateChanged)
            {
                _stateChanged = false;

                switch (State)
                {
                    case GameState.MainMenu:
                        HandleMainMenuState();
                        break;
                    case GameState.GeneratingBoard:
                        HandleGeneratingBoardState();
                        break;
                    case GameState.PlacingMarbleP1:
                        HandlePlacingMarbleP1State();
                        break;
                    case GameState.BetweenTurns:
                        HandleBetweenTurnsState();
                        break;
                    case GameState.PlacingMarbleP2:
                        HandlePlacingMarbleP2State();
                        break;
                    case GameState.GameOver:
                        HandleGameOverState();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }   

            if (State == GameState.GameOver)
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

        private void HandlePlacingMarbleP2State()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        private void HandleBetweenTurnsState()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        private void HandlePlacingMarbleP1State()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        private void HandleGeneratingBoardState()
        {
            // TODO: Implement
            throw new NotImplementedException();
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

        private bool GenerateBoard()
        {
            return new BoardGenerator().GenerateBoard(ref _tiles, ref _occupiedPositions);
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
            CountScore();

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
                State = GameState.GameOver;
            }
        }

        private bool IsGameEnded()
        {
            return _round >= _numberOfMarbles * 2 || _possibleMoves.Count == 0;
        }

        private void HandleGameOverState()
        {            
            ShowGameOverScreen();
            StopShowingPossibleMovesOrPreviews();
        }

        private void ShowGameOverScreen()
        {
            _gameDrawer.ShowGameOverScreen(Winner, _playerOneScore, _playerTwoScore);
        }

        private int _playerOneScore = 0;
        private int _playerTwoScore = 0;

        private Player? Winner
        {
            get
            {
                if (_playerOneScore > _playerTwoScore)
                    return Player.One;
                else if (_playerOneScore < _playerTwoScore)
                    return Player.Two;
                else
                    return null;
            }
        }

        private void CountScore()
        {
            _playerOneScore = 0;
            _playerTwoScore = 0;

            foreach (var tile in _tiles)
            {
                var owner = tile.GetOwner();

                if (owner == null)
                    continue;

                if (owner == Player.One)
                    _playerOneScore += tile.Number;
                else
                    _playerTwoScore += tile.Number;
            }

            Debug.Log("Player one score: " + _playerOneScore);
            Debug.Log("Player two score: " + _playerTwoScore);
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

            InitializeBoardGenerator();

            bool isBoardGenerated = false;

            while (!isBoardGenerated)
            {
                InitializeVariables();
                InitializeSockets();
                InitializePlayableTiles();
                isBoardGenerated = GenerateBoard();
            }

            _possibleMoves = _occupiedPositions;

            DrawTiles();

            HideStartMenu();
        }

        private void InitializeBoardGenerator()
        {
            _boardGenerator.MaxStraightLine = _maxStraightLine;
        }

        private void InitializeVariables()
        {
            _round = 0;
            CurrentPlayer = Player.One;
            _lastPlacedTile = null;

            _occupiedPositions = new List<Vector2Int>();
        }

        // Triggered via button in GameOverScreen
        public void BackToStartMenu()
        {
            State = GameState.MainMenu;
        }

        private void HandleMainMenuState()
        {
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
