using UnityEngine;

namespace Assets.Scripts
{
    public class UnitTileGO : MonoBehaviour
    {
        private UnitTile _unitTileReference;

        internal void Initialize(UnitTile tile)
        {
            _unitTileReference = tile;
        }

        public void OnMouseUpAsButton()
        {
            GameManager.Instance.GameTileClickedEvent(_unitTileReference);
        }
    }
}