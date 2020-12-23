using DeliveriesDemo.Core.Models.Interfaces;
using System.Collections.Generic;

namespace DeliveriesDemo.Core
{
	/// <summary>
	/// Simple data provider interface.
	/// </summary>
	public interface IDataProvider
	{
		/// <summary>
		/// Gets a list of cargo names.
		/// </summary>
		List<string> GetCargoNames();

		/// <summary>
		/// Gets a list of city names.
		/// </summary>
		List<string> GetCities();

		/// <summary>
		/// Gets a list of trucks.
		/// </summary>
		List<ITruckModel> GetTrucks();

		/// <summary>
		/// Gets a list of geographical routes from two city names.
		/// </summary>
		/// <param name="origin">Name of the first city.</param>
		/// <param name="destination">Name of the second city.</param>
		IGeographicalRouteModel GetGeographicalRoute(string origin, string destination);

		/// <summary>
		/// Gets a truck from the truck model name.
		/// </summary>
		/// <param name="modelName">Truck model name.</param>
		ITruckModel GetTruckByModelName(string modelName);

		/// <summary>
		/// Gets a random item from the <see cref="List{T}"/>.
		/// </summary>
		/// <typeparam name="T">Type of items.</typeparam>
		/// <param name="items">List of items.</param>
		T GetRandomItem<T>(List<T> items);

		/// <summary>
		/// Gets two different random items from the <see cref="List{T}"/>
		/// </summary>
		/// <typeparam name="T">Type of items.</typeparam>
		/// <param name="items">List of items.</param>
		/// <param name="item1">First random item.</param>
		/// <param name="item2">Second random item.</param>
		void GetTwoRandomDifferentItems<T>(List<T> items, out T item1, out T item2);

		/// <summary>
		/// Gets a random <see cref="double"/> value from the specified range<br/>
		/// and rounds it according to the specified number of decimal places.
		/// </summary>
		/// <param name="min">Minimum result value.</param>
		/// <param name="max">Maximum result value.</param>
		/// <param name="decimalPlaces">Number of decimal places of result value.</param>
		double GetRandomDoubleFromRange(double min, double max, int decimalPlaces);
	}
}
