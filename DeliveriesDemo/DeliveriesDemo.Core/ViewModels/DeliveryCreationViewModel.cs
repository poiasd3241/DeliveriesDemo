using DeliveriesDemo.Core.Commands;
using DeliveriesDemo.Core.Models.Interfaces;
using DeliveriesDemo.Core.Validation;
using DeliveriesDemo.Core.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DeliveriesDemo.Core.ViewModels
{
	/// <summary>
	/// A view model for creating a new <see cref="DeliveryListItemViewModel"/>.
	/// </summary>
	public class DeliveryCreationViewModel : PropertyChangedNotifyBase
	{
		#region Private Members

		/// <summary>
		/// Property validator to use.
		/// </summary>
		private readonly IPropertyValidator _validator;

		/// <summary>
		/// Data provider to use.
		/// </summary>
		private readonly IDataProvider _dataProvider;

		/// <summary>
		/// Delivery to edit and create.
		/// </summary>
		private readonly IDeliveryModel _delivery;

		/// <summary>
		/// Time speed model to use.
		/// </summary>
		private readonly ITimeSpeedModel _timeSpeedModel;

		/// <summary>
		/// Delivery list to insert a created delivery into.
		/// </summary>
		private readonly DeliveryListViewModel _deliveryList;

		#endregion

		#region Private Properties

		/// <summary>
		/// Returns an <see cref="ITruckModel"/> from selected truck model name.<br/>
		/// If selected truck model name is <see langword="null"/>, returns <see langword="null"/>.
		/// </summary>
		private ITruckModel SelectedTruck => SelectedTruckModelName != null ? _dataProvider.GetTruckByModelName(SelectedTruckModelName) : null;

		/// <summary>
		/// Returns <see langword="true"/> if the number of active deliveries has reached its limit; otherwise, <see langword="false"/>.
		/// </summary>
		private bool ActiveDeliveriesAmountLimitReached => _deliveryList.ActiveDeliveriesAmountLimitReached;

		#endregion

		#region Public Properties

		/// <summary>
		/// Returns <see langword="true"/> if the user needs to be notified about invalid cargo weight input; 
		/// otherwise, <see langword="false"/>.
		/// </summary>
		public bool CargoWeightLbsHasErrorMessagesForUser
		{
			get
			{
				var cargoWeightErrors = _validator.GetErrors(nameof(CargoWeightLbsText));

				if (cargoWeightErrors == null ||
					// NULL and empty error messages don't need to be displayed to the user.
					cargoWeightErrors.Cast<string>().Where(errMsg => errMsg != null && errMsg != string.Empty).Count() == 0)
				{
					return false;
				}

				return true;
			}
		}

		/// <summary>
		/// Error messages to display to the user regarding invalid data input.
		/// </summary>
		public List<string> ValidationErrorMessages => _validator.GetAllErrors().FindAll(errMsg => errMsg != null && errMsg != string.Empty);

		/// <summary>
		/// List of names of all available cargo.
		/// </summary>
		public List<string> AvailableCargoNames => _dataProvider.GetCargoNames();

		/// <summary>
		/// List of model names of all available trucks.
		/// </summary>
		public List<string> AvaliableTruckModelNames => _dataProvider.GetTrucks().Select(x => x.ModelName).ToList();

		/// <summary>
		/// Truck power grade displayed using equivalent amount of stars ★ (U+2605)
		/// </summary>
		public string TruckPowerGradeStars => ConvertTruckPowerGradeToStars();

		/// <summary>
		/// Truck fuel efficiency grade displayed using equivalent amount of stars ★ (U+2605)
		/// </summary>
		public string TruckFuelEfficiencyGradeStars => ConvertTruckFuelEfficiencyGradeToStars();

		#endregion

		#region Public Properties With Backing Fields

		private string _selectedCargoName;

		/// <summary>
		/// Selected cargo name.
		/// </summary>
		public string SelectedCargoName
		{
			get => _selectedCargoName;
			set
			{
				if (_selectedCargoName != value)
				{
					var oldCargoName = _selectedCargoName;
					_selectedCargoName = value;

					if (oldCargoName == null || value == null)
					{
						// When this property changes from NULL to NOT NULL and vice versa,
						// Reevaluate the dependent commands' CanExecute.
						CreateDeliveryCommand.InvokeCanExecuteChanged();
						ClearFieldsCommand.InvokeCanExecuteChanged();
					}

					InvokePropertyChanged();
				}
			}
		}


		private string _cargoWeightLbsText;

		/// <summary>
		/// Cargo weight input value.
		/// </summary>
		public string CargoWeightLbsText
		{
			get => _cargoWeightLbsText;
			set
			{
				value = value.Trim();

				if (_cargoWeightLbsText != value)
				{
					var oldValue = _cargoWeightLbsText;
					_cargoWeightLbsText = value;

					if ((oldValue == null || oldValue == string.Empty) && value != null && value != string.Empty ||
						oldValue != null && oldValue != string.Empty && (value != null || value != string.Empty))
					{
						// When this property changes from (NULL/empty) to (NOT NULL & NOT empty) and vice versa,
						// Reevaluate the dependent command's CanExecute.
						ClearFieldsCommand.InvokeCanExecuteChanged();
					}

					if (ValidateCargoWeight(_cargoWeightLbsText))
					{
						InvokePropertyChanged();
					}
				}
			}
		}


		private List<string> _availableOriginCities;

		/// <summary>
		/// List of names of available origin cities.
		/// </summary>
		public List<string> AvailableOriginCities
		{
			get => _availableOriginCities;
			set
			{
				if (_availableOriginCities != value)
				{
					_availableOriginCities = value;
				}
			}
		}


		private string _selectedOriginCity;

		/// <summary>
		/// Selected origin city (name).
		/// </summary>
		public string SelectedOriginCity
		{
			get => _selectedOriginCity;
			set
			{
				if (_selectedOriginCity != value)
				{
					var oldOriginValue = _selectedOriginCity;
					_selectedOriginCity = value;

					if (oldOriginValue == null || value == null)
					{
						// When this property changes from NULL to NOT NULL and vice versa,
						// Reevaluate the dependent commands' CanExecute.
						CreateDeliveryCommand.InvokeCanExecuteChanged();
						ClearFieldsCommand.InvokeCanExecuteChanged();
					}

					HandleOriginSelection(oldOriginValue, value);

					InvokePropertyChanged();
				}
			}
		}


		private List<string> _availableDestinationCities;

		/// <summary>
		/// List of names of available destination cities.
		/// </summary>
		public List<string> AvailableDestinationCities
		{
			get => _availableDestinationCities;
			set
			{
				if (_availableDestinationCities != value)
				{
					_availableDestinationCities = value;
				}
			}
		}


		private string _selectedDestinationCity;

		/// <summary>
		/// Selected destination city (name).
		/// </summary>
		public string SelectedDestinationCity
		{
			get => _selectedDestinationCity;
			set
			{
				if (_selectedDestinationCity != value)
				{
					var oldDestinationValue = _selectedDestinationCity;
					_selectedDestinationCity = value;

					if (oldDestinationValue == null || value == null)
					{
						// When this property changes from NULL to NOT NULL and vice versa,
						// Reevaluate the dependent commands' CanExecute.
						CreateDeliveryCommand.InvokeCanExecuteChanged();
						ClearFieldsCommand.InvokeCanExecuteChanged();
					}

					HandleDestinationSelection(oldDestinationValue, value);

					InvokePropertyChanged();
				}
			}
		}


		private string _selectedTruckModelName;

		/// <summary>
		/// Selected truck model name.
		/// </summary>
		public string SelectedTruckModelName
		{
			get => _selectedTruckModelName;
			set
			{
				if (_selectedTruckModelName != value)
				{
					var oldTruck = _selectedTruckModelName;
					_selectedTruckModelName = value;


					if (oldTruck == null || value == null)
					{
						// When this property changes from NULL to NOT NULL and vice versa,
						// Reevaluate the dependent commands' CanExecute.
						CreateDeliveryCommand.InvokeCanExecuteChanged();
						ClearFieldsCommand.InvokeCanExecuteChanged();
					}

					InvokePropertyChanged();

					// Update dependent properties.
					InvokePropertyChanged(nameof(TruckPowerGradeStars));
					InvokePropertyChanged(nameof(TruckFuelEfficiencyGradeStars));
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Parameterless constructor for XAML designer (WPF).
		/// </summary>
		public DeliveryCreationViewModel()
		{
			_validator = new PropertyValidator();
			_validator.AddError("Cargo weight must be between 10000 and 45000 lbs.", nameof(CargoWeightLbsText));
			_validator.AddError("Can't have more than 10 active deliveries.", nameof(ActiveDeliveriesAmountLimitReached));
		}

		/// <summary>
		/// Main constructor.
		/// </summary>
		/// <param name="propertyValidator">Property validator to use.</param>
		/// <param name="dataProvider">Data provider to use.</param>
		/// <param name="delivery">Delivery to edit and create.</param>
		/// <param name="timeSpeedModel">Time speed model to use.</param>
		/// <param name="deliveryList">Delivery list to insert a created delivery into.</param>
		public DeliveryCreationViewModel(IPropertyValidator propertyValidator, IDataProvider dataProvider, IDeliveryModel delivery,
								   ITimeSpeedModel timeSpeedModel, DeliveryListViewModel deliveryList)
		{
			_validator = propertyValidator;
			_validator.ErrorsChanged += OnValidatorErrorsChanged;

			_dataProvider = dataProvider;
			_delivery = delivery;
			_timeSpeedModel = timeSpeedModel;

			_deliveryList = deliveryList;
			_deliveryList.Items.CollectionChanged += OnDeliveryListItemsCollectionChanged;

			CreateDeliveryCommand = new RelayParameterlessCommand(ExecuteCreateDelivery, CanExecuteCreateDelivery);
			FillInRandomDataCommand = new RelayParameterlessCommand(ExecuteFillInRandomData);
			ClearFieldsCommand = new RelayParameterlessCommand(ExecuteClearFields, CanExecuteClearFields);

			// Populate available cities lists during initialization.
			AvailableOriginCities = _dataProvider.GetCities();
			AvailableDestinationCities = _dataProvider.GetCities();
		}

		#endregion

		#region Data Validation

		/// <summary>
		/// Called when validation errors change. Updates dependent properties and reevaluates dependent commands' CanExecute.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event data.</param>
		private void OnValidatorErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			InvokePropertyChanged(nameof(ValidationErrorMessages));
			CreateDeliveryCommand.InvokeCanExecuteChanged();
		}

		/// <summary>
		/// Called when delivery list items change. Updates dependent properties.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event data.</param>
		private void OnDeliveryListItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			_validator.ClearErrors(nameof(ActiveDeliveriesAmountLimitReached));
			if (ActiveDeliveriesAmountLimitReached)
			{
				_validator.AddError("Can't have more than 10 active deliveries.", nameof(ActiveDeliveriesAmountLimitReached));
			}
		}

		/// <summary>
		/// Validation logic for cargo weight input.
		/// </summary>
		/// <param name="value">Input cargo weight value to validate.</param>
		/// <returns><see langword="true"/> if cargo weight input is valid; otherwise, <see langword="false"/>.</returns>
		private bool ValidateCargoWeight(string value)
		{
			_validator.ClearErrors(nameof(CargoWeightLbsText));

			bool isValid = false;

			if (value == null || value == string.Empty)
			{
				// Add a NULL error message that shouldn't be displayed to the user.
				_validator.AddError(null, nameof(CargoWeightLbsText));
			}
			// Check if the input is a number.
			else if (double.TryParse(value.Replace(',', '.'), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out double number))
			{
				// Check if the input is in the correct range.
				if (number >= 10000 && number <= 45000)
				{
					isValid = true;
				}
				else
				{
					_validator.AddError("Cargo weight must be between 10000 and 45000 lbs.", nameof(CargoWeightLbsText));
				}
			}
			else
			{
				_validator.AddError("Cargo weight must be a number.", nameof(CargoWeightLbsText));
			}

			InvokePropertyChanged(nameof(CargoWeightLbsHasErrorMessagesForUser));
			return isValid;
		}

		#endregion

		#region Commands

		/// <summary>
		/// Command that adds a user-defined delivery to the delivery list.
		/// </summary>
		public IRelayCommand CreateDeliveryCommand { get; }

		/// <summary>
		/// Determines if <see cref="CreateDeliveryCommand"/> can execute.
		/// </summary>
		/// <returns><see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.</returns>
		private bool CanExecuteCreateDelivery()
		{
			// Check if all input fields are NOT (NULL/empty)
			// and there are no data validation errors.
			if (CargoWeightLbsText?.Length > 0 &&
				_validator.HasErrors == false &&
				SelectedCargoName != null &&
				SelectedOriginCity != null &&
				SelectedDestinationCity != null &&
				SelectedTruckModelName != null)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Logic that is executed by <see cref="CreateDeliveryCommand"/>.
		/// </summary>
		private void ExecuteCreateDelivery()
		{
			// Update delivery with valid data.
			_delivery.Cargo.Name = SelectedCargoName;
			_delivery.Cargo.WeightLbs = double.Parse(CargoWeightLbsText);
			_delivery.GeographicalRoute = _dataProvider.GetGeographicalRoute(SelectedOriginCity, SelectedDestinationCity);
			_delivery.Truck = SelectedTruck;

			// TODO: Replace "new" with DI if possible.
			_deliveryList.Items.Add(new DeliveryListItemViewModel(_delivery, _timeSpeedModel, _deliveryList));
		}

		/// <summary>
		/// Command that fills in random valid input data for quick delivery creation.
		/// </summary>
		public IRelayCommand FillInRandomDataCommand { get; }

		/// <summary>
		/// Logic that is executed by <see cref="FillInRandomDataCommand"/>.
		/// </summary>
		private void ExecuteFillInRandomData()
		{
			SelectedCargoName = _dataProvider.GetRandomItem(_dataProvider.GetCargoNames());
			CargoWeightLbsText = _dataProvider.GetRandomDoubleFromRange(10000, 45000, 2).ToString();
			_dataProvider.GetTwoRandomDifferentItems(_dataProvider.GetCities(), out var origin, out var destination);
			SelectedOriginCity = origin;
			SelectedDestinationCity = destination;
			SelectedTruckModelName = _dataProvider.GetRandomItem(_dataProvider.GetTrucks()).ModelName;
		}

		/// <summary>
		/// Command that clears input fields.
		/// </summary>
		public IRelayCommand ClearFieldsCommand { get; }

		/// <summary>
		/// Determines if <see cref="ClearFieldsCommand"/> can execute.
		/// </summary>
		/// <returns><see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.</returns>
		private bool CanExecuteClearFields()
		{
			// Check if any input field is NOT (NULL/empty).
			if (CargoWeightLbsText?.Length > 0 ||
				SelectedCargoName != null ||
				SelectedOriginCity != null ||
				SelectedDestinationCity != null ||
				SelectedTruckModelName != null)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Logic that is executed by <see cref="ClearFieldsCommand"/>.
		/// </summary>
		private void ExecuteClearFields()
		{
			// Clear fields only if they are (NULL/empty).

			if (CargoWeightLbsText?.Length > 0)
			{
				CargoWeightLbsText = string.Empty;
				InvokePropertyChanged(nameof(CargoWeightLbsText));
			}

			if (SelectedCargoName != null)
			{
				SelectedCargoName = null;
			}

			if (SelectedOriginCity != null)
			{
				SelectedOriginCity = null;
			}

			if (SelectedDestinationCity != null)
			{
				SelectedDestinationCity = null;
			}

			if (SelectedTruckModelName != null)
			{
				SelectedTruckModelName = null;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the <see cref="AvailableOriginCities"/>,<br/>
		/// <see cref="AvailableDestinationCities"/> and<br/>
		/// <see cref="SelectedDestinationCity"/> if necessary.
		/// </summary>
		/// <param name="oldOrigin">Old (previous) origin city name (before selection changed).</param>
		/// <param name="newOrigin">New (current) origin city name (after selection changed).</param>
		/// <param name="caller">Name of this method's caller.</param>
		private void HandleOriginSelection(string oldOrigin, string newOrigin, [CallerMemberName] string caller = null)
		{
			if (caller != nameof(SelectedOriginCity))
			{
				throw new MethodAccessException($"Access of the {nameof(HandleOriginSelection)} method from ({caller}) isn't supported.");
			}

			if (oldOrigin == newOrigin)
			{
				throw new ArgumentException($"Equal old and new origin values aren't supported in this ({nameof(HandleOriginSelection)}) method.");
			}

			if (oldOrigin == null)
			{
				// Origin changed from NULL to NOT NULL.

				if (SelectedDestinationCity == null)
				{
					// Update the destinations list to exclude the new origin.
					AvailableDestinationCities = _dataProvider.GetCities();
					AvailableDestinationCities.Remove(newOrigin);
					InvokePropertyChanged(nameof(AvailableDestinationCities));
				}
				else
				{
					// Update the origins list to include all cities (to allow swapping selected origin and destination).
					AvailableOriginCities = _dataProvider.GetCities();
					InvokePropertyChanged(nameof(AvailableOriginCities));
				}
			}
			else
			{
				// Origin changed from NOT NULL...

				if (newOrigin == null)
				{
					// ...to NULL.

					if (SelectedDestinationCity == null)
					{
						// Update the destinations list to include all cities (to allow swapping selected origin and destination).
						AvailableDestinationCities = _dataProvider.GetCities();
						InvokePropertyChanged(nameof(AvailableDestinationCities));
					}
					else
					{
						// Update the origins list to exclude the selected destination.
						AvailableOriginCities = _dataProvider.GetCities();
						AvailableOriginCities.Remove(SelectedDestinationCity);
						InvokePropertyChanged(nameof(AvailableOriginCities));
					}
				}
				else
				{
					// ...to NOT NULL (different  value).

					if (SelectedDestinationCity == null)
					{
						// Update the destinations list to exclude the new origin.
						AvailableDestinationCities = _dataProvider.GetCities();
						AvailableDestinationCities.Remove(newOrigin);
						InvokePropertyChanged(nameof(AvailableDestinationCities));
					}
					else if (newOrigin == SelectedDestinationCity)
					{
						// Swap the selected origin and destination.
						SelectedDestinationCity = oldOrigin;
					}
				}
			}
		}

		/// <summary>
		/// Updates the <see cref="AvailableOriginCities"/>,<br/>
		/// <see cref="AvailableDestinationCities"/> and<br/>
		/// <see cref="SelectedOriginCity"/> if necessary.
		/// </summary>
		/// <param name="oldDestination">Old (previous) destination city name (before selection changed).</param>
		/// <param name="newDestination">New (current) destination city name (after selection changed).</param>
		/// <param name="caller">Name of this method's caller.</param>
		private void HandleDestinationSelection(string oldDestination, string newDestination, [CallerMemberName] string caller = null)
		{
			if (caller != nameof(SelectedDestinationCity))
			{
				throw new MethodAccessException($"Access of the {nameof(HandleDestinationSelection)} method from ({caller}) isn't supported.");
			}

			if (oldDestination == newDestination)
			{
				throw new ArgumentException($"Equal old and new destination values aren't supported in this ({nameof(HandleDestinationSelection)}) method.");
			}

			if (oldDestination == null)
			{
				// Destination changed from NULL to NOT NULL.

				if (SelectedOriginCity == null)
				{
					// Update the origins list to exclude the new destination.
					AvailableOriginCities = _dataProvider.GetCities();
					AvailableOriginCities.Remove(newDestination);
					InvokePropertyChanged(nameof(AvailableOriginCities));
				}
				else
				{
					// Update the destinations list to include all cities (to allow swapping selected origin and destination).
					AvailableDestinationCities = _dataProvider.GetCities();
					InvokePropertyChanged(nameof(AvailableDestinationCities));
				}
			}
			else
			{
				// Destination changed from NOT NULL...

				if (newDestination == null)
				{
					// ...to NULL.

					if (SelectedOriginCity == null)
					{
						// Update the origins list to include all cities to allow swapping selected origin and destination.
						AvailableOriginCities = _dataProvider.GetCities();
						InvokePropertyChanged(nameof(AvailableOriginCities));
					}
					else
					{
						// Update the destinations list to exclude the selected origin.
						AvailableDestinationCities = _dataProvider.GetCities();
						AvailableDestinationCities.Remove(SelectedOriginCity);
						InvokePropertyChanged(nameof(AvailableDestinationCities));
					}
				}
				else
				{
					// ...to NOT NULL (different value).

					if (SelectedOriginCity == null)
					{
						// Update the origins list to exclude the new destination.
						AvailableOriginCities = _dataProvider.GetCities();
						AvailableOriginCities.Remove(newDestination);
						InvokePropertyChanged(nameof(AvailableOriginCities));
					}
					else if (newDestination == SelectedOriginCity)
					{
						// Swap the selected origin and destination.
						SelectedOriginCity = oldDestination;
					}
				}
			}
		}

		/// <summary>
		/// Converts truck power grade to equivalent amount of stars ★ (U+2605).
		/// </summary>
		/// <returns>Stars (★) if <see cref="SelectedTruckModelName"/> is not <see langword="null"/>; otherwise, <see langword="null"/>.</returns>
		private string ConvertTruckPowerGradeToStars()
		{
			if (SelectedTruckModelName == null)
			{
				return null;
			}

			return SelectedTruck.PowerGrade switch
			{
				PowerGrade.Low => "★",
				PowerGrade.Medium => "★★",
				PowerGrade.High => "★★★",
				_ => throw new ArgumentException("Unknown PowerGrade value", nameof(SelectedTruck.PowerGrade))
			};
		}

		/// <summary>
		/// Converts truck fuel efficiency grade to equivalent amount of stars ★ (U+2605).
		/// </summary>
		/// <returns>Stars (★) if <see cref="SelectedTruckModelName"/> is not <see langword="null"/>; otherwise, <see langword="null"/>.</returns>
		private string ConvertTruckFuelEfficiencyGradeToStars()
		{
			if (SelectedTruckModelName == null)
			{
				return null;
			}

			return SelectedTruck.FuelEfficiencyGrade switch
			{
				FuelEfficiencyGrade.Low => "★",
				FuelEfficiencyGrade.Medium => "★★",
				FuelEfficiencyGrade.High => "★★★",
				_ => throw new ArgumentException("Unknown FuelEfficiencyGrade value", nameof(SelectedTruck.FuelEfficiencyGrade))
			};
		}

		#endregion
	}
}
