using DeliveriesDemo.Core.Models;
using DeliveriesDemo.Core.Models.Interfaces;
using DeliveriesDemo.Core.Validation;
using DeliveriesDemo.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DeliveriesDemo.Core
{
	/// <summary>
	/// Dependency injection helper class.
	/// </summary>
	public class DI
	{
		/// <summary>
		/// Registers interfaces and classes.<br/>
		/// Should be only used during app startup (App.xaml.cs for WPF).
		/// </summary>
		/// <returns>A service provider for resolving dependencies.</returns>
		public static IServiceProvider Setup()
		{
			var services = new ServiceCollection();

			// Register Models.
			services.AddSingleton<ITimeSpeedModel, TimeSpeedModel>();

			services.AddTransient<ICargoModel, CargoModel>();
			services.AddTransient<IGeographicalRouteModel, GeographicalRouteModel>();
			services.AddTransient<ITruckModel, TruckModel>();
			services.AddTransient<IDeliveryModel, DeliveryModel>();

			// Register helpers/data providers.
			services.AddSingleton<IDataProvider, DataProvider>();
			services.AddTransient<IPropertyValidator, PropertyValidator>();

			// Register ViewModels.

			services.AddSingleton<DeliveryCreationViewModel>();
			services.AddTransient<DeliveryListItemViewModel>();
			services.AddSingleton<DeliveryListViewModel>();

			services.AddSingleton<ShellViewModel>();

			// Build service provider.
			ServiceProvider serviceProvider = services.BuildServiceProvider();
			return serviceProvider;
		}
	}
}
