using Kulami.Data;
using Kulami.Graphics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami
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

        #region Action<SocketGO> OnSocketClicked
        public event Action<Socket> OnSocketClicked;

        public void TriggerSocketClickedEvent(Socket socket) => OnSocketClicked?.Invoke(socket);
        #endregion

        #region Action<List<Vector2Int>> PossibleMovesBroadcast
        public event Action<List<Vector2Int>> PossibleMovesBroadcast;

        public void TriggerPossibleMovesBroadcastEvent(List<Vector2Int> possibleMoves) => PossibleMovesBroadcast?.Invoke(possibleMoves);
        #endregion

        #region Action ClearPossibleMoves
        public event Action ClearPossibleMoves;

        public void TriggerClearPossibleMovesEvent() => ClearPossibleMoves?.Invoke();
        #endregion

        #region Action DrawLastPlacedMarble
        public event Action<Player, Vector2Int> DrawLastPlacedMarble;

        public void TriggerDrawLastPlacedMarbleEvent(Player player, Vector2Int position) => DrawLastPlacedMarble?.Invoke(player, position);
        #endregion

        #region Action<BoardGenerationInfo> DrawBoard
        public event Action<BoardGenerationInfo> DrawBoard;

        public void TriggerDrawBoardEvent(BoardGenerationInfo boardGenerationInfo) => DrawBoard?.Invoke(boardGenerationInfo);
        #endregion

        #region Action BoardDrawn
        public event Action BoardDrawn;

        public void TriggerBoardDrawnEvent() => BoardDrawn?.Invoke();
        #endregion

        #region Action<GameStateInfo> StateChanged
        public event Action<GameStateInfo> StateChanged;

        public void TriggerStateChangedEvent(GameStateInfo gameStateInfo) => StateChanged?.Invoke(gameStateInfo);
        #endregion
    }
}
