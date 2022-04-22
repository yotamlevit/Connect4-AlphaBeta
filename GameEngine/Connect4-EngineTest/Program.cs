using System;

using System.Threading.Tasks;

using Connect4_Engine.src;

namespace Connect4_EngineTest
{
    class Program
    {

       

        static void Main(string[] args)
        {

            Game game = new Game();
            game.Start();
            Console.ReadLine();


            //Board board = new Board();
            //Console.WriteLine(board.AvailableMoves().Count);

            /*
            Board board = new Board();

            board.InsertToken(TokenType.Player1, 1);
            Console.WriteLine(board.ToString());
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            board.InsertToken(TokenType.Player1, 1);
            Console.WriteLine(board.ToString());
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            board.InsertToken(TokenType.Player1, 1);
            Console.WriteLine(board.ToString());
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            board.InsertToken(TokenType.Player1, 1);
            Console.WriteLine(board.ToString());
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            board.InsertToken(TokenType.Player2, 1);
            Console.WriteLine(board.ToString());
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            board.InsertToken(TokenType.Player1, 3);
            Console.WriteLine(board.ToString());
            */

        }
    }
}
