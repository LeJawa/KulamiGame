using Assets.Scripts;
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

        foreach (UnitTile tile in tiles)
        {
            // Instantiate and Initialize in one line
            // Smells dirty but will reformat later
            Instantiate(_unitTilePrefab, tile.Position.ToVector3(), Quaternion.identity).GetComponent<UnitTileGO>().Initialize(tile);
        }
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
