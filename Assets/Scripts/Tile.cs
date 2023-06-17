using UnityEngine;

namespace Assets.Scripts
{
    public class Tile : MonoBehaviour
    {
        private UnitTile[] _unitTiles;
        public int Number { get; private set; }

        public Tile(int numberOfUnitTiles)
        {
            Number = numberOfUnitTiles;
        }

        public void InitializeTile(UnitTile[] unitTiles)
        {
            _unitTiles = unitTiles;
        }

    }
}
