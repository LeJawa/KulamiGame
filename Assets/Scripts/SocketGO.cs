using UnityEngine;

namespace Assets.Scripts
{
    public class SocketGO : MonoBehaviour
    {
        private Socket _socketReference;

        private SpriteRenderer _marbleSpriteRenderer;
        private SpriteRenderer _previewSpriteRenderer;

        private void Awake()
        {
            _marbleSpriteRenderer = transform.Find("Marble").GetComponentInChildren<SpriteRenderer>();
            _previewSpriteRenderer = transform.Find("MarblePreview").GetComponentInChildren<SpriteRenderer>();
        }

        public void Initialize(Socket tile)
        {
            _socketReference = tile;

            ShowEmptySocket();
        }

        private void ShowEmptySocket()
        {
            _marbleSpriteRenderer.enabled = false;
            _previewSpriteRenderer.enabled = false;
        }

        private void ShowMarble(Color color)
        {
            _marbleSpriteRenderer.enabled = true;
            _marbleSpriteRenderer.color = color;

            HidePreview();
        }

        private void HideMarble()
        {
            _marbleSpriteRenderer.enabled = false;
        }

        private void ShowPreview(Color color)
        {
            _previewSpriteRenderer.enabled = true;
            _previewSpriteRenderer.color = color;

            HideMarble();
        }

        private void HidePreview()
        {
              _previewSpriteRenderer.enabled = false;
        }

        public void OnMouseUpAsButton()
        {
            GameManager.Instance.GameTileClickedEvent(_socketReference);
        }

        public void OnMouseEnter()
        {
            GameEvents.Instance.TriggerMouseEnterSocketEvent(this);
        }

        public void OnMouseExit()
        {
            GameEvents.Instance.TriggerMouseExitSocketEvent(this);
        }

        public Player? Owner => _socketReference.Owner;

        public void SetStatus(SocketStatus status)
        {
            switch (status)
            {
                case SocketStatus.Empty:
                    ShowEmptySocket();
                    break;
                case SocketStatus.OwnedByPlayerOne:
                    ShowMarble(GameDrawer.Instance.PlayerOneColor);
                    break;
                case SocketStatus.OwnedByPlayerTwo:
                    ShowMarble(GameDrawer.Instance.PlayerTwoColor);
                    break;
                case SocketStatus.OwnedByPlayerOneHoverPlayerOne:
                    ShowMarble(GameDrawer.Instance.PlayerOneColor);
                    break;
                case SocketStatus.OwnedByPlayerOneHoverPlayerTwo:
                    ShowMarble(GameDrawer.Instance.PlayerOneColor);
                    break;
                case SocketStatus.OwnedByPlayerTwoHoverPlayerOne:
                    ShowMarble(GameDrawer.Instance.PlayerTwoColor);
                    break;
                case SocketStatus.OwnedByPlayerTwoHoverPlayerTwo:
                    ShowMarble(GameDrawer.Instance.PlayerTwoColor);
                    break;
                case SocketStatus.PossibleMoveByPlayerOne:
                    ShowPreview(GameDrawer.Instance.PlayerOneColor);
                    break;
                case SocketStatus.PossibleMoveByPlayerTwo:
                    ShowPreview(GameDrawer.Instance.PlayerTwoColor);
                    break;
                case SocketStatus.EmptyHoverPlayerOne:
                    ShowPreview(GameDrawer.Instance.PlayerOneColor);
                    break;
                case SocketStatus.EmptyHoverPlayerTwo:
                    ShowPreview(GameDrawer.Instance.PlayerTwoColor);
                    break;
            }
        }

    }
}