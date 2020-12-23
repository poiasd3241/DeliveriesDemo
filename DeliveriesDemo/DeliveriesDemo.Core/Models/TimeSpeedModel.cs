using DeliveriesDemo.Core.Models.Interfaces;

namespace DeliveriesDemo.Core.Models
{
	/// <summary>
	/// Simple <see cref="ITimeSpeedModel"/> implementation.
	/// </summary>
	public class TimeSpeedModel : ITimeSpeedModel
	{
		#region Public Properties

		public double TimeSpeedMultiplier { get; set; }

		#endregion
	}
}
