using System;
using System.Windows.Input;

namespace MaciLaci4.ViewModel
{

    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public DelegateCommand(Action<object> execute) : this(null, execute) { }

        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        internal BearField BearField
        {
            get => default;
            set
            {
            }
        }

        internal BearViewModel BearViewModel
        {
            get => default;
            set
            {
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);

        }
    }
}