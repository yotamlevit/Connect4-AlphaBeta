using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4_Engine.src
{
    public class Game
    {

        private Board GameBoard;

        private int TurnCount;

        private int[] Moves;

        private int ConnectStreak;

        /// <summary>
        /// Init Function
        /// </summary>
        public Game(int ConnectStreak)
        {
            this.GameBoard = new Board();
            this.TurnCount = 0;
            this.Moves = new int[47];
            this.ConnectStreak = ConnectStreak;
        }

        public int GetPlayerMove()
        {
            Console.Write("Enter Column To Insert Token: ");
            return int.Parse(Console.ReadLine());
        }

        public void Start()
        {
            bool IsWin = false;
            int PlayerMove;
            TokenType CurrentPlayer = TokenType.Player1;

            while(!IsWin && this.GameBoard.AvailableMoves().Count != 0)
            {
                Console.Clear();
                CurrentPlayer = (TokenType)(this.TurnCount % 2);
                Console.WriteLine("Its " + CurrentPlayer + " Turn");

                Console.WriteLine(this.GameBoard.ToString());

                PlayerMove = GetPlayerMove();
                if (!MakeMove(PlayerMove))
                {
                    Console.WriteLine("Errorrrr");
                }

                IsWin = this.GameBoard.CheckPlayerWin(CurrentPlayer, ConnectStreak);
            }
            Console.Clear();
            Console.WriteLine(this.GameBoard.ToString());

            if (this.GameBoard.AvailableMoves().Count == 0)
            {
                Console.WriteLine("Board if FULL its a TIE");
            }
            else
            {
                Console.WriteLine(CurrentPlayer + " WIN!!!!!!!!!");
            }
        }

        public bool MakeMove(int InsertionColumne)
        {
            //Give token ype of player1 if the turn counter if even else player2
            if (!this.GameBoard.InsertToken((TokenType)(this.TurnCount % 2), InsertionColumne))
                return false;

            this.Moves[this.TurnCount++] = InsertionColumne;
            return true;
        }

        public bool UndoMove()
        {
            int Col = this.Moves[--this.TurnCount];
            if (!this.GameBoard.RemoveToken((TokenType)(this.TurnCount % 2), Col))
                return true;

            this.TurnCount++;

            return false;
        }
    }
}
