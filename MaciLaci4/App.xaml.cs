using System;
using System.Windows;
using System.Windows.Input;
using MaciLaci4.Model;
using MaciLaci4.ViewModel;

namespace MaciLaci4
{
    public partial class App : Application
    {
        private MainWindow _window;
        private BearGameModel _model;
        private BearViewModel _viewModel;
        
        public App()
        {
            _window = new MainWindow();
            _model = new BearGameModel();
            _viewModel = new BearViewModel(_model);

            _window.DataContext = _viewModel;
            _window.Show();
            
            _window.KeyDown += _window_KeyDown;
            _viewModel.ExitGame += _viewModel_ExitGame;
            _viewModel.PauseGame += _viewModel_PauseGame;
        }

        private void _viewModel_PauseGame(object sender, EventArgs e)
        {
            if (_viewModel._timer.IsEnabled)
                _viewModel._timer.Stop();

            else
                _viewModel._timer.Start();
        }

        private void _viewModel_ExitGame(object sender, EventArgs e)
        {
            _window.Close();
        }

        private void _window_KeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.OnKeyPressed(e);
        }
    }
}
