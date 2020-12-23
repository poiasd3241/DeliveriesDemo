using System;

namespace DeliveriesDemo.Core.Commands
{
	/// <summary>
	/// Custom <see cref="IRelayCommand"/> implementation.<br/>
	/// Any parameter passed in to the <see cref="CanExecute(object)"/> or <see cref="Execute(object)"/> will be ignored.
	/// </summary>
	public class RelayParameterlessCommand : IRelayCommand
	{
		#region Private Members

		/// <summary>
		/// <see cref="Action"/> to execute.
		/// </summary>
		private readonly Action _execute;

		/// <summary>
		/// <see cref="Func{TResult}"/> that defines the <see cref="CanExecute(object)"/> value.
		/// </summary>
		private readonly Func<bool> _canExecute;

		#endregion

		#region Public Events

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor to use when the <see cref="CanExecute(object)"/> should always return <see langword="true"/>.
		/// </summary>
		/// <param name="execute"><see cref="Action"/> to execute.</param>
		public RelayParameterlessCommand(Action execute) : this(execute, null)
		{
			_execute = execute;
		}

		/// <summary>
		/// Constructor to use when the <see cref="CanExecute(object)"/> needs to be defined by a <see cref="Func{TResult}"/>.
		/// </summary>
		/// <param name="execute"><see cref="Action"/> to execute.</param>
		/// <param name="canExecute"><see cref="Func{TResult}"/> that defines the <see cref="CanExecute(object)"/> value.</param>
		public RelayParameterlessCommand(Action execute, Func<bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Determines if <see cref="Execute(object)"/> can execute.
		/// </summary>
		/// <param name="parameter">This parameter will be ignored. It is here to satisfy the <see cref="System.Windows.Input.ICommand"/>.</param>
		/// <returns><see langword="true"/> if this command can be executed; otherwise, <see langword="false"/></returns>
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute();
		}

		/// <summary>
		/// Executes the command by calling the <see cref="_execute"/> <see cref="Action"/>
		/// </summary>
		/// <param name="parameter">This parameter will be ignored. It is here to satisfy the <see cref="System.Windows.Input.ICommand"/>.</param>
		public void Execute(object parameter)
		{
			_execute();
		}

		public void InvokeCanExecuteChanged(object sender = null, EventArgs e = null)
		{
			CanExecuteChanged?.Invoke(sender, e);
		}

		#endregion
	}
}
