using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDrawer : MonoBehaviour
{
    public static GameDrawer Instance { get; private set; }

    [SerializeField]
    private GameObject _unitTilePrefab;

    [SerializeField]
    private GameObject _tilePrefab;

    [SerializeField]
    private GameObject _marblePrefab;

    [SerializeField]
    private GameObject _possibleMovePrefab;

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
        InitializeMarbles();
        InitializeLastMoves();
    }

    private void InitializeLastMoves()
    {
        _playerOneLastMove = Instantiate(_lastMovePrefab);
        _playerTwoLastMove = Instantiate(_lastMovePrefab);

        _playerOneLastMove.GetComponentInChildren<SpriteRenderer>().color = PlayerOneColor;
        _playerTwoLastMove.GetComponentInChildren<SpriteRenderer>().color = PlayerTwoColor;

        _playerOneLastMove.SetActive(false);
        _playerTwoLastMove.SetActive(false);
    }

    private void SubscribeToEvents()
    {
        GameEvents.Instance.OnMouseEnterSocket += OnMouseEnterSocket;
        GameEvents.Instance.OnMouseExitSocket += OnMouseExitSocket;

        GameEvents.Instance.PossibleMovesBroadcast += OnPossibleMovesBroadcast;
        GameEvents.Instance.ClearPossibleMoves += OnClearPossibleMoves;

        GameEvents.Instance.SetPlayerLastMove += OnSetPlayerLastMove;
    }

    private void OnSetPlayerLastMove(Player player, Vector2Int position)
    {
        if (player == Player.One)
        {
            _playerOneLastMove.transform.position = position.ToVector3();
            _playerOneLastMove.SetActive(true);
        }
        else
        {
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
        var socketOwner = socketGO.Owner;

        SocketStatus status = SocketStatus.Empty;

        if (socketOwner != null)
        {
            status = socketOwner == Player.One ? SocketStatus.OwnedByPlayerOne : SocketStatus.OwnedByPlayerTwo;
        }

        socketGO.SetStatus(status);
    }

    private void OnMouseEnterSocket(SocketGO socketGO)
    {
        var currentPlayer = GameManager.Instance.CurrentPlayer;
        var socketOwner = socketGO.Owner;

        SocketStatus status = SocketStatus.Empty;

        if (socketOwner == null)
        {
            status = currentPlayer == Player.One ? SocketStatus.EmptyHoverPlayerOne : SocketStatus.EmptyHoverPlayerTwo;
        }
        else if (socketOwner == Player.One)
        {
            status = currentPlayer == Player.One ? SocketStatus.OwnedByPlayerOneHoverPlayerOne : SocketStatus.OwnedByPlayerOneHoverPlayerTwo;
        }
        else
        {
            status = currentPlayer == Player.One ? SocketStatus.OwnedByPlayerTwoHoverPlayerOne : SocketStatus.OwnedByPlayerTwoHoverPlayerTwo;
        }

        socketGO.SetStatus(status);
    }

    private void InitializeMarbles()
    {
        _playerOneMarble = Instantiate(_marblePrefab);
        _playerTwoMarble = Instantiate(_marblePrefab);

        _playerOneMarble.GetComponentInChildren<SpriteRenderer>().color = PlayerOneColor;
        _playerTwoMarble.GetComponentInChildren<SpriteRenderer>().color = PlayerTwoColor;

        _playerOneMarble.SetActive(false);
        _playerTwoMarble.SetActive(false);
    }

    public void DrawUnitTiles(Socket[] sockets)
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
            Instantiate(_unitTilePrefab, socket.Position.ToVector3(), Quaternion.identity).GetComponent<SocketGO>().Initialize(socket);

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

    public void DrawMarble(Player player, Vector2Int position)
    {
        if (player == Player.One)
        {
            Instantiate(_playerOneMarble, position.ToVector3(), Quaternion.identity).SetActive(true);
        }
        else
        {
            Instantiate(_playerTwoMarble, position.ToVector3(), Quaternion.identity).SetActive(true);
        }
    }

    [SerializeField]
    private GameObject _startMenu;

    public void HideStartMenu()
    {
        _startMenu.SetActive(false);
    }

    public void ShowStartMenu()
    {
        _startMenu.SetActive(true);
    }

    public void ClearAllGameComponents()
    {
        foreach (var gameObject in GameObject.FindGameObjectsWithTag("GameComponent"))
        {
            Destroy(gameObject);
        }
    }
}
