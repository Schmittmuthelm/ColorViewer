using System;
using System.Windows.Input;

namespace WpfColorView
{
    public class RelayCommand : ICommand
    {
        private readonly Func<bool> canExecuteEvaluator;
        private readonly Action<object> methodToExecute;

        public RelayCommand(Action<object> methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action<object> methodToExecute) : this(methodToExecute, () => true)
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteEvaluator.Invoke();
        }

        public void Execute(object parameter)
        {
            methodToExecute?.Invoke(parameter);
        }
    }
}