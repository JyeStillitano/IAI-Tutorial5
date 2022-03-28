using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class State
    {
        public State(int move)
        {
            Move = move;
        }
        public int Move { get; }
    }
}
