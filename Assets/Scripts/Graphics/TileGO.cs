using Kulami.Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Kulami.Graphics
{
    public class TileGO : MonoBehaviour
    {
        private Tile _tileReference;
        private LineRenderer _line;
        [SerializeField]
        private SpriteRenderer _backgroundRenderer;
        [SerializeField]
        private SpriteRenderer _ownershipRenderer;

        public int TileNumber => _tileReference.Number;

        private Player? Owner => _tileReference.Owner;

        [SerializeField] private float _lineWidth = 0.03f;

        [SerializeField] private float _ownershipAlpha = 0.65f;

        private void Start()
        {
            _ownershipRenderer.enabled = false;

            GameEvents.Instance.TileOwnershipUpdated += UpdateOwnership;
        }

        public void Initialize(Tile tile)
        {
            _tileReference = tile;

            _line = GetComponentInChildren<LineRenderer>();
            _line.startWidth = _lineWidth;

            DrawTile();
        }

        private void DrawTile()
        {
            var xmin = _tileReference.GetLeftMostX();
            var xmax = _tileReference.GetRightMostX() + 1;
            var ymin = _tileReference.GetBottomMostY();
            var ymax = _tileReference.GetTopMostY() + 1;

            // Draw background
            _backgroundRenderer.transform.localScale = new Vector3(xmax - xmin, ymax - ymin, 1);
            _backgroundRenderer.transform.localPosition = new Vector3(xmin, ymin, 0);

            // Draw ownership
            _ownershipRenderer.size = new Vector2(xmax - xmin, ymax - ymin);
            _ownershipRenderer.transform.localPosition = new Vector3(xmin, ymin, 0);

            // Draw outline
            _line.positionCount = 5;
            _line.SetPosition(0, new Vector3(xmin, ymin, 0));
            _line.SetPosition(1, new Vector3(xmax, ymin, 0));
            _line.SetPosition(2, new Vector3(xmax, ymax, 0));
            _line.SetPosition(3, new Vector3(xmin, ymax, 0));
            _line.SetPosition(4, new Vector3(xmin, ymin, 0));
        }

        private void UpdateOwnership()
        {
            _ownershipRenderer.enabled = true;

            if (Owner == Player.One)
            {
                var color = GameDrawer.Instance.PlayerOneColor;
                color.a = _ownershipAlpha;
                _ownershipRenderer.color = color;
            }
            else if (Owner == Player.Two) 
            {
                var color = GameDrawer.Instance.PlayerTwoColor;
                color.a = _ownershipAlpha;
                _ownershipRenderer.color = color;                
            }
            else
                _ownershipRenderer.enabled = false;
        }

        void OnDestroy()
        {
            GameEvents.Instance.TileOwnershipUpdated -= UpdateOwnership;
        }
    }
}