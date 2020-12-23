using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DeliveriesDemo.Core.Validation
{
	public class PropertyValidator : IPropertyValidator
	{
		#region Private Members

		/// <summary>
		/// Lists of error messages by property name.
		/// </summary>
		private readonly Dictionary<string, List<string>> _errorMessagesByPropertyName = new Dictionary<string, List<string>>();

		#endregion

		#region Public Events

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		#endregion

		#region Public Properties

		public bool HasErrors => _errorMessagesByPropertyName.Any();

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets all error messages for the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of property.</param>
		/// <returns>A list of error messages. If there are no error messages for the specified property name, returns <see langword="null"/>.</returns>
		public IEnumerable GetErrors([CallerMemberName] string propertyName = null)
		{
			return _errorMessagesByPropertyName.GetValueOrDefault(propertyName, null);
		}

		public List<string> GetAllErrors()
		{
			return _errorMessagesByPropertyName.SelectMany(entry => entry.Value).ToList();
		}

		public void AddError(string errorMessage, [CallerMemberName] string propertyName = null)
		{
			if (_errorMessagesByPropertyName.ContainsKey(propertyName) == false)
			{
				_errorMessagesByPropertyName.Add(propertyName, new List<string>());
			}

			_errorMessagesByPropertyName[propertyName].Add(errorMessage);

			OnErrorsChanged();
		}

		public void ClearErrors([CallerMemberName] string propertyName = null)
		{
			if (_errorMessagesByPropertyName.Remove(propertyName))
			{
				OnErrorsChanged();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Invokes the <see cref="ErrorsChanged"/> <see cref="EventHandler"/>.
		/// </summary>
		/// <param name="propertyName">Name of property that is being validated.</param>
		private void OnErrorsChanged([CallerMemberName] string propertyName = null)
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		#endregion
	}
}
