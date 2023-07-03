using UnityEngine;

namespace Assets.Scripts
{
    public class SocketGO : MonoBehaviour
    {
        private Socket _socketReference;



        internal void Initialize(Socket tile)
        {
            _socketReference = tile;
        }

        public void OnMouseUpAsButton()
        {
            GameManager.Instance.GameTileClickedEvent(_socketReference);
        }

        public void OnMouseEnter()
        {
            GameEvents.Instance.TriggerMouseEnterSocketEvent(this);
        }

        public Player? Owner => _socketReference.Owner;

        public void SetStatus(SocketStatus status)
        {
            switch (status)
            {
                case SocketStatus.Empty:
                    break;
                case SocketStatus.OwnedByPlayerOne:
                    break;
                case SocketStatus.OwnedByPlayerTwo:
                    break;
                case SocketStatus.OwnedByPlayerOneHoverPlayerOne:
                    break;
                case SocketStatus.OwnedByPlayerOneHoverPlayerTwo:
                    break;
                case SocketStatus.OwnedByPlayerTwoHoverPlayerOne:
                    break;
                case SocketStatus.OwnedByPlayerTwoHoverPlayerTwo:
                    break;
                case SocketStatus.PossibleMoveByPlayerOne:
                    break;
                case SocketStatus.PossibleMoveByPlayerTwo:
                    break;
                case SocketStatus.EmptyHoverPlayerOne:
                    break;
                case SocketStatus.EmptyHoverPlayerTwo:
                    break;
            }
        }

    }
}