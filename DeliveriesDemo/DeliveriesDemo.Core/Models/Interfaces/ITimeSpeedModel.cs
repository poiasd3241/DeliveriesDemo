namespace DeliveriesDemo.Core.Models.Interfaces
{
	/// <summary>
	/// Simple time speed interface.
	/// </summary>
	public interface ITimeSpeedModel
	{
		/// <summary>
		/// Multiplier of time speed.
		/// </summary>
		double TimeSpeedMultiplier { get; set; }
	}
}
