using MaciLaci4.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MaciLaci4.Model
{
    public enum GameSize { Small = 10, Medium = 15, Large = 20 }

    public class BearGameModel
    {
        #region Fields
        private int _basketsOnMap;
        private int _basketCount;
        private int _gameTime;
        private Point _bearPos;
        private GameSize _gameSize;
        private BearGameBoard _board;
        private List<KeyVal<int, Point>> _guards;
        #endregion
        #region Properties
        public int BasketCount
        {
            get { return _basketCount; }
        }

        public int GameTime
        {
            get { return _gameTime; }
        }

        public BearGameBoard Board
        {
            get { return _board; }
        }

        public Boolean IsGameWon
        {
            get { return (_basketCount == _basketsOnMap); }
        }

        public GameSize GameSize
        {
            get { return _gameSize; }
            set { _gameSize = value; }
        }

        public Point BearPosition
        {
            get { return _bearPos; }
            set { _bearPos = value; }
        }

        public List<KeyVal<int, Point>> Guards
        {
            get { return _guards; }
        }

        public BearGameEventArgs EventArgs
        {
            get => default;
            set
            {
            }
        }

        public KeyVal<object, object> KeyVal
        {
            get => default;
            set
            {
            }
        }
        #endregion

        #region Events
        public event EventHandler<BearGameEventArgs> GameAdvanced;
        public event EventHandler<BearGameEventArgs> GameOver;
        #endregion
        #region Constructor
        public BearGameModel()
        {
            _board = new BearGameBoard();
            _gameSize = GameSize.Medium;
            _gameTime = 0;
            _bearPos = new Point(0, 0);
            _guards = new List<KeyVal<int, Point>>();
        }
        #endregion
        #region Public game methods
        public void NewGame()
        {
            _board = new BearGameBoard((int)_gameSize);
            _basketCount = 0;
            _gameTime = 0;
            _bearPos = new Point(0, 0);
            _guards.Clear();
            
            switch (_gameSize)
            {
                case GameSize.Small:
                    _basketsOnMap = 3;
                    _board = LoadFields(Properties.Resources.smallMap);
                    break;
                case GameSize.Medium:
                    _basketsOnMap = 5;
                    _board = LoadFields(Properties.Resources.mediumMap);
                    break;
                case GameSize.Large:
                    _basketsOnMap = 7;
                    _board = LoadFields(Properties.Resources.largeMap);
                    break;
            }
        }

        public void MoveBear(int dir)
        {
            switch (dir)
            {
                case 1:
                    if (_bearPos.X > 0 && _board[_bearPos.X - 1, _bearPos.Y] != 1)
                        _bearPos.X--;
                    break;
                case 2:
                    if (_bearPos.Y > 0 && _board[_bearPos.X, _bearPos.Y - 1] != 1)
                        _bearPos.Y--;
                    break;
                case 3:
                    if (_bearPos.X < _board.BoardSize - 1 && (_board[_bearPos.X + 1, _bearPos.Y] != 1))
                        _bearPos.X++;
                    break;
                case 4:
                    if (_bearPos.Y < _board.BoardSize - 1 && (_board[_bearPos.X, _bearPos.Y + 1] != 1))
                        _bearPos.Y++;
                    break;
            }
            if (_board[_bearPos.X, _bearPos.Y] == 3)
            {
                _basketCount++;
                _board[_bearPos.X, _bearPos.Y] = 0;
            }
            OnGameAdvanced();
        }

        public void MoveGuards()
        {
            foreach (var guard in _guards)
            {
                if (guard.Id > _board.BoardSize)
                {
                    switch (guard.Id % 2)
                    {
                        case 0:
                            if (guard.Position.X < _board.BoardSize - 2 && _board[guard.Position.X + 2, guard.Position.Y] != 1)
                                guard.Position = new Point(guard.Position.X + 1, guard.Position.Y);
                            else
                            {
                                guard.Position = new Point(guard.Position.X + 1, guard.Position.Y);
                                guard.Id = guard.Id - 50;
                            }
                            break;
                        case 1:
                            if (guard.Position.Y < _board.BoardSize - 2 && _board[guard.Position.X, guard.Position.Y + 2] != 1)
                                guard.Position = new Point(guard.Position.X, guard.Position.Y + 1);
                            else
                            {
                                guard.Position = new Point(guard.Position.X, guard.Position.Y + 1);
                                guard.Id = guard.Id - 50;
                            }
                            break;
                    }
                }
                else
                {
                    switch (guard.Id % 2)
                    {
                        case 0:
                            if (guard.Position.X > 1 && _board[guard.Position.X - 2, guard.Position.Y] != 1)
                                guard.Position = new Point(guard.Position.X - 1, guard.Position.Y);
                            else
                            {
                                guard.Position = new Point(guard.Position.X - 1, guard.Position.Y);
                                guard.Id = guard.Id + 50;
                            }
                            break;
                        case 1:
                            if (guard.Position.Y > 1 && _board[guard.Position.X, guard.Position.Y - 2] != 1)
                                guard.Position = new Point(guard.Position.X, guard.Position.Y - 1);
                            else
                            {
                                guard.Position = new Point(guard.Position.X, guard.Position.Y - 1);
                                guard.Id = guard.Id + 50;
                            }
                            break;
                    }
                }

            }
            OnGameAdvanced();
        }


        public BearGameBoard LoadFields(String map)
        {
            string[] tmp = map.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            int gameSize = int.Parse(tmp[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
            int basketsOnMap = int.Parse(tmp[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
            BearGameBoard board = new BearGameBoard(gameSize);
            for (int i = 1; i <= gameSize; i++)
            {
                string[] numbers = tmp[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < gameSize; j++)
                {
                    board[i - 1, j] = int.Parse(numbers[j]);
                }
            }
            for (int i = 0; i < _board.BoardSize; i++)
            {
                for (int j = 0; j < _board.BoardSize; j++)
                {
                    if (board[i, j] == 2)
                    {
                        _guards.Add(new KeyVal<int, Point>(i, new Point(i, j)));
                    }
                }
            }
            return board;
        }
        #endregion
        #region Private game methods
        private void CheckGame()
        {
            if (IsGameWon)
                OnGameOver(true);
            else
            {
                foreach (var guard in _guards)
                {
                    if (_bearPos.X != _board.BoardSize - 1 && _bearPos.Y != _board.BoardSize - 1)
                    {
                        if (_bearPos.Y + 1 == guard.Position.Y && guard.Position.X == _bearPos.X + 1)
                            OnGameOver(false);
                    }
                    if (_bearPos.X != 0 && _bearPos.Y != 0)
                    {
                        if (_bearPos.Y - 1 == guard.Position.Y && guard.Position.X == _bearPos.X - 1)
                            OnGameOver(false);
                    }
                    if (_bearPos.X != _board.BoardSize - 1 && _bearPos.Y != 0)
                    {
                        if (_bearPos.Y - 1 == guard.Position.Y && guard.Position.X == _bearPos.X + 1)
                            OnGameOver(false);
                    }
                    if (_bearPos.X != 0 && _bearPos.Y != _board.BoardSize - 1)
                    {
                        if (_bearPos.Y + 1 == guard.Position.Y && guard.Position.X == _bearPos.X - 1)
                            OnGameOver(false);
                    }
                    if (_bearPos.X != _board.BoardSize - 1)
                    {
                        if (_bearPos.Y == guard.Position.Y && guard.Position.X + 1 == _bearPos.X)
                            OnGameOver(false);
                    }
                    if (_bearPos.X != 0)
                    {
                        if (_bearPos.Y == guard.Position.Y && guard.Position.X - 1 == _bearPos.X)
                            OnGameOver(false);
                    }
                    if (_bearPos.Y != _board.BoardSize - 1)
                    {
                        if (_bearPos.X == guard.Position.X && guard.Position.Y + 1 == _bearPos.Y)
                            OnGameOver(false);
                    }
                    if (_bearPos.Y != 0)
                    {
                        if (_bearPos.X == guard.Position.X && guard.Position.Y - 1 == _bearPos.Y)
                            OnGameOver(false);
                    }
                }
            }
        }
        #endregion
        #region Private event methods
        private void OnGameAdvanced()
        {
            if (GameAdvanced != null)
            {
                GameAdvanced(this, new BearGameEventArgs(false, _basketCount, _gameTime));
                CheckGame();
            }

        }

        private void OnGameOver(Boolean isWon)
        {
            if (GameOver != null)
                GameOver(this, new BearGameEventArgs(isWon, _basketCount, _gameTime));
        }
        #endregion
    }
}
