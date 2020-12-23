using DeliveriesDemo.Core.Models;
using DeliveriesDemo.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace DeliveriesDemo.Core
{
	/// <summary>
	/// Custom <see cref="IDataProvider"/> implementation.
	/// </summary>
	public class DataProvider : IDataProvider
	{
		#region Private Members

		/// <summary>
		/// A <see cref="Random"/> to use in GetRandom methods
		/// </summary>
		private readonly Random _rnd = new Random();

		#endregion

		#region Private Methods

		private List<IGeographicalRouteModel> GetGeographicalRoutes()
		{
			return new List<IGeographicalRouteModel>
			{
				new GeographicalRouteModel
				{
					OriginCity = "Atlanta",
					DestinationCity = "Columbus",
					RouteLengthMiles = 567
				},
				new GeographicalRouteModel
				{
					OriginCity = "Atlanta",
					DestinationCity = "Indianapolis",
					RouteLengthMiles = 534
				},
				new GeographicalRouteModel
				{
					OriginCity = "Atlanta",
					DestinationCity = "Louisville",
					RouteLengthMiles = 421
				},
				new GeographicalRouteModel
				{
					OriginCity = "Atlanta",
					DestinationCity = "Nashville",
					RouteLengthMiles = 250
				},
				new GeographicalRouteModel
				{
					OriginCity = "Atlanta",
					DestinationCity = "St. Louis",
					RouteLengthMiles = 555
				},

				new GeographicalRouteModel
				{
					OriginCity = "Columbus",
					DestinationCity = "Indianapolis",
					RouteLengthMiles = 176
				},
				new GeographicalRouteModel
				{
					OriginCity = "Columbus",
					DestinationCity = "Louisville",
					RouteLengthMiles = 211
				},
				new GeographicalRouteModel
				{
					OriginCity = "Columbus",
					DestinationCity = "Nashville",
					RouteLengthMiles = 385
				},
				new GeographicalRouteModel
				{
					OriginCity = "Columbus",
					DestinationCity = "St. Louis",
					RouteLengthMiles = 418
				},

				new GeographicalRouteModel
				{
					OriginCity = "Indianapolis",
					DestinationCity = "Louisville",
					RouteLengthMiles = 114
				},
				new GeographicalRouteModel
				{
					OriginCity = "Indianapolis",
					DestinationCity = "Nashville",
					RouteLengthMiles = 289
				},
				new GeographicalRouteModel
				{
					OriginCity = "Indianapolis",
					DestinationCity = "St. Louis",
					RouteLengthMiles = 242
				},

				new GeographicalRouteModel
				{
					OriginCity = "Louisville",
					DestinationCity = "Nashville",
					RouteLengthMiles = 176
				},
				new GeographicalRouteModel
				{
					OriginCity = "Louisville",
					DestinationCity = "St. Louis",
					RouteLengthMiles = 262
				},

				new GeographicalRouteModel
				{
					OriginCity = "Nashville",
					DestinationCity = "St. Louis",
					RouteLengthMiles = 309
				}
			};
		}

		#endregion

		#region Public methods

		public List<string> GetCargoNames()
		{
			return new List<string>
			{
				"Bananas",
				"Canned beans",
				"Chocolate",
				"Coal",
				"Empty pallets",
				"Ice cream",
				"Large tubes",
				"Lemonade",
				"Milk",
				"Outdoor floor tiles",
				"Sand",
				"Sawdust panels",
				"Used packaging",
			};
		}

		public List<string> GetCities()
		{
			return new List<string>
			{
				"Atlanta",
				"Columbus",
				"Indianapolis",
				"Louisville",
				"Nashville",
				"St. Louis"
			};
		}

		public List<ITruckModel> GetTrucks()
		{
			return new List<ITruckModel>
			{
				new TruckModel
				{
					ModelName = "Cyber Tractor",
					PowerGrade = PowerGrade.High,
					FuelEfficiencyGrade = FuelEfficiencyGrade.High
				},
				new TruckModel
				{
					ModelName = "Decent Wheels",
					PowerGrade = PowerGrade.Medium,
					FuelEfficiencyGrade = FuelEfficiencyGrade.Medium
				},
				new TruckModel
				{
					ModelName = "Monster V64",
					PowerGrade = PowerGrade.High,
					FuelEfficiencyGrade = FuelEfficiencyGrade.Low
				},
				new TruckModel
				{
					ModelName = "Thirsty Snail",
					PowerGrade = PowerGrade.Low,
					FuelEfficiencyGrade = FuelEfficiencyGrade.Low
				},
			};
		}

		public T GetRandomItem<T>(List<T> items) => items[_rnd.Next(items.Count)];

		public double GetRandomDoubleFromRange(double min, double max, int decimalPlaces)
		{
			return Math.Round(_rnd.NextDouble() * (max - min) + min, decimalPlaces);
		}

		public void GetTwoRandomDifferentItems<T>(List<T> items, out T item1, out T item2)
		{
			item1 = items[_rnd.Next(items.Count)];
			do
			{
				item2 = items[_rnd.Next(items.Count)];
			}
			while (item2.Equals(item1));
		}

		public IGeographicalRouteModel GetGeographicalRoute(string origin, string destination)
		{
			// Looks for a route regardless of origin and destination order.
			var route = GetGeographicalRoutes().Find(x =>
				x.OriginCity == origin && x.DestinationCity == destination ||
				x.OriginCity == destination && x.DestinationCity == origin);

			return route ??
			throw new Exception($"{nameof(GetGeographicalRoute)} method couldn't find a geographical route " +
				$"corresponding to \"{origin}\" (origin) and \"{destination}\" (destination) cities.");
		}

		public ITruckModel GetTruckByModelName(string modelName)
		{
			return GetTrucks().Find(x => x.ModelName == modelName);

			throw new Exception($"{nameof(GetTruckByModelName)} method couldn't find a truck with the model name \"{modelName}\"");
		}

		#endregion
	}
}
