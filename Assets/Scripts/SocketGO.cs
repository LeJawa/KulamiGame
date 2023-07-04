using UnityEngine;

namespace Assets.Scripts
{
    public class SocketGO : MonoBehaviour
    {
        private Socket _socketReference;

        private SpriteRenderer _ballSpriteRenderer;
        private SpriteRenderer _previewSpriteRenderer;

        private void Awake()
        {
            _ballSpriteRenderer = transform.Find("Ball").GetComponentInChildren<SpriteRenderer>();
            _previewSpriteRenderer = transform.Find("BallPreview").GetComponentInChildren<SpriteRenderer>();
        }

        public void Initialize(Socket tile)
        {
            _socketReference = tile;

            ShowEmptySocket();
        }

        private void ShowEmptySocket()
        {
            _ballSpriteRenderer.enabled = false;
            _previewSpriteRenderer.enabled = false;
        }

        private void ShowBall(Color color)
        {
            _ballSpriteRenderer.enabled = true;
            _ballSpriteRenderer.color = color;

            HidePreview();
        }

        private void HideBall()
        {
            _ballSpriteRenderer.enabled = false;
        }

        private void ShowPreview(Color color)
        {
            _previewSpriteRenderer.enabled = true;
            _previewSpriteRenderer.color = color;

            HideBall();
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
                    ShowBall(GameDrawer.Instance.PlayerOneColor);
                    break;
                case SocketStatus.OwnedByPlayerTwo:
                    ShowBall(GameDrawer.Instance.PlayerTwoColor);
                    break;
                case SocketStatus.OwnedByPlayerOneHoverPlayerOne:
                    ShowBall(GameDrawer.Instance.PlayerOneColor);
                    break;
                case SocketStatus.OwnedByPlayerOneHoverPlayerTwo:
                    ShowBall(GameDrawer.Instance.PlayerOneColor);
                    break;
                case SocketStatus.OwnedByPlayerTwoHoverPlayerOne:
                    ShowBall(GameDrawer.Instance.PlayerTwoColor);
                    break;
                case SocketStatus.OwnedByPlayerTwoHoverPlayerTwo:
                    ShowBall(GameDrawer.Instance.PlayerTwoColor);
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