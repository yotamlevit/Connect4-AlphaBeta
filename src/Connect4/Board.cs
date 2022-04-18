using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.Connect4
{
    class Board
    {
        public static readonly int Rows = 6;

        public static readonly int Colums = 6;

        private TokenType[,] grid;

        public Board()
        {
            this.grid = new TokenType[Rows, Colums];
        }

        private void InitBoard()
        {
            
        }
    }
}
