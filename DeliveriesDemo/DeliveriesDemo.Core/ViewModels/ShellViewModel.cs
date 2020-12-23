using DeliveriesDemo.Core.Commands;
using DeliveriesDemo.Core.Models.Interfaces;
using DeliveriesDemo.Core.ViewModels.Base;
using System;

namespace DeliveriesDemo.Core.ViewModels
{
	/// <summary>
	/// Shell of the application. Controls time speed.
	/// </summary>
	public class ShellViewModel : PropertyChangedNotifyBase
	{
		#region Private Members

		/// <summary>
		/// Time speed model to control.
		/// </summary>
		private readonly ITimeSpeedModel _timeSpeedModel;

		#endregion

		#region Public Properties

		/// <summary>
		/// Current time speed info for display.
		/// </summary>
		public string CurrentTimeSpeedInfo => $"Current time multiplier: x{_timeSpeedModel.TimeSpeedMultiplier}";

		/// <summary>
		/// Caption for the button that changes time speed.
		/// </summary>
		public string ChangeTimeSpeedButtonCaption
		{
			get
			{
				return _timeSpeedModel.TimeSpeedMultiplier switch
				{
					1 => "Speed up time!!!",
					_ => "Slow down time!"
				};
			}
		}

		/// <summary>
		/// Delivery creation view model to use.
		/// </summary>
		public DeliveryCreationViewModel DeliveryCreation { get; }

		/// <summary>
		/// Delivery list view model to use.
		/// </summary>
		public DeliveryListViewModel DeliveryList { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Parameterless constructor for XAML designer (WPF).
		/// </summary>
		public ShellViewModel()
		{
			DeliveryCreation = new DeliveryCreationViewModel();
			DeliveryList = new DeliveryListViewModel();
		}

		/// <summary>
		/// Main constructor.
		/// </summary>
		/// <param name="deliveryCreation">Delivery creation view model to use.</param>
		/// <param name="deliveryList">Delivery list view model to use.</param>
		/// <param name="timeSpeedModel">Time speed model to control.</param>
		public ShellViewModel(DeliveryCreationViewModel deliveryCreation, DeliveryListViewModel deliveryList, ITimeSpeedModel timeSpeedModel)
		{
			DeliveryCreation = deliveryCreation;
			DeliveryList = deliveryList;

			_timeSpeedModel = timeSpeedModel;
			_timeSpeedModel.TimeSpeedMultiplier = 1;

			ChangeTimeSpeedCommand = new RelayParameterlessCommand(ExecuteChangeTimeSpeed);
		}

		#endregion

		#region Commands

		/// <summary>
		/// Command that changes time speed.
		/// </summary>
		public IRelayCommand ChangeTimeSpeedCommand { get; }

		/// <summary>
		/// Logic that is executed by <see cref="ChangeTimeSpeedCommand"/>.
		/// </summary>
		private void ExecuteChangeTimeSpeed()
		{
			if (_timeSpeedModel.TimeSpeedMultiplier == 1)
			{
				// Speed up time.
				_timeSpeedModel.TimeSpeedMultiplier = 3241;
				InvokePropertyChanged(nameof(CurrentTimeSpeedInfo));
				InvokePropertyChanged(nameof(ChangeTimeSpeedButtonCaption));
			}
			else if (_timeSpeedModel.TimeSpeedMultiplier == 3241)
			{
				// Slow down to normal time speed.
				_timeSpeedModel.TimeSpeedMultiplier = 1;
				InvokePropertyChanged(nameof(CurrentTimeSpeedInfo));
				InvokePropertyChanged(nameof(ChangeTimeSpeedButtonCaption));
			}
			else
			{
				throw new NotImplementedException($"Time speed multiplier value of ({_timeSpeedModel.TimeSpeedMultiplier}) isn't supported");
			}
		}

		#endregion
	}
}
