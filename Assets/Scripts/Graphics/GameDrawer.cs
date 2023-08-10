using DG.Tweening;
using Kulami.Data;
using Kulami.Game;
using Kulami.Helpers;
using Kulami.Control;
using Kulami.UI;
using System;
using System.Collections;
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
        private CameraController _camera;

        private List<GameObject> _possibleMoveGameObjects = new List<GameObject>();

        [SerializeField]
        private GameObject _startMenu;

        [SerializeField]
        private GameOverScreen _gameOverScreen;

        private List<TileGO> _tileGOs;

        [SerializeField] SceneTransition _sceneTransition;

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
#if UNITY_EDITOR
            if (InputManager.Instance.GetTestDown())
            {
                OnDrawBoard(_lastBoardGenerationInfo);
            }   
#endif
        }

        public void Initialize()
        {
            ClearAllGameComponents();

            InitializeMarblePrefabs();

            InitializeLastMoves();

            InitializeMarblePreview();

            GameUI.Instance.Initialize();
        }

        private void InitializeMarblePrefabs()
        {
            _marblePrefab.SetActive(false);

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
                    ShowStartMenu(info.PreviousState);
                    HideGameOverScreen();
                    GameUI.Instance.Hide();
                    break;
                case GameState.GeneratingBoard:
                    //HideStartMenu(); This will be hidden at the middle of the transition time
                    HideGameOverScreen();
                    GameUI.Instance.Hide();
                    break;
                case GameState.PlacingMarbleP1:
                    HideStartMenu();
                    HideGameOverScreen();
                    GameUI.Instance.CurrentPlayer = Player.One;
                    break;
                case GameState.BetweenTurns:
                    HideStartMenu();
                    HideGameOverScreen();
                    break;
                case GameState.PlacingMarbleP2:
                    HideStartMenu();
                    HideGameOverScreen();
                    GameUI.Instance.CurrentPlayer = Player.Two;
                    break;
                case GameState.GameOverScreen:
                    HideStartMenu();
                    ShowGameOverScreen(info.Winner, info.PreviousState);
                    GameUI.Instance.Show();
                    break;
                case GameState.GameOverShowingBoard:
                    HideStartMenu();
                    HideGameOverScreen();
                    GameUI.Instance.Hide();
                    break;
                default:
                    throw new Exception("Unknown game state");
            }
        }

        private void UpdateScores(int player1Score, int player2Score)
        {
            GameUI.Instance.PlayerOneScore = player1Score;
            GameUI.Instance.PlayerTwoScore = player2Score;
        }
        private BoardGenerationInfo _lastBoardGenerationInfo;
        private void OnDrawBoard(BoardGenerationInfo info)
        {

            _lastBoardGenerationInfo = info;
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
                var socketGO = Instantiate(_socketPrefab, socket.Position.ToVector3(), Quaternion.identity).GetComponent<SocketGO>();
                socketGO.transform.SetParent(parentTile.transform);
                socketGO.Initialize(socket);
            }
        }

        [SerializeField] private float _offCameraDistance = 15f;
        [SerializeField] private float _delayBetweenTiles = 1f;
        [SerializeField] private float _tileMoveTime = 0.3f;
        [SerializeField] private float _delayReductionPercentage = 0.7f;
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

        [SerializeField]
        private bool _animateBoard = true;

        private IEnumerator AnimateBoardGenerationCoroutine()
        {
            _sceneTransition.Play();

            yield return new WaitForSeconds(_sceneTransition.Duration / 2);
            HideStartMenu();
            yield return new WaitForSeconds(_sceneTransition.Duration / 2);

            var delay = _delayBetweenTiles;

            foreach (var tile in _tileGOs)
            {
                if (_animateBoard)
                {
                    tile.transform.DOMove(Vector3.zero, _tileMoveTime).SetEase(Ease.OutBack);
                    AudioManager.Instance.PlayTileMovedSound();

                    yield return new WaitForSeconds(delay);
                    delay *= _delayReductionPercentage;
                }
                else
                {
                    tile.transform.position = Vector3.zero;
                }
            }

            yield return new WaitForSeconds(_timeBeforeBoardDrawnEvent);
            GameEvents.Instance.TriggerBoardDrawnEvent();
        }


        private void OnDrawLastPlacedMarble(Player player, Vector2Int position)
        {
            if (player == Player.One)
            {
                Instantiate(_playerOneMarblePrefab, position.ToVector3(), Quaternion.identity).SetActive(true);
                _playerOneLastMove.transform.position = position.ToVector3();
            }
            else
            {
                Instantiate(_playerTwoMarblePrefab, position.ToVector3(), Quaternion.identity).SetActive(true);
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
            if (GameOptions.Instance.ShowPossibleMoves == false) return;

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

            _camera.SetPosition(new Vector2(meanX, meanY));
        }

        private void HideStartMenu()
        {
            _startMenu.SetActive(false);
        }

        private void ShowStartMenu(GameState previousState)
        {
            if (previousState != GameState.Null)
            {
                StartCoroutine(TransitionToStartMenu());
            }
        }

        private IEnumerator TransitionToStartMenu()
        {

            _sceneTransition.Play();

            yield return new WaitForSeconds(_sceneTransition.Duration / 2);
            AudioManager.Instance.PlayMenuMusic();
            _gameOverScreen.Hide();
            _startMenu.SetActive(true);
        }

        private void ClearAllGameComponents()
        {
            foreach (var gameObject in GameObject.FindGameObjectsWithTag("GameComponent"))
            {
                Destroy(gameObject);
            }
        }

        private void ShowGameOverScreen(Player? winner, GameState previousState)
        {
            var winnerText = "";
            var winnerTextColor = NeutralColor;

            if (winner == null)
            {
                winnerText = "It's a tie!";
            }
            else
            {
                winnerText = winner == Player.One ? "Player One Wins!" : "Player Two Wins!";
                winnerTextColor = winner == Player.One ? PlayerOneColor : PlayerTwoColor;
            }

            if (previousState == GameState.GameOverShowingBoard)
            {
                _gameOverScreen.ShowWithoutAnimation();
            }
            else
            {
                _gameOverScreen.Show(winnerText, winnerTextColor);
            }

        }

        private void HideGameOverScreen()
        {
            _gameOverScreen.gameObject.SetActive(false);
        }
    }
}