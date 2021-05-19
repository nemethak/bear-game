using System;
using System.Collections.Generic;
using System.Text;

namespace MaciLaci4.Model
{
    public class BearGameEventArgs : EventArgs
    {
        private int _gameTime;
        private int _basketCount;
        private Boolean _isWon;
        public int GameTime { get { return _gameTime; } }
        public int BasketCount { get { return _basketCount; } }
        public Boolean IsWon { get { return _isWon; } }
        public BearGameEventArgs(Boolean isWon, int basketCount, int gameTime)
        {
            _isWon = isWon;
            _basketCount = basketCount;
            _gameTime = gameTime;
        }
    }
}
