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
    private GameObject _ballPrefab;

    [SerializeField]
    public Color PlayerOneColor;
    [SerializeField]
    public Color PlayerTwoColor;

    private GameObject _playerOneBall;
    private GameObject _playerTwoBall;

    [SerializeField]
    private Camera _camera;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        InitializeBalls();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.Instance.OnMouseEnterSocket += OnMouseEnterSocket;
        GameEvents.Instance.OnMouseExitSocket += OnMouseExitSocket;
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

    private void InitializeBalls()
    {
        _playerOneBall = Instantiate(_ballPrefab);
        _playerTwoBall = Instantiate(_ballPrefab);

        _playerOneBall.GetComponentInChildren<SpriteRenderer>().color = PlayerOneColor;
        _playerTwoBall.GetComponentInChildren<SpriteRenderer>().color = PlayerTwoColor;

        _playerOneBall.SetActive(false);
        _playerTwoBall.SetActive(false);
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

    public void DrawBall(Player player, Vector2Int position)
    {
        if (player == Player.One)
        {
            Instantiate(_playerOneBall, position.ToVector3(), Quaternion.identity).SetActive(true);
        }
        else
        {
            Instantiate(_playerTwoBall, position.ToVector3(), Quaternion.identity).SetActive(true);
        }
    }
}
