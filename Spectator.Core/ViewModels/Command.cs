using GalaSoft.MvvmLight.Command;
using System;

namespace Spectator.Core.ViewModels
{
    class Command : RelayCommand
    {
        readonly Action action;

        public Command(Action action) : base(action)
        {
            this.action = action;
        }

        public override void Execute(object parameter)
        {
            action();
        }
    }
}