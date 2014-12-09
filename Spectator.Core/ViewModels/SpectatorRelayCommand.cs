using GalaSoft.MvvmLight.Command;
using System;

namespace Spectator.Core.ViewModels
{
    class SpectatorRelayCommand : RelayCommand
    {
        Action action;

        public SpectatorRelayCommand(Action action) : base(action)
        {
            this.action = action;
        }

        public override void Execute(object parameter)
        {
            action();
        }
    }
}