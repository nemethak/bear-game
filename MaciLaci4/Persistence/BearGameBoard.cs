using System;
using System.Collections.Generic;
using System.Text;

namespace MaciLaci4.Persistence
{
    public class BearGameBoard
    {
        private int _boardSize;
        private int[,] _fieldValues;

        public int BoardSize
        {
            get { return _boardSize; }
            set { _boardSize = value; }
        }

        public int this[int x, int y]
        {
            get { return _fieldValues[x, y]; }
            set { _fieldValues[x, y] = value; }
        }

        public BearGameBoard() : this(15) { }

        public BearGameBoard(int boardSize)
        {
            if (boardSize < 0)
                throw new ArgumentOutOfRangeException("The board size is less than 0.", "boardSize");
            _boardSize = boardSize;
            _fieldValues = new int[boardSize, boardSize];
        }
    }
}
