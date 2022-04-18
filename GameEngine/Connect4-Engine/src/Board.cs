using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4_Engine.src
{
    /// <summary>
    /// This function represent a connect 4 game board.
    /// </summary>
    class Board
    {

        //The number of columns on the board
        public static readonly int Columns = 6;

        //An array of bitboard for connect4 - Each bitboard in a long (8 bytes). There are two boards, one for player1 and another for player2.
        //The actual Board contains 42 bits (7*6). So we have left with 22 bits (because long is 8 bytes * 8 bits = 64 - 42 = 22).
        //We use the remaining 22 bits to create a top row that will act as "full flag" and borders.
        private long[] BitBoard;

        //An array to indicate where each Column start on the board
        private int[] ColumnsPosition;

        //The location of the top "row" (7`th row) of bits on the board. The row`s role is to indicate if the column is full or not.
        //If the bit on a column`s top row is 1 the column if full.
        private static readonly long TopRow = 0b1000000_1000000_1000000_1000000_1000000_1000000_1000000L;

        //The directions that make 4 tokens "connect"
        //1 - vertical ; 7 - horizontal ; 6 - diagonal \ ; 8 - diagonal /
        private static readonly int[] WinDirections = { 1, 7, 6, 8};

        /// <summary>
        /// 
        /// Initialize the Board.
        /// Sets the defult ColumnsPosition and Create 2 bitboards for each player
        /// 
        /// </summary>
        public Board()
        {
            this.BitBoard = new long[2];
            this.ColumnsPosition = new int[] {0, 7, 15, 24, 30, 35, 42};
        }

        /// <summary>
        /// 
        /// This function validates that a new token can be inserted to the column
        /// 
        /// </summary>
        /// 
        /// <param name="InsertionColumneIndex"> (int) The index of the column that the player want to insert a token in to</param>
        /// 
        /// <returns> (bool) True if the column if free for insertion false if not</returns>
        private bool ValidateTokenInsertion(int InsertionColumneIndex)
        {
            return (TopRow & (1L << this.ColumnsPosition[InsertionColumneIndex])) == 0;
        }

        /// <summary>
        /// 
        /// This function inserts a new token into the board.
        /// 
        /// </summary>
        /// 
        /// <param name="PlayerToken"> (TokenType) The player`s token type to insert -
        ///                           This parameter actually effects on the board that the token will be inserted.
        ///                           Players1 bitboard or players2
        /// </param>
        /// 
        /// <param name="InsertionColumne"> (int) The column that the player wish to insert his token to</param>
        /// 
        /// <returns> (bool) True if the insertion succeed. Else false</returns>
        public bool InsertToken(TokenType PlayerToken, int InsertionColumne)
        {
            if (!ValidateTokenInsertion(InsertionColumne - 1))
                return false;

            long InsertionBit = 1L << this.ColumnsPosition[InsertionColumne - 1]++;
            this.BitBoard[(int)PlayerToken] ^= InsertionBit;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlayerToken"> (TokenType) The player to check hit </param>
        /// 
        /// <returns>(bool) True if win, else false</returns>
        public bool CheckPlayerWin(TokenType PlayerToken)
        {
            long CheckBoard;

            foreach (int Direction in WinDirections)
            {
                CheckBoard = this.BitBoard[(int)PlayerToken] & (this.BitBoard[(int)PlayerToken] >> Direction);

                if ((CheckBoard & (CheckBoard >> (2 * Direction)) & (CheckBoard >> (3 * Direction))) != 0)
                    return true;    
            }

            return false;
        }

        /// <summary>
        /// 
        /// This function checks for all the available moves in the current state of the board
        /// 
        /// </summary>
        /// 
        /// <returns> ( List<int> ) A list of all the available moves in the current state of the board</returns>
        public List<int> AvailableMoves()
        {
            List<int> moves = new List<int>();
            for (int col = 0; col <= Columns; col++)
            {
                if (ValidateTokenInsertion(col))
                    moves.Add(col);
            }

            return moves;
        }
    }
}
