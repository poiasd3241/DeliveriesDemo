namespace DeliveriesDemo.Core.Models.Interfaces
{
	/// <summary>
	/// Simple delivery interface
	/// </summary>
	public interface IDeliveryModel
	{
		/// <summary>
		/// Cargo that is delivered.
		/// </summary>
		ICargoModel Cargo { get; set; }

		/// <summary>
		/// Geographical route of delivery.
		/// </summary>
		IGeographicalRouteModel GeographicalRoute { get; set; }

		/// <summary>
		/// Truck that is used for delivery.
		/// </summary>
		ITruckModel Truck { get; set; }
	}
}
