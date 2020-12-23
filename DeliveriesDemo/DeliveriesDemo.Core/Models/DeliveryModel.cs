using DeliveriesDemo.Core.Models.Interfaces;

namespace DeliveriesDemo.Core.Models
{
	/// <summary>
	/// Simple <see cref="IDeliveryModel"/> implementation.
	/// </summary>
	public class DeliveryModel : IDeliveryModel
	{
		#region Public Properties

		public ICargoModel Cargo { get; set; }
		public IGeographicalRouteModel GeographicalRoute { get; set; }
		public ITruckModel Truck { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="cargo">Cargo to use.</param>
		/// <param name="geographicalRoute">Geographical route to use.</param>
		/// <param name="truck">Truck to use.</param>
		public DeliveryModel(ICargoModel cargo, IGeographicalRouteModel geographicalRoute, ITruckModel truck)
		{
			Cargo = cargo;
			GeographicalRoute = geographicalRoute;
			Truck = truck;
		}

		#endregion
	}
}
