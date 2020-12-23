namespace DeliveriesDemo.Core.Models.Interfaces
{
	/// <summary>
	/// Simple geographical route interface.
	/// </summary>
	public interface IGeographicalRouteModel
	{
		/// <summary>
		/// Name of city-origin.
		/// </summary>
		string OriginCity { get; set; }

		/// <summary>
		/// Name of city-destination.
		/// </summary>
		string DestinationCity { get; set; }

		/// <summary>
		/// Route length between origin and destination cities.
		/// </summary>
		double RouteLengthMiles { get; set; }
	}
}
