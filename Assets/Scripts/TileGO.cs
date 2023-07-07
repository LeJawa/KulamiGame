using UnityEngine;

namespace Assets.Scripts
{
    public class TileGO : MonoBehaviour
    {
        private Tile _tileReference;
        private LineRenderer _line;

        [SerializeField] public int TileNumber = 0;

        public void Initialize(Tile tile)
        {
            _tileReference = tile;
            DrawOutline();

            TileNumber = tile.Number;
        }

        private void DrawOutline()
        {
            _line = GetComponentInChildren<LineRenderer>();

            var xmin = _tileReference.GetLeftMostX();
            var xmax = _tileReference.GetRightMostX() + 1;
            var ymin = _tileReference.GetBottomMostY();
            var ymax = _tileReference.GetTopMostY() + 1;

            _line.positionCount = 5;
            _line.SetPosition(0, new Vector3(xmin, ymin, 0));
            _line.SetPosition(1, new Vector3(xmax, ymin, 0));
            _line.SetPosition(2, new Vector3(xmax, ymax, 0));
            _line.SetPosition(3, new Vector3(xmin, ymax, 0));
            _line.SetPosition(4, new Vector3(xmin, ymin, 0));
        }
    }
}