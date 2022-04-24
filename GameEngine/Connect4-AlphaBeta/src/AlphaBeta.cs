using Connect4_Engine.src;

namespace Connect4_AlphaBeta
{
    public class AlphaBeta
    {

        private readonly int DepthDifficulty;

        private readonly TokenType AIPlayer;


        public AlphaBeta(TokenType AIPlayer, int DepthDifficulty)
        {
            this.AIPlayer = AIPlayer;
            this.DepthDifficulty = DepthDifficulty;
        }

        public int AlphaBetaNextMove(Board GameBoard, TokenType Player, int Depth=0)
        {
            int NextMove;
            Board NextMoveBoard;
            TokenType NextPlayer;

            //Depth limit
            if (Depth == DepthDifficulty)
                return 0; //Eval Board

            //Draw
            if (GameBoard.AvailableMoves().Count == 0)
                return 0;

            //Win
            if (GameBoard.CheckPlayerWin(Player))
                return Player == AIPlayer ? int.MaxValue : int.MinValue;

            // Set Minimum Value for the moves (To get the max)
            NextMove = int.MinValue;
            // Set the next player for the next layer
            NextPlayer = (int)AIPlayer == 0 ? TokenType.Player2 : TokenType.Player1;

            // Next moves
            for (int Col = 1; Col <= Board.Columns; Col++)
            {
                NextMoveBoard = GameBoard.DeepCopy();
                NextMoveBoard.InsertToken(Player, Col);
                NextMove = Math.Max(NextMove, this.AlphaBetaNextMove(NextMoveBoard, NextPlayer, Depth++));
            }

            return NextMove;
        
        
        }
    }
}