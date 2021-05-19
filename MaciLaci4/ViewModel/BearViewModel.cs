using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MaciLaci4.Model;

namespace MaciLaci4.ViewModel
{
    class BearViewModel : ViewModelBase
    {
        private int _width;
        private int _height;

        private int _size;
        private ObservableCollection<BearField> _bearFields;
        private BearGameModel _model;
        public DispatcherTimer _timer;
        private int _gameTime;
        private int _basketCount;

        private DelegateCommand _sizeChangeCommand;
        private DelegateCommand _newGameCommand;
        private DelegateCommand _exitCommand;
        private DelegateCommand _pauseCommand;

        public event EventHandler ExitGame;
        public event EventHandler PauseGame;

        public BearViewModel(BearGameModel m)
        {
            Width = 600;
            Height = 600;
            GameTime = 0;
            BasketCount = 0;
            _model = m;
            BearFields = new ObservableCollection<BearField>();

            _sizeChangeCommand = new DelegateCommand(p => _model.GameSize = (GameSize)Convert.ToInt32(p));
            _newGameCommand = new DelegateCommand(p => { _model.NewGame(); SetupBoard(); });
            _exitCommand = new DelegateCommand(param => OnExitGame());
            _pauseCommand = new DelegateCommand(param => OnPauseGame());
            _model.GameAdvanced += _model_GameAdvanced;
            _model.GameOver += _model_GameOver;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += _timer_Tick;

            _model.NewGame();
            SetupBoard();

            _timer.Start();
        }

        #region Properties

        public int Width { get => _width; set { _width = value; OnPropertyChanged(); } }
        public int Height { get => _height; set { _height = value; OnPropertyChanged(); } }
        public int Size { get => _size; set { _size = value; OnPropertyChanged(); } }
        public ObservableCollection<BearField> BearFields { get => _bearFields; set => _bearFields = value; }
        public DelegateCommand SizeChangeCommand { get => _sizeChangeCommand; set => _sizeChangeCommand = value; }
        public DelegateCommand NewGameCommand { get => _newGameCommand; set => _newGameCommand = value; }
        public DelegateCommand ExitCommand { get => _exitCommand; set => _exitCommand = value; }
        public DelegateCommand PauseCommand { get => _pauseCommand; set => _pauseCommand = value; }
        public int GameTime { get => _gameTime; set { _gameTime = value; OnPropertyChanged(); } }
        public int BasketCount { get => _basketCount; set { _basketCount = value; OnPropertyChanged(); } }
        #endregion

        private void SetupBoard()
        {
            GameTime = 0;
            _timer.Start();
            Size = (int)_model.GameSize;
            Width = Size * 40;
            Height = Size * 40 + 50;
            BearFields.Clear();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int t;
                    if (_model.Board[i,j] == 0 || _model.Board[i, j] == 2 || _model.Board[i, j] == 4)
                    {
                        t = 0;
                        BearFields.Add(new BearField(t));
                    }
                    else
                    {
                        BearField bf = new BearField(_model.Board[i, j]);
                        BearFields.Add(bf);
                    }
                }
            }

            BearFields[_model.BearPosition.X * Size + _model.BearPosition.Y].Type = 4;
            foreach (var guard in _model.Guards)
            {
                BearFields[guard.Position.X * Size + guard.Position.Y].Type = 2;
            }
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int t;
                    if (_model.Board[i, j] == 0 || _model.Board[i, j] == 2 || _model.Board[i, j] == 4)
                    {
                        t = 0;
                        BearFields[i*Size+j].Type = t;
                    }
                    else
                    {
                        BearFields[i * Size + j].Type = _model.Board[i, j];
                    }
                }
            }
            BearFields[_model.BearPosition.X * Size + _model.BearPosition.Y].Type = 4;
            foreach (var guard in _model.Guards)
            {
                BearFields[guard.Position.X * Size + guard.Position.Y].Type = 2;
            }
            BasketCount = _model.BasketCount;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            GameTime++;
            _model.MoveGuards();
        }

        private void _model_GameAdvanced(object sender, BearGameEventArgs e)
        {
            UpdateBoard();
        }

        private void _model_GameOver(object sender, BearGameEventArgs e)
        {
            _timer.Stop();

            if (e.IsWon)
            {
                MessageBox.Show("Gratulálok, győztél!" + Environment.NewLine +
                                "Összesen " + GameTime.ToString() + " másodpercig játszottál.",
                                "Maci Laci játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Sajnálom, vesztettél, a vadőrök megláttak!",
                                "Maci Laci játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
        }

        public void OnKeyPressed(KeyEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                switch (e.Key)
                {
                    case Key.W:
                        _model.MoveBear(1);
                        break;
                    case Key.A:
                        _model.MoveBear(2);
                        break;
                    case Key.S:
                        _model.MoveBear(3);
                        break;
                    case Key.D:
                        _model.MoveBear(4);
                        break;
                }
            }
        }

        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        private void OnPauseGame()
        {
            if (PauseGame != null)
                PauseGame(this, EventArgs.Empty);
        }
    }
}