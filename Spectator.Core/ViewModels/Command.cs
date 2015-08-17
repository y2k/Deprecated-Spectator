using System;
using GalaSoft.MvvmLight.Command;

namespace Spectator.Core.ViewModels
{
    class Command : RelayCommand
    {
        readonly Action action;

        public Command(Action action)
            : base(action)
        {
            this.action = action;
        }

        public override void Execute(object parameter)
        {
            action();
        }
    }

    class Command<T> : Command
    {
        readonly Action<T> callback;

        public event EventHandler CanExecuteChanged;

        public new bool CanExecute(object parameter)
        {
            return true;
        }

        public Command(Action<T> callback)
            : base(() => {})
        {
            this.callback = callback;
        }

        public override void Execute(object parameter)
        {
            callback((T)parameter);
        }
    }
}