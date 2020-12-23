using System;
using System.Windows.Input;

namespace DeliveriesDemo.Core.Commands
{
	/// <summary>
	/// Custom <see cref="ICommand"/> implementation.
	/// </summary>
	public interface IRelayCommand : ICommand
	{
		/// <summary>
		/// Invokes the <see cref="ICommand.CanExecuteChanged"/> <see cref="EventHandler"/>.
		/// </summary>
		/// <param name="sender">Sender, optional.</param>
		/// <param name="e">Event data, optional.</param>
		void InvokeCanExecuteChanged(object sender = null, EventArgs e = null);
	}
}
