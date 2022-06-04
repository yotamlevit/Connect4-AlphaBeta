﻿using System;
using System.Collections.Generic;
using System.Text;


namespace Connect4_Engine.src.AI
{
    public class AlphaBeta
    {

        private readonly int DepthDifficulty;

        private readonly TokenType AIPlayer;
        private readonly TokenType OpponentPlayer;

        private int count;


        public AlphaBeta(TokenType AIPlayer, int DepthDifficulty)
        {
            this.AIPlayer = AIPlayer;
            this.OpponentPlayer = (int)AIPlayer == 0 ? TokenType.Player2 : TokenType.Player1;
            this.DepthDifficulty = DepthDifficulty;
        }

        private int DepthEvaluationFunction(Board GameBoard)
        {
            return ((20 * Convert.ToInt32(GameBoard.CheckPlayerWin(this.AIPlayer, 3))) +
                (10 * Convert.ToInt32(GameBoard.CheckPlayerWin(this.AIPlayer, 2)))) -
                (20 * Convert.ToInt32(GameBoard.CheckPlayerWin(this.OpponentPlayer, 3))) -
                (10 * Convert.ToInt32(GameBoard.CheckPlayerWin(this.OpponentPlayer, 2)));
        }

        /*
        private int EvaluationFunction(Board GameBoard, TokenType player)
        {
            //Win AI
            if (GameBoard.CheckPlayerWin(this.AIPlayer, 4))
                return 2000000000; //int.MaxValue;

            //Win Opponent
            if (GameBoard.CheckPlayerWin(this.OpponentPlayer, 4))
                return -2000000000;//int.MinValue;

            int boardScore = DepthEvaluationFunction(GameBoard);

            return player == this.AIPlayer ? boardScore : -boardScore;
        }
        */

        private int EvaluationFunction(Board GameBoard, TokenType player, int moveCount)
        {
            //Win AI
            if (GameBoard.CheckPlayerWin(this.AIPlayer, 4))
                return Board.Columns * Board.Rows +1 - moveCount / 2;

            //Win Opponent
            if (GameBoard.CheckPlayerWin(this.OpponentPlayer, 4))
                return -(Board.Columns * Board.Rows + 1 - moveCount / 2);


            return int.MinValue;
        }

        public int FindBestMove(Board GameBoard, int moveCount)
        {
            int bestMove = 0, bestScore = int.MinValue, alphaBetaScore;
            bool moveResult;
            Board nextMoveBoard;

            this.count = 0;

            for (int col = 1; col <= Board.Columns; col++)
            {
                nextMoveBoard = GameBoard.DeepCopy();
                moveResult = nextMoveBoard.InsertToken(this.AIPlayer, col);
                if (moveResult)
                {
                    alphaBetaScore = this.NegaMaxNextMove(nextMoveBoard, this.AIPlayer, moveCount);
                    Console.WriteLine(GameBoard.ToString());
                    Console.WriteLine(col + " - " + " Score: " + alphaBetaScore);
                    if (alphaBetaScore > bestScore)
                    {
                        bestMove = col;
                        bestScore = alphaBetaScore;
                    }
                    else if (alphaBetaScore == bestScore)
                    {
                        bestMove = Math.Abs(4 - bestMove) < Math.Abs(4 - col) ? bestMove : col;
                    }
                    nextMoveBoard.RemoveToken(this.AIPlayer, col);
                }
            }
            Console.WriteLine("BestMove: " + bestMove);
            Console.WriteLine("BestCrore: " + bestScore);
            return bestMove;
        }

        public int AlphaBetaNextMove(Board GameBoard, TokenType player, int moveCount, int alpha, int beta)
        {
            int bestScore, score, nextMoveCount;
            Board nextMoveBoard;
            bool moveResult;
            TokenType nextPlayer;

            this.count++;

            // Draw
            if (GameBoard.AvailableMoves().Count == 0)
            {
                //Console.WriteLine(GameBoard.ToString());
                //Console.WriteLine(this.count + " - " + moveCount + " - Score: 0");
                return 0;
            }

            if (GameBoard.CheckPlayerWin(player, 4))
            {
                //Console.WriteLine(GameBoard.ToString());
                //Console.WriteLine(this.count + " - " + moveCount + " Score: " + (Board.Columns * Board.Rows + 1 - moveCount / 2));
                return - (Board.Columns * Board.Rows + 1 - moveCount / 2);
            }


            bestScore = (Board.Columns * Board.Rows - 1 - moveCount / 2);

            if (beta > bestScore)
            {
                beta = bestScore;
                if (alpha >= beta)
                    return beta;
            }

            // Set the next player for the next layer
            nextPlayer = player == this.AIPlayer ? this.OpponentPlayer : this.AIPlayer;

            nextMoveCount = ++moveCount;

            //Console.WriteLine(GameBoard.ToString());

            // Next moves
            for (int Col = 1; Col <= Board.Columns; Col++)
            {
                nextMoveBoard = GameBoard.DeepCopy();
                moveResult = nextMoveBoard.InsertToken(player, Col);
                if (moveResult)
                {
                    score = -this.AlphaBetaNextMove(nextMoveBoard, nextPlayer, nextMoveCount, -beta, -alpha);
                    nextMoveBoard.RemoveToken(player, Col);

                    if (score >= beta) return score;
                    if (score > alpha) alpha = score;
                    
                }

            }

            return alpha;

        }

        public int NegaMaxNextMove(Board gameBoard, TokenType player, int moveCount)
        {
            int bestScore, nextMoveCount, negamaxScore;
            bool moveResult;
            TokenType nextPlayer;

            this.count++;

            // Draw
            if (gameBoard.AvailableMoves().Count == 0)
                return 0;

            // Win
            if (gameBoard.CheckPlayerWin(player))
                return Board.Columns * Board.Rows + 1 - moveCount / 2;

            // Init the best possible score with a lower bound of score.
            bestScore = -(Board.Columns * Board.Rows);
            // Set the next player for the next layer
            nextPlayer = player == this.AIPlayer ? this.OpponentPlayer : this.AIPlayer;
            nextMoveCount = ++moveCount;

            // Next moves
            for (int Col = 1; Col <= Board.Columns; Col++)
            {
                moveResult = gameBoard.InsertToken(nextPlayer, Col);
                if (moveResult)
                {
                    negamaxScore = this.NegaMaxNextMove(gameBoard, nextPlayer, nextMoveCount);   
                    bestScore = Math.Max(bestScore, negamaxScore);
                    gameBoard.RemoveToken(nextPlayer, Col);
                }

            }

            return -bestScore;

        }
    }
}
