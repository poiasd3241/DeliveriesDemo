using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeliveriesDemo.Core.ViewModels.Base
{
	/// <summary>
	/// Simple <see cref="INotifyPropertyChanged"/> implementation.
	/// </summary>
	public class PropertyChangedNotifyBase : INotifyPropertyChanged
	{
		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Protected Methods

		/// <summary>
		/// Invokes <see cref="PropertyChanged"/> for the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of property.</param>
		protected void InvokePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
