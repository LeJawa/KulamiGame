using Kulami.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        private GameObject _playerOneMarblePrefab;
        private GameObject _playerTwoMarblePrefab;


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

        private GameObject _playerOneLastMove;
        private GameObject _playerTwoLastMove;

        [SerializeField]
        private Camera _camera;

        private List<GameObject> _possibleMoveGameObjects = new List<GameObject>();

        [SerializeField]
        private GameObject _startMenu;

        [SerializeField]
        private GameOverScreen _gameOverScreen;

        [SerializeField]
        private GameUI _gameUI;

        private List<TileGO> _tileGOs;

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

            InitializeMarblePrefabs();

            InitializeLastMoves();

            InitializeMarblePreview();
        }

        private void InitializeMarblePrefabs()
        {
            _playerOneMarblePrefab = Instantiate(_marblePrefab);
            _playerOneMarblePrefab.GetComponentInChildren<MarbleGO>().SetPlayer(Player.One);
            _playerOneMarblePrefab.transform.position = new Vector3(-100, -100, -100);

            _playerTwoMarblePrefab = Instantiate(_marblePrefab);
            _playerTwoMarblePrefab.GetComponentInChildren<MarbleGO>().SetPlayer(Player.Two);
            _playerTwoMarblePrefab.transform.position = new Vector3(-100, -100, -100);
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
            UpdateScores(info.Player1Score, info.Player2Score);

            switch (info.State)
            {
                case GameState.MainMenu:
                    ShowStartMenu();
                    HideGameOverScreen();
                    _gameUI.Hide();
                    break;
                case GameState.GeneratingBoard:
                    HideStartMenu();
                    HideGameOverScreen();
                    _gameUI.Hide();
                    break;
                case GameState.PlacingMarbleP1:
                    HideStartMenu();
                    HideGameOverScreen();
                    _gameUI.CurrentPlayer = Player.One;
                    break;
                case GameState.BetweenTurns:
                    HideStartMenu();
                    HideGameOverScreen();
                    _gameUI.Hide();
                    break;
                case GameState.PlacingMarbleP2:
                    HideStartMenu();
                    HideGameOverScreen();
                    _gameUI.CurrentPlayer = Player.Two;
                    break;
                case GameState.GameOverScreen:
                    HideStartMenu();
                    ShowGameOverScreen(info.Winner, info.Player1Score, info.Player2Score);
                    _gameUI.Hide();
                    break;
                case GameState.GameOverShowingBoard:
                    HideStartMenu();
                    HideGameOverScreen();
                    _gameUI.Hide();
                    break;
                default:
                    throw new Exception("Unknown game state");
            }
        }

        private void UpdateScores(int player1Score, int player2Score)
        {
            _gameUI.PlayerOneScore = player1Score;
            _gameUI.PlayerTwoScore = player2Score;
        }

        private void OnDrawBoard(BoardGenerationInfo info)
        {
            Initialize();

            _tileGOs = new();

            foreach (var tile in info.Tiles)
            {
                var tileGO = DrawGameTile(tile);
                DrawSockets(tile.Sockets, tileGO);
                _tileGOs.Add(tileGO);
            }

            SetCamera(info.Sockets);

            AnimateBoardGeneration();
        }

        public TileGO DrawGameTile(Tile tile)
        {
            var tileGO = Instantiate(_tilePrefab).GetComponent<TileGO>();
            tileGO.Initialize(tile);
            return tileGO;
        }

        public void DrawSockets(Socket[] sockets, TileGO parentTile)
        {

            foreach (Socket socket in sockets)
            {
                // Instantiate and Initialize in one line
                // Smells dirty but will reformat later
                var socketGO = Instantiate(_socketPrefab, socket.Position.ToVector3(), Quaternion.identity).GetComponent<SocketGO>();
                socketGO.transform.SetParent(parentTile.transform);
                socketGO.Initialize(socket);
            }
        }

        [SerializeField] private float _offCameraDistance = 15f;
        [SerializeField] private float _delayBetweenTiles = 1f;
        [SerializeField] private float _tileMoveTime = 0.5f;
        [SerializeField] private float _delayReductionPercentage = 0.7f;
        [SerializeField] private float _minDelayBetweenTiles = 0.05f;
        [SerializeField] private float _timeBeforeBoardDrawnEvent = 1f;

        private void AnimateBoardGeneration()
        {
            // Initialize the tiles to be off screen
            var tilePosition = Vector3.right * _offCameraDistance;
            var rotationAmount = 360f / _tileGOs.Count;

            foreach (var tile in _tileGOs)
            {
                tile.transform.position = tilePosition;
                tilePosition = Quaternion.Euler(0, 0, rotationAmount) * tilePosition;
            }

            StartCoroutine(AnimateBoardGenerationCoroutine());
        }

        private IEnumerator AnimateBoardGenerationCoroutine()
        {
            var delay = _delayBetweenTiles;

            foreach (var tile in _tileGOs)
            {
                LeanTween.move(tile.gameObject, Vector3.zero, _tileMoveTime).setEaseOutBack();

                yield return new WaitForSeconds(delay);
                //tile.transform.position = Vector3.zero;
                delay *= _delayReductionPercentage;
            }

            yield return new WaitForSeconds(_timeBeforeBoardDrawnEvent);
            GameEvents.Instance.TriggerBoardDrawnEvent();
        }


        private void OnDrawLastPlacedMarble(Player player, Vector2Int position)
        {
            if (player == Player.One)
            {
                Instantiate(_playerOneMarblePrefab, position.ToVector3(), Quaternion.identity);
                _playerOneLastMove.transform.position = position.ToVector3();
            }
            else
            {
                Instantiate(_playerTwoMarblePrefab, position.ToVector3(), Quaternion.identity);
                _playerTwoLastMove.transform.position = position.ToVector3();
            }

            GameEvents.Instance.TriggerDrawMarbleShadowEvent(position.ToVector3());
            GameEvents.Instance.TriggerTileOwnershipUpdatedEvent();
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

        private void SetCamera(Socket[] sockets)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            foreach (Socket socket in sockets)
            {
                if (minX > socket.Position.x) minX = socket.Position.x;
                if (maxX < socket.Position.x) maxX = socket.Position.x;
                if (minY > socket.Position.y) minY = socket.Position.y;
                if (maxY < socket.Position.y) maxY = socket.Position.y;
            }

            // We add maxX and minX together because minX is always negative
            float meanX = (maxX + 1 + minX) / 2f;
            float meanY = (maxY + 1 + minY) / 2f;

            _camera.transform.position = new Vector3(meanX, meanY, _camera.transform.position.z);
        }

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
    }
}