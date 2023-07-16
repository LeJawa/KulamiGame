using Kulami.Game;

namespace Kulami.Data
{
    public struct BoardGenerationInfo
    {
        public Tile[] Tiles { get; }
        public Socket[] Sockets { get; }

        public BoardGenerationInfo(Tile[] tiles, Socket[] sockets)
        {
            Tiles = tiles;
            Sockets = sockets;
        }
    }
}
