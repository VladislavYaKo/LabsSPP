using System;
using System.Windows.Input;

namespace Lab3SPP
{
    public class RelayCommand : ICommand
    {
        public Action<object> action;
        public Func<object, bool> _canExecute;

        public RelayCommand(Action<object> act, Func<object, bool> canExec = null)
        {
            this.action = act;
            this._canExecute = canExec;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
