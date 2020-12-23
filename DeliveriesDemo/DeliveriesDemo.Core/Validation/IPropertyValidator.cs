using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeliveriesDemo.Core.Validation
{
	/// <summary>
	/// Custom <see cref="INotifyDataErrorInfo"/> implementation.
	/// </summary>
	public interface IPropertyValidator : INotifyDataErrorInfo
	{
		/// <summary>
		/// Adds an error message for the specified property name.
		/// </summary>
		/// <param name="errorMessage">Error message to add.</param>
		/// <param name="propertyName">Name of invalid property.</param>
		void AddError(string errorMessage, [CallerMemberName] string propertyName = null);

		/// <summary>
		/// Removes all error messages for the specified property name.
		/// </summary>
		/// <param name="errorMessage">Error message to add.</param>
		/// <param name="propertyName">Name of invalid property.</param>
		void ClearErrors([CallerMemberName] string propertyName = null);

		/// <summary>
		/// Gets all error messages for all property names.
		/// </summary>
		List<string> GetAllErrors();

		/// <summary>
		/// Gets all error messages for the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of property.</param>
		/// <returns></returns>
		new IEnumerable GetErrors([CallerMemberName] string propertyName = null);
	}
}
