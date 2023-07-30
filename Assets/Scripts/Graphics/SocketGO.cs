using Kulami.Game;
using UnityEngine;

namespace Kulami.Graphics
{
    public class SocketGO : MonoBehaviour
    {
        private Socket _socketReference;

        public Player? Owner => _socketReference.Owner;

        public Vector2Int Position => _socketReference.Position;

        public void Initialize(Socket tile)
        {
            _socketReference = tile;
        }

        public void OnMouseUpAsButton()
        {
            GameEvents.Instance.TriggerSocketClickedEvent(_socketReference);
        }

        public void OnMouseEnter()
        {
            GameEvents.Instance.TriggerMouseEnterSocketEvent(this);
        }

        public void OnMouseExit()
        {
            GameEvents.Instance.TriggerMouseExitSocketEvent(this);
        }
    }
}