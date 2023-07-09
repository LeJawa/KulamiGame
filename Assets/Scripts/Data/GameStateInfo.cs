using Kulami;

namespace Kulami.Data
{
    public struct GameStateInfo
    {
        public GameState State { get; }
        public int Round { get; }
        public int Player1Score { get; }
        public int Player2Score { get; }
        public Player? Winner { get; }

        public GameStateInfo(GameState state, int round, int player1Score, int player2Score, Player? winner)
        {
            State = state;
            Round = round;
            Player1Score = player1Score;
            Player2Score = player2Score;
            Winner = winner;
        }
    }
}
