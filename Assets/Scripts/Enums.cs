namespace Kulami
{
    public enum SocketStatus
    {
        Empty,
        OwnedByPlayerOne,
        OwnedByPlayerTwo,
        OwnedByPlayerOneHoverPlayerOne,
        OwnedByPlayerOneHoverPlayerTwo,
        OwnedByPlayerTwoHoverPlayerOne,
        OwnedByPlayerTwoHoverPlayerTwo,
        PossibleMoveByPlayerOne,
        PossibleMoveByPlayerTwo,
        EmptyHoverPlayerOne,
        EmptyHoverPlayerTwo
    }

    public enum TileType
    {
        SixTile,
        FourTile,
        ThreeTile,
        TwoTile
    }

    public enum Player
    {
        One,
        Two
    }
}
