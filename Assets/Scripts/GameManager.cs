using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int _numberOf6Tiles = 4;
        [SerializeField]
        private int _numberOf4Tiles = 4;
        [SerializeField]
        private int _numberOf3Tiles = 4;
        [SerializeField]
        private int _numberOf2Tiles = 4;


        private UnitTile[] _individualTiles;
        private Tile[] _playableTiles;

        private Vector2Int _startingPosition = Vector2Int.zero;

        public void Start()
        {
            InitializePlayableTiles();

            InitializeIndividualTiles();

            GenerateBoard();
        }

        private void GenerateBoard()
        {
            // Assign unit tiles to playable tiles
            // Try to position the playable tile
                // Select a random orientation for the playable tile
                // take a possible position at random (first round only 0,0 )
                // check if playable tile can be placed here
                    // if no, take another possible position at random
                // place all unit tiles
                // create list of possible new positions (positions surrounding existing positions)




            int individualTilesIndex = 0;

            List<Vector2Int> possiblePositions = new List<Vector2Int> { _startingPosition };

            foreach (var tile in _playableTiles)
            {
                possiblePositions.Shuffle();

                int number = tile.Number;

                UnitTile[] unitTileArray = new UnitTile[number];

                for (int i = 0; i < number; i++)
                {
                    _individualTiles[individualTilesIndex] = new UnitTile();
                    unitTileArray[i] = _individualTiles[individualTilesIndex];

                    individualTilesIndex++;
                }

                tile.InitializeTile(unitTileArray);
            }
        }

        private void InitializePlayableTiles()
        {
            int totalNumberOfPlayableTiles =
                              _numberOf2Tiles
                            + _numberOf3Tiles
                            + _numberOf4Tiles
                            + _numberOf6Tiles;

            _playableTiles = new Tile[totalNumberOfPlayableTiles];

            int index = 0;

            for (int i = 0; i < _numberOf2Tiles; i++)
            {
                _playableTiles[index++] = new Tile(2);
            }

            for (int i = 0; i < _numberOf3Tiles; i++)
            {
                _playableTiles[index++] = new Tile(3);
            }

            for (int i = 0; i < _numberOf4Tiles; i++)
            {
                _playableTiles[index++] = new Tile(4);
            }

            for (int i = 0; i < _numberOf6Tiles; i++)
            {
                _playableTiles[index++] = new Tile(6);
            }
        }

        private void InitializeIndividualTiles()
        {
            int totalNumberOfUnitTiles =
                              _numberOf2Tiles * 2
                            + _numberOf3Tiles * 3
                            + _numberOf4Tiles * 4
                            + _numberOf6Tiles * 6;

            _individualTiles = new UnitTile[totalNumberOfUnitTiles];
        }
    }
}
