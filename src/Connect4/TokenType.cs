using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.Connect4
{
    /// <summary>
    /// This enum represent all the possibol token types that can be used in the game of connect 4.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Empty space/no token in the space on the game board.
        /// </summary>
        Empty,
        /// <summary>
        /// Player 1 token type.
        /// </summary>
        Player1,
        /// <summary>
        /// Player 2 token type.
        /// </summary>
        Player2
    }
}
