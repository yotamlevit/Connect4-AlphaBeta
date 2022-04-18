using System;

using Connect4_Engine.src;

namespace Connect4_EngineTest
{
    class Program
    {

        public static TokenType Fun(TokenType a)
        {
            return a;
        }

        static void Main(string[] args)
        {
            //TokenType a = 0;
            int a = 0;

            TokenType b = Fun((TokenType)a);
            Console.WriteLine(b);
            Console.WriteLine((int)b);
            /*
            Board board = new Board(5, 6);

            board.PrintBoard();

            Console.WriteLine("----------------------------------------");

            board.InsertToken(TokenType.Player1, 2);

            board.PrintBoard();

            Console.WriteLine("----------------------------------------");

            board.InsertToken(TokenType.Player1, 2);

            board.PrintBoard();

            Console.WriteLine("----------------------------------------");

            board.InsertToken(TokenType.Player1, 2);

            board.PrintBoard();

            Console.WriteLine("----------------------------------------");

            board.InsertToken(TokenType.Player1, 2);

            board.PrintBoard();

            Console.WriteLine("----------------------------------------");

            board.InsertToken(TokenType.Player1, 2);

            board.PrintBoard();

            Console.WriteLine("----------------------------------------");

            Console.WriteLine(board.InsertToken(TokenType.Player1, 2));

            board.PrintBoard();
            */
        }
    }
}
