using DeliveriesDemo.Core.Models.Interfaces;
using DeliveriesDemo.Core.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeliveriesDemo.Core.ViewModels
{
	/// <summary>
	/// A view model for each active delivery in the active deliveries list <see cref="DeliveryListViewModel.Items"/>.
	/// </summary>
	public class DeliveryListItemViewModel : PropertyChangedNotifyBase
	{
		#region Private Members

		/// <summary>
		/// Remaining route length in miles (to destination city).
		/// </summary>
		private double _routeLengthMilesRemaining;

		/// <summary>
		/// Total route length in miles between origin and destination cities.
		/// </summary>
		private readonly double _routeLengthMilesTotal;

		/// <summary>
		/// Average speed in mph of the truck.<br/>
		/// Value depends on the cargo weight and the truck <see cref="PowerGrade"/>.
		/// </summary>
		private readonly double _averageSpeedMph;

		/// <summary>
		/// Average fuel economy in mpg of the truck.<br/>
		/// Value depends on the cargo weight and the truck <see cref="FuelEfficiencyGrade"/>.
		/// </summary>
		private readonly double _averageFuelEconomyMpg;

		/// <summary>
		/// Time speed model to use.
		/// </summary>
		private readonly ITimeSpeedModel _timeSpeedModel;

		/// <summary>
		/// Delivery list to remove this delivery from once destination is reached.
		/// </summary>
		private readonly DeliveryListViewModel _deliveryList;

		#endregion

		#region Private Properties

		/// <summary>
		/// Time left to reach destination city.
		/// </summary>
		private TimeSpan RemainingDeliveryTime => TimeSpan.FromHours(_routeLengthMilesRemaining / _averageSpeedMph);

		#endregion

		#region Public Properties

		/// <summary>
		/// Name of city where the cargo is picked up for delivery.
		/// </summary>
		public string OriginName { get; }

		/// <summary>
		/// Name of city where the cargo is to be delivered to.
		/// </summary>
		public string DestinationName { get; }

		/// <summary>
		/// Composite string for displaying information about the cargo name and weight.
		/// </summary>
		public string CargoDisplayInfo { get; }

		/// <summary>
		/// Name of truck model.
		/// </summary>
		public string TruckModel { get; }

		/// <summary>
		/// Average speed in mph of the truck (formatted for display).
		/// </summary>
		public string AverageSpeedMphDisplay => _averageSpeedMph.ToString("0.00");

		/// <summary>
		/// Average fuel economy in mpg of the truck (formatted for display).
		/// </summary>
		public string AverageFuelEconomyMpgDisplay => _averageFuelEconomyMpg.ToString("0.00");

		/// <summary>
		/// Time left to reach destination city (formatted for display).
		/// </summary>
		public string RemainingDeliveryTimeDisplay => $"{(int)RemainingDeliveryTime.TotalHours}:{RemainingDeliveryTime:mm}:{RemainingDeliveryTime:ss}";

		/// <summary>
		/// Remaining route length in miles (to destination city) (formatted for display).
		/// </summary>
		public string RouteLengthMilesRemainingDisplay => _routeLengthMilesRemaining.ToString("0.00");

		/// <summary>
		/// Indicates delivery progress.
		/// </summary>
		public double DeliveryCompletedPercent => 100 - _routeLengthMilesRemaining / _routeLengthMilesTotal * 100;

		#endregion

		#region Constructors

		/// <summary>
		/// Parameterless constructor for XAML designer (WPF).
		/// </summary>
		public DeliveryListItemViewModel() { }

		/// <summary>
		/// Main constructor.
		/// </summary>
		/// <param name="delivery">Delivery to use.</param>
		/// <param name="timeSpeedModel">Time speed model to use.</param>
		/// <param name="deliveryList">Delivery list to remove created delivery item from once destination is reached.</param>
		public DeliveryListItemViewModel(IDeliveryModel delivery, ITimeSpeedModel timeSpeedModel, DeliveryListViewModel deliveryList)
		{
			_timeSpeedModel = timeSpeedModel;
			_deliveryList = deliveryList;

			CargoDisplayInfo = $"{delivery.Cargo.Name} ({delivery.Cargo.WeightLbs} lbs)";
			OriginName = delivery.GeographicalRoute.OriginCity;
			DestinationName = delivery.GeographicalRoute.DestinationCity;
			TruckModel = delivery.Truck.ModelName;

			_routeLengthMilesTotal = delivery.GeographicalRoute.RouteLengthMiles;
			_routeLengthMilesRemaining = _routeLengthMilesTotal;

			_averageSpeedMph = CalculateAverageSpeedMph(delivery.Cargo.WeightLbs, delivery.Truck.PowerGrade);
			_averageFuelEconomyMpg = CalculateAverageFuelEconomyMpg(delivery.Cargo.WeightLbs, delivery.Truck.FuelEfficiencyGrade);

			Task.Run(async () => await StartDelivery());
		}

		#endregion

		#region Private Tasks

		/// <summary>
		/// Logic for changing delivery status and updating UI.<br/>
		/// Removes the delivery once it is completed from delivery list.
		/// </summary>
		private async Task StartDelivery()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			while (_routeLengthMilesRemaining > 0)
			{
				// TODO: Find a better solution to keeping track of time?

				// Stopwatch adds a small performance hit, but allows for a higher quality time tracking than just using Task.Delay.
				// Task.Delay doesn't take into account the execution time of the code that is executed between Task.Delay() calls.

				await Task.Delay(50);

				_routeLengthMilesRemaining -= _averageSpeedMph * ((double)stopwatch.ElapsedMilliseconds / 3600000) * _timeSpeedModel.TimeSpeedMultiplier;

				stopwatch.Restart();

				if (_routeLengthMilesRemaining < 0)
				{
					// Avoid negative values.
					_routeLengthMilesRemaining = 0;
				}

				// Update UI.
				InvokePropertyChanged(nameof(RemainingDeliveryTimeDisplay));
				InvokePropertyChanged(nameof(RouteLengthMilesRemainingDisplay));
				InvokePropertyChanged(nameof(DeliveryCompletedPercent));
			}

			// Remove this completed delivery from delivery list.
			_deliveryList.DeliveryListSyncContext.Post((o) => _deliveryList.Items.Remove(this), null);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Returns average speed of the truck in mph according to the cargo weight and the truck power grade.
		/// </summary>
		/// <param name="cargoWeightLbs">Weight of transported cargo in lbs.</param>
		/// <param name="powerGrade">Power grade of the truck.</param>
		private double CalculateAverageSpeedMph(double cargoWeightLbs, PowerGrade powerGrade)
		{
			// Base value.
			double averageSpeedMph = 45;

			// Adjust for truck power grade.
			averageSpeedMph += powerGrade switch
			{
				PowerGrade.Low => -10,
				PowerGrade.Medium => 0,
				PowerGrade.High => 20,
				_ => throw new ArgumentException("Unknown PowerGrade value", nameof(powerGrade))
			};

			// Adjust for cargo weight.
			averageSpeedMph -= cargoWeightLbs / 5000;

			return averageSpeedMph;
		}

		/// <summary>
		/// Returns average fuel economy of the truck in mpg according to the cargo weight and the truck fuel efficiency grade.
		/// </summary>
		/// <param name="cargoWeightLbs">Weight of transported cargo in lbs.</param>
		/// <param name="fuelEfficiencyGrade">Fuel efficiency grade of the truck.</param>
		private double CalculateAverageFuelEconomyMpg(double cargoWeightLbs, FuelEfficiencyGrade fuelEfficiencyGrade)
		{
			// Base value.
			double averageFuelEconomyMpg = 7;

			// Adjust for truck fuel efficiency.
			averageFuelEconomyMpg += fuelEfficiencyGrade switch
			{
				FuelEfficiencyGrade.Low => -1,
				FuelEfficiencyGrade.Medium => 0,
				FuelEfficiencyGrade.High => 3,
				_ => throw new ArgumentException("Unknown FuelEfficiencyGrade value", nameof(fuelEfficiencyGrade))
			};

			// Adjust for cargo weight.
			averageFuelEconomyMpg -= cargoWeightLbs / 20000;

			return averageFuelEconomyMpg;
		}

		#endregion
	}
}
