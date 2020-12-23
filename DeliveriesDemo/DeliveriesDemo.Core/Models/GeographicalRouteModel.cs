using DeliveriesDemo.Core.Models.Interfaces;

namespace DeliveriesDemo.Core.Models
{
	/// <summary>
	/// Simple <see cref="IGeographicalRouteModel"/> implementation.
	/// </summary>
	public class GeographicalRouteModel : IGeographicalRouteModel
	{
		#region Public Properties

		public string OriginCity { get; set; }
		public string DestinationCity { get; set; }
		public double RouteLengthMiles { get; set; }

		#endregion
	}
}
