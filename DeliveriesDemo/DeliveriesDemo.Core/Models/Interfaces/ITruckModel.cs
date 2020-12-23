namespace DeliveriesDemo.Core.Models.Interfaces
{
	/// <summary>
	/// Simple truck interface.
	/// </summary>
	public interface ITruckModel
	{
		/// <summary>
		/// Name of truck model.
		/// </summary>
		public string ModelName { get; set; }

		/// <summary>
		/// Measure of truck power. Affects truck speed.
		/// </summary>
		public PowerGrade PowerGrade { get; set; }

		/// <summary>
		/// Measure of truck fuel efficiency. Affects truck fuel consumption.
		/// </summary>
		public FuelEfficiencyGrade FuelEfficiencyGrade { get; set; }
	}
}
