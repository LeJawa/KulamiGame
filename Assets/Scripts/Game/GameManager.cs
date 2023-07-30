using DG.Tweening;
using kulami;
using Kulami.Data;
using Kulami.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

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

        private Tile _lastPlacedTile = null;

        private List<Vector2Int> _possibleMoves = new();

        public Player CurrentPlayer { get; private set; } = Player.One;

        private GameState _state = GameState.MainMenu;
        private GameState _previousState = GameState.Null;
        private bool _stateChanged = true;

        private readonly BoardGenerator _boardGenerator = new BoardGenerator();

        private int _playerOneScore = 0;
        private int _playerTwoScore = 0;

        public bool IsPlaying => State == GameState.PlacingMarbleP1 || State == GameState.PlacingMarbleP2 || State == GameState.BetweenTurns;

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

        public GameStateInfo StateInfo => new(State, _previousState, _round, _playerOneScore, _playerTwoScore, Winner);

        public GameState State
        {
            get => _state;
            private set
            {
                _previousState = _state;
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

            DOTween.Init();
        }

        private void Update()
        {
            HandleStateChanged();
            HandleGameOverScreenBehaviour();
        }

        private void HandleGameOverScreenBehaviour()
        {
            if (State == GameState.GameOverScreen || State == GameState.GameOverShowingBoard)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    State = GameState.GameOverShowingBoard;
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    State = GameState.GameOverScreen;
                }
            }
        }

        private void HandleStateChanged()
        {
            if (_stateChanged)
            {
                _stateChanged = false;

                switch (State)
                {
                    case GameState.MainMenu:
                        HandleMainMenuStateChange();
                        break;
                    case GameState.GeneratingBoard:
                        AudioManager.Instance.StopMusic();
                        break;
                    case GameState.PlacingMarbleP1:
                        AudioManager.Instance.PlayGameMusic();
                        CurrentPlayer = Player.One;
                        break;
                    case GameState.BetweenTurns:
                        break;
                    case GameState.PlacingMarbleP2:
                        CurrentPlayer = Player.Two;
                        break;
                    case GameState.GameOverScreen:
                        break;
                    case GameState.GameOverShowingBoard:
                        break;
                    default:
                        throw new NotImplementedException();
                }
                GameEvents.Instance.TriggerStateChangedEvent(StateInfo);
            }
        }

        private void HandleMainMenuStateChange()
        {
            AudioManager.Instance.PlayMenuMusic();

            ResetScores();
        }

        private void ResetScores()
        {
            _playerOneScore = 0;
            _playerTwoScore = 0;
        }

        private void SubscribeToEvents()
        {
            GameEvents.Instance.BoardDrawn += OnBoardDrawn;

            GameEvents.Instance.OnSocketClicked += OnSocketClicked;
        }

        private void OnBoardDrawn()
        {
            State = GameState.PlacingMarbleP1;
        }

        private void DrawBoard()
        {
            GameEvents.Instance.TriggerDrawBoardEvent(new BoardGenerationInfo(_tiles, _sockets));
        }

        private bool GenerateBoard()
        {
            return _boardGenerator.GenerateBoard(ref _tiles, ref _possibleMoves);
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
            State = GameState.BetweenTurns;
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

            if (IsGameEnded)
            {
                State = GameState.GameOverScreen;
            }
            else
            {
                State = CurrentPlayer == Player.One ? GameState.PlacingMarbleP1 : GameState.PlacingMarbleP2;
            }
        }

        private bool IsGameEnded => _round >= _numberOfMarbles * 2 || _possibleMoves.Count == 0;

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
            GameEvents.Instance.TriggerDrawLastPlacedMarbleEvent(CurrentPlayer, clickedSocket.Position);

            // Clear previous possible moves
            GameEvents.Instance.TriggerClearPossibleMovesEvent();

            // Show possible moves
            GameEvents.Instance.TriggerPossibleMovesBroadcastEvent(_possibleMoves);
        }

        private bool ClickedSocketIsNotAllowed(Socket clickedSocket)
        {
            if (State != GameState.PlacingMarbleP1 && State != GameState.PlacingMarbleP2)
            {
                return true;
            }

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

        private void PlaceMarble(Socket socket)
        {
            socket.Owner = CurrentPlayer;
        }

        // Triggered via button in StartMenu
        public void NewGame()
        {
            State = GameState.GeneratingBoard;

            InitializeBoardGenerator();

            bool isBoardGenerated = false;

            while (!isBoardGenerated)
            {
                InitializeVariables();
                InitializeSockets();
                InitializePlayableTiles();
                isBoardGenerated = GenerateBoard();
            }

            DrawBoard();
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

            _possibleMoves = new List<Vector2Int>();
        }

        // Triggered via button in GameOverScreen
        public void BackToStartMenu()
        {
            State = GameState.MainMenu;
        }

        // Triggered via button in StartMenu
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
