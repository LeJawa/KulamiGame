using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class GameEvents
    {
        #region Singleton pattern
        private static GameEvents _current;
        public static GameEvents Instance => _current ??= new GameEvents();
        #endregion

        #region Action<SocketGO> OnMouseEnterSocket
        public event Action<SocketGO> OnMouseEnterSocket;

        public void TriggerMouseEnterSocketEvent(SocketGO socket) => OnMouseEnterSocket?.Invoke(socket);
        #endregion

        #region Action<SocketGO> OnMouseExitSocket
        public event Action<SocketGO> OnMouseExitSocket;

        public void TriggerMouseExitSocketEvent(SocketGO socket) => OnMouseExitSocket?.Invoke(socket);
        #endregion


    }
}
