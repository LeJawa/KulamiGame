namespace Kulami.Data
{
    public struct GameStateInfo
    {
        public GameState State { get; }
        public GameState PreviousState { get; }
        public int Round { get; }
        public int Player1Score { get; }
        public int Player2Score { get; }
        public Player? Winner { get; }

        public GameStateInfo(GameState state, GameState previousState, int round, int player1Score, int player2Score, Player? winner)
        {
            State = state;
            PreviousState = previousState;
            Round = round;
            Player1Score = player1Score;
            Player2Score = player2Score;
            Winner = winner;
        }
    }
}
