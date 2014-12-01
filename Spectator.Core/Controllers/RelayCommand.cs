using System;
using System.Windows.Input;

namespace Spectator.Core.Controllers
{
	public class RelayCommand : ICommand
	{
		Action command;

		public RelayCommand (Action command)
		{
			this.command = command;
		}

		#region ICommand implementation

		public event EventHandler CanExecuteChanged;

		public bool CanExecute (object parameter)
		{
			return true;
		}

		public void Execute (object parameter)
		{
			command ();
		}

		#endregion
	}
}