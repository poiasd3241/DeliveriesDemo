using DeliveriesDemo.Core.Models.Interfaces;

namespace DeliveriesDemo.Core.Models
{
	/// <summary>
	/// Simple <see cref="ITruckModel"/> implementation.
	/// </summary>
	public class TruckModel : ITruckModel
	{
		#region Public Properties

		public string ModelName { get; set; }
		public PowerGrade PowerGrade { get; set; }
		public FuelEfficiencyGrade FuelEfficiencyGrade { get; set; }

		#endregion
	}
}
