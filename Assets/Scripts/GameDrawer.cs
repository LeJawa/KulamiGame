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
    private Color _playerOneColor;
    [SerializeField]
    private Color _playerTwoColor;

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
    }

    private void InitializeBalls()
    {
        _playerOneBall = Instantiate(_ballPrefab);
        _playerTwoBall = Instantiate(_ballPrefab);

        _playerOneBall.GetComponentInChildren<SpriteRenderer>().color = _playerOneColor;
        _playerTwoBall.GetComponentInChildren<SpriteRenderer>().color = _playerTwoColor;

        _playerOneBall.SetActive(false);
        _playerTwoBall.SetActive(false);
    }

    public void DrawUnitTiles(UnitTile[] tiles)
    {
        var result = new GameObject[tiles.Length];

        var minX = int.MaxValue;
        var maxX = int.MinValue;
        var minY = int.MaxValue;
        var maxY = int.MinValue;

        foreach (UnitTile tile in tiles)
        {
            // Instantiate and Initialize in one line
            // Smells dirty but will reformat later
            Instantiate(_unitTilePrefab, tile.Position.ToVector3(), Quaternion.identity).GetComponent<UnitTileGO>().Initialize(tile);

            if (minX > tile.Position.x) minX = tile.Position.x;
            if (maxX < tile.Position.x) maxX = tile.Position.x;
            if (minY > tile.Position.y) minY = tile.Position.y;
            if (maxY < tile.Position.y) maxY = tile.Position.y;
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
