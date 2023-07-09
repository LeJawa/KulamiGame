using Kulami.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Graphics
{
    public class GameDrawer : MonoBehaviour
    {
        public static GameDrawer Instance { get; private set; }

        [SerializeField]
        private GameObject _socketPrefab;

        [SerializeField]
        private GameObject _tilePrefab;

        [SerializeField]
        private GameObject _marblePrefab;

        [SerializeField]
        private GameObject _possibleMovePrefab;

        private GameObject _marblePreviewPlayerOne;
        private GameObject _marblePreviewPlayerTwo;

        [SerializeField]
        private GameObject _lastMovePrefab;

        [SerializeField]
        public Color PlayerOneColor;
        [SerializeField]
        public Color PlayerTwoColor;
        [SerializeField]
        public Color NeutralColor;

        private GameObject _playerOneMarble;
        private GameObject _playerTwoMarble;

        private GameObject _playerOneLastMove;
        private GameObject _playerTwoLastMove;

        [SerializeField]
        private Camera _camera;

        private List<GameObject> _possibleMoveGameObjects = new List<GameObject>();

        [SerializeField]
        private OuterRim _outerRim;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            SubscribeToEvents();
        }

        public void Initialize()
        {
            ClearAllGameComponents();

            InitializeMarbles();
            InitializeLastMoves();

            InitializeMarblePreview();
        }

        private void InitializeMarblePreview()
        {
            _marblePreviewPlayerOne = Instantiate(_possibleMovePrefab);
            _marblePreviewPlayerOne.GetComponentInChildren<SpriteRenderer>().color = PlayerOneColor;
            _marblePreviewPlayerOne.GetComponentInChildren<SpriteRenderer>().sortingOrder = 21; // To be drawn on top of the possible moves

            _marblePreviewPlayerTwo = Instantiate(_marblePreviewPlayerOne);
            _marblePreviewPlayerTwo.GetComponentInChildren<SpriteRenderer>().color = PlayerTwoColor;

            HideMarblePreviews();
        }

        private void InitializeLastMoves()
        {
            _playerOneLastMove = Instantiate(_lastMovePrefab);
            _playerTwoLastMove = Instantiate(_lastMovePrefab);

            _playerOneLastMove.GetComponentInChildren<SpriteRenderer>().color = PlayerOneColor;
            _playerTwoLastMove.GetComponentInChildren<SpriteRenderer>().color = PlayerTwoColor;

            _playerOneLastMove.transform.position = new Vector3(-100, -100, -100);
            _playerTwoLastMove.transform.position = new Vector3(-100, -100, -100);
        }

        private void SubscribeToEvents()
        {
            GameEvents.Instance.OnMouseEnterSocket += OnMouseEnterSocket;
            GameEvents.Instance.OnMouseExitSocket += OnMouseExitSocket;

            GameEvents.Instance.PossibleMovesBroadcast += OnPossibleMovesBroadcast;
            GameEvents.Instance.ClearPossibleMoves += OnClearPossibleMoves;

            GameEvents.Instance.DrawLastPlacedMarble += OnDrawLastPlacedMarble;

            GameEvents.Instance.DrawBoard += OnDrawBoard;

            GameEvents.Instance.StateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStateInfo info)
        {
            switch (info.State)
            {
                case GameState.MainMenu:
                    ShowStartMenu();
                    HideGameOverScreen();
                    _outerRim.Hide();
                    break;
                case GameState.GeneratingBoard:
                    HideStartMenu();
                    HideGameOverScreen();
                    _outerRim.Hide();
                    break;
                case GameState.PlacingMarbleP1:
                    HideStartMenu();
                    HideGameOverScreen();
                    _outerRim.SetPlayer(Player.One);
                    break;
                case GameState.BetweenTurns:
                    HideStartMenu();
                    HideGameOverScreen();
                    _outerRim.Hide();
                    break;
                case GameState.PlacingMarbleP2:
                    HideStartMenu();
                    HideGameOverScreen();
                    _outerRim.SetPlayer(Player.Two);
                    break;
                case GameState.GameOverScreen:
                    HideStartMenu();
                    ShowGameOverScreen(info.Winner, info.Player1Score, info.Player2Score);
                    _outerRim.Hide();
                    break;
                case GameState.GameOverShowingBoard:
                    HideStartMenu();
                    HideGameOverScreen();
                    _outerRim.Hide();
                    break;
                default:
                    throw new Exception("Unknown game state");
            }
        }

        private void OnDrawBoard(BoardGenerationInfo info)
        {
            Initialize();

            foreach (var tile in info.Tiles)
            {
                DrawGameTile(tile);
            }

            DrawSockets(info.Sockets);

            GameEvents.Instance.TriggerBoardDrawnEvent();
        }

        private void OnDrawLastPlacedMarble(Player player, Vector2Int position)
        {
            if (player == Player.One)
            {
                Instantiate(_playerOneMarble, position.ToVector3(), Quaternion.identity).SetActive(true);
                _playerOneLastMove.transform.position = position.ToVector3();
                _playerOneLastMove.SetActive(true);
            }
            else
            {
                Instantiate(_playerTwoMarble, position.ToVector3(), Quaternion.identity).SetActive(true);
                _playerTwoLastMove.transform.position = position.ToVector3();
                _playerTwoLastMove.SetActive(true);
            }
        }

        private void OnClearPossibleMoves()
        {
            foreach (var possibleMove in _possibleMoveGameObjects)
            {
                Destroy(possibleMove);
            }
        }

        private void OnPossibleMovesBroadcast(List<Vector2Int> positionList)
        {
            foreach (var position in positionList)
            {
                var possibleMove = Instantiate(_possibleMovePrefab, position.ToVector3(), Quaternion.identity);
                _possibleMoveGameObjects.Add(possibleMove);
            }
        }

        private void OnMouseExitSocket(SocketGO socketGO)
        {
            HideMarblePreviews();
        }

        private void OnMouseEnterSocket(SocketGO socketGO)
        {
            if (GameManager.Instance.State != GameState.PlacingMarbleP1 && GameManager.Instance.State != GameState.PlacingMarbleP2) return;

            var currentPlayer = GameManager.Instance.CurrentPlayer;
            var socketOwner = socketGO.Owner;

            if (socketOwner != null)
                return;

            ShowMarblePreview(currentPlayer, socketGO.Position);
        }

        private void ShowMarblePreview(Player? currentPlayer, Vector2Int position)
        {
            if(currentPlayer == null)
            {
                return;
            }

            if (currentPlayer == Player.One)
            {
                _marblePreviewPlayerOne.transform.position = position.ToVector3();
            }
            else
            {
                _marblePreviewPlayerTwo.transform.position = position.ToVector3();
            }
        }

        private void HideMarblePreviews()
        {
            _marblePreviewPlayerOne.transform.position = new Vector3(-100, -100, -100);
            _marblePreviewPlayerTwo.transform.position = new Vector3(-100, -100, -100);
        }

        private void InitializeMarbles()
        {
            _playerOneMarble = Instantiate(_marblePrefab);
            _playerTwoMarble = Instantiate(_marblePrefab);

            _playerOneMarble.GetComponentInChildren<SpriteRenderer>().color = PlayerOneColor;
            _playerTwoMarble.GetComponentInChildren<SpriteRenderer>().color = PlayerTwoColor;

            _playerOneMarble.transform.position = new Vector3(-100, -100, -100);
            _playerTwoMarble.transform.position = new Vector3(-100, -100, -100);
        }

        public void DrawSockets(Socket[] sockets)
        {
            var result = new GameObject[sockets.Length];

            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            foreach (Socket socket in sockets)
            {
                // Instantiate and Initialize in one line
                // Smells dirty but will reformat later
                Instantiate(_socketPrefab, socket.Position.ToVector3(), Quaternion.identity).GetComponent<SocketGO>().Initialize(socket);

                if (minX > socket.Position.x) minX = socket.Position.x;
                if (maxX < socket.Position.x) maxX = socket.Position.x;
                if (minY > socket.Position.y) minY = socket.Position.y;
                if (maxY < socket.Position.y) maxY = socket.Position.y;
            }

            // We add maxX and minX together because minX is always negative
            float meanX = (maxX + 1 + minX) / 2f;
            float meanY = (maxY + 1 + minY) / 2f;

            SetCamera(meanX, meanY);
        }

        private void SetCamera(float centerX, float centerY)
        {
            _camera.transform.position = new Vector3(centerX, centerY, _camera.transform.position.z);
        }

        public void DrawGameTile(Tile tile)
        {
            Instantiate(_tilePrefab).GetComponent<TileGO>().Initialize(tile);
        }

        [SerializeField]
        private GameObject _startMenu;

        [SerializeField]
        private GameOverScreen _gameOverScreen;



        public void HideStartMenu()
        {
            _startMenu.SetActive(false);
        }

        public void ShowStartMenu()
        {
            _gameOverScreen.Hide();
            _startMenu.SetActive(true);
        }

        public void ClearAllGameComponents()
        {
            foreach (var gameObject in GameObject.FindGameObjectsWithTag("GameComponent"))
            {
                Destroy(gameObject);
            }
        }

        public void ShowGameOverScreen(Player? winner, int playerOnePoints, int playerTwoPoints)
        {
            var winnerText = "";

            if (winner == null)
            {
                winnerText = "It's a tie!";
            }
            else
            {
                winnerText = winner == Player.One ? "Player One Wins!" : "Player Two Wins!";
            }

            _gameOverScreen.Show(winnerText, playerOnePoints, playerTwoPoints);
        }

        public void HideGameOverScreen()
        {
            _gameOverScreen.gameObject.SetActive(false);
        }

        public void ShowGameOverScreen()
        {
            _gameOverScreen.gameObject.SetActive(true);
        }
    }
}