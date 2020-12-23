namespace DeliveriesDemo.Core.Models.Interfaces
{
	/// <summary>
	/// Simple cargo interface.
	/// </summary>
	public interface ICargoModel
	{
		/// <summary>
		/// Name of cargo.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Weight of cargo in lbs.
		/// </summary>
		double WeightLbs { get; set; }
	}
}
