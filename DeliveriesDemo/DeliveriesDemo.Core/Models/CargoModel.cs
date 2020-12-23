using DeliveriesDemo.Core.Models.Interfaces;

namespace DeliveriesDemo.Core.Models
{
	/// <summary>
	/// Simple <see cref="ICargoModel"/> implementation.
	/// </summary>
	public class CargoModel : ICargoModel
	{
		#region Public Properties

		public string Name { get; set; }
		public double WeightLbs { get; set; }

		#endregion
	}
}
