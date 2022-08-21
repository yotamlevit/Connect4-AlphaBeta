using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4_Engine.src
{
    /// <summary>
    /// This function represent a connect 4 game board.
    /// </summary>
    public class Board
    {

        //The number of columns on the board
        public static readonly int Columns = 7;

        //The number of rows on the board
        public static readonly int Rows = 6;

        //An array of bitboard for connect4 - Each bitboard in a long (8 bytes). There are two boards, one for player1 and another for player2.
        //The actual Board contains 42 bits (7*6). So we have left with 22 bits (because long is 8 bytes * 8 bits = 64 - 42 = 22).
        //We use the remaining 22 bits to create a top row that will act as "full flag" and borders.
        private long[] BitBoard;

        //An array to indicate where each Column start on the board
        private int[] ColumnsPosition;

        //The location of the top "row" (7`th row) of bits on the board. The row`s role is to indicate if the column is full or not.
        //If the bit on a column`s top row is 1 the column if full.
        private static readonly long TopRow = 0b1000000_1000000_1000000_1000000_1000000_1000000_1000000L;

        private static readonly byte RowMask = 0b0111111;

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
            this.ColumnsPosition = new int[] {0, 7, 14, 21, 28, 35, 42};
        }

        public Board(long[] BitBoard, int[] ColumnsPosition)
        {
            this.BitBoard = BitBoard;
            this.ColumnsPosition = ColumnsPosition;
        }

        /// <summary>
        /// 
        /// This function returns the bit board array
        /// 
        /// </summary>
        /// 
        /// <returns> (long[]) The array of the bitb boards</returns>
        public long[] GetBitBoard()
        {
            return this.BitBoard;
        }

        /// <summary>
        /// 
        /// This function makes a DeepCopy of the Board object
        /// 
        /// </summary>
        /// 
        /// <returns> (Board) A DeepCopy of the current object </returns>
        public Board DeepCopy()
        {
            long[] bitBoardCopy = new long[this.BitBoard.Length];
            int[] columnsPositionCopy = new int[this.ColumnsPosition.Length];

            this.BitBoard.CopyTo(bitBoardCopy, 0);
            this.ColumnsPosition.CopyTo(columnsPositionCopy, 0);

            return new Board(bitBoardCopy, columnsPositionCopy);
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
            return 0 <= InsertionColumneIndex && InsertionColumneIndex <= 6 && (TopRow & (1L << this.ColumnsPosition[InsertionColumneIndex])) == 0;
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
        /// <returns> (bool) True if the insertion succeeded. Else false</returns>
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
        /// This function removes a tocken from the board
        /// 
        /// </summary>
        /// 
        /// <param name="PlayerToken"> (TokenType) The player`s token type to remove -
        ///                           This parameter actually effects on the board that the token will be inserted.
        ///                           Players1 bitboard or players2
        /// </param>
        /// <param name="InsertionColumne"> (int) The column that the player wish to insert his token to </param>
        /// <returns> (bool) True if the removal succeeded. Else false </returns>
        public bool RemoveToken(TokenType PlayerToken, int InsertionColumne)
        {
            long InsertionBit = 1L << --this.ColumnsPosition[InsertionColumne - 1];
            this.BitBoard[(int)PlayerToken] ^= InsertionBit;

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlayerToken"> (TokenType) The player to check hit </param>
        /// <param name="StreakCount"> (int) The number of tokens to connect </param>
        /// 
        /// <returns>(bool) True if win, else false</returns>
        public bool CheckPlayerWin(TokenType PlayerToken)
        {
            return CheckPlayerWin(PlayerToken, 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlayerToken"> (TokenType) The player to check hit </param>
        /// <param name="StreakCount"> (int) The number of tokens to connect </param>
        /// 
        /// <returns>(bool) True if win, else false</returns>
        public bool CheckPlayerWin(TokenType PlayerToken, int StreakCount)
        {
            long CheckBoard;

            foreach (int Direction in WinDirections)
            {
                CheckBoard = this.BitBoard[(int)PlayerToken] & (this.BitBoard[(int)PlayerToken] >> Direction);

                if ((CheckBoard & (CheckBoard >> ((StreakCount -2) * Direction))) != 0)
                    return true;    
            }

            return false;
        }

        public bool IsWinningMove(TokenType PlayerToken, int InsertionColumne)
        {
            bool isWin;

            this.InsertToken(PlayerToken, InsertionColumne);
            isWin = this.CheckPlayerWin(PlayerToken);
            this.RemoveToken(PlayerToken, InsertionColumne);

            return isWin;
        }


        public bool CanWinNextMove(TokenType PlayerToken)
        {
            for (int move = 1; move <= Columns; move++)
                if (this.ValidateTokenInsertion(move) && IsWinningMove(PlayerToken, move))
                    return true;

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
            for (int col = 0; col < Columns; col++)
            {
                if (ValidateTokenInsertion(col))
                    moves.Add(col);
            }

            return moves;
        }

        /// <summary>
        /// 
        /// This function merage both players bit boards into one string with thair tokens.
        /// 
        /// </summary>
        /// 
        /// <returns> (String) a meraged board of both players bitboard </returns>
        private String MergePlayerBoards()
        {
            StringBuilder sb = new StringBuilder(new String('-', 49));

            char[] tmp;
            String PlayerBitBoard;
            int TokenIndex;
            char TokenChar;
            for (int Player = 0; Player < 2; Player++)
            {
                tmp = Convert.ToString(this.BitBoard[Player], 2).ToCharArray();
                Array.Reverse(tmp);
                PlayerBitBoard = new String(tmp);

                TokenIndex = PlayerBitBoard.IndexOf('1');
                TokenChar = Player == 0 ? 'X' : 'O';

                while (TokenIndex != -1)
                {
                    sb[TokenIndex] = TokenChar;
                    TokenIndex = PlayerBitBoard.IndexOf('1', TokenIndex + 1);

                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// This is a ToString function to print the board
        /// </summary>
        /// <returns> (string) A string that represent the board</returns>
        public override string ToString()
        {
            String MergedBitBoard;
            String ParsedBitBoard;
            StringBuilder sb = new StringBuilder();

            MergedBitBoard = this.MergePlayerBoards();

            sb.AppendLine("+---------------------+");
            
            for (int Row = 5; Row >= 0; Row--)
            {
                sb.Append("|");

                for (int Col = 0; Col < 7; Col++)
                {
                    sb.Append(" " + MergedBitBoard[Row + (7 * Col)] + " ");
                }

                sb.Append("|").AppendLine();
            }

            sb.AppendLine("+---------------------+");
            sb.AppendLine("  1  2  3  4  5  6  7");

            ParsedBitBoard = sb.ToString();

            return ParsedBitBoard;
        }
    }
}
