using Connect4_Engine.src;

namespace Connect4_AlphaBeta
{
    public class AlphaBeta
    {

        private readonly int DepthDifficulty;

        private readonly TokenType AIPlayer;
        private readonly TokenType OpponentPlayer;


        public AlphaBeta(TokenType AIPlayer, int DepthDifficulty)
        {
            this.AIPlayer = AIPlayer;
            this.OpponentPlayer = (int)AIPlayer == 0 ? TokenType.Player2 : TokenType.Player1;
            this.DepthDifficulty = DepthDifficulty;
        }


        private int EvaluationFunction(Board GameBoard)
        {
            //Win AI
            if (GameBoard.CheckPlayerWin(this.AIPlayer, 4))
                return int.MaxValue;

            //Win Opponent
            if (GameBoard.CheckPlayerWin(this.OpponentPlayer, 4))
                return int.MinValue;

            //Draw
            if (GameBoard.AvailableMoves().Count == 0)
                return 0;

            return 1;
        }


        public int FindBestMove(Board GameBoard)
        {
            int bestMove = 0, bestScore = int.MinValue;
            Board nextMoveBoard;

            for (int col = 1; col <= Board.Columns; col++)
            {
                nextMoveBoard = GameBoard.DeepCopy();
                nextMoveBoard.InsertToken(this.AIPlayer, col);
                if (this.AlphaBetaNextMove(nextMoveBoard, this.OpponentPlayer, 5) > bestScore)
                {
                    bestMove = col;
                    bestScore = this.AlphaBetaNextMove(GameBoard, this.AIPlayer, 5);
                }
            }

            return bestMove;
        }

        private int AlphaBetaNextMove(Board GameBoard, TokenType player, int Depth=0)
        {
            int bestScore, boardScore;
            Board nextMoveBoard;
            TokenType nextPlayer;

            // //Depth limit
            // if (Depth == DepthDifficulty)
            //     return 0; //Eval Board

            boardScore = this.EvaluationFunction(GameBoard);

            if (boardScore == int.MinValue || boardScore == int.MaxValue || boardScore == 0)
                return boardScore;

            // Set Minimum Value for the moves (To get the max)
            bestScore = int.MinValue;
            // Set the next player for the next layer
            nextPlayer = player == this.AIPlayer ? this.OpponentPlayer : this.AIPlayer;

            // Next moves
            for (int Col = 1; Col <= Board.Columns; Col++)
            {
                nextMoveBoard = GameBoard.DeepCopy();
                nextMoveBoard.InsertToken(player, Col);
                bestScore = Math.Max(bestScore, this.AlphaBetaNextMove(nextMoveBoard, nextPlayer, Depth++));
            }

            return bestScore;
        
        
        }
    }
}