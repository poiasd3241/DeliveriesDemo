using DeliveriesDemo.Core.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading;

namespace DeliveriesDemo.Core.ViewModels
{
	/// <summary>
	/// A view model for the list of active deliveries <see cref="DeliveryListItemViewModel"/>.
	/// </summary>
	public class DeliveryListViewModel : PropertyChangedNotifyBase
	{
		#region Public Properties

		/// <summary>
		/// List of active deliveries.
		/// </summary>
		public ObservableCollection<DeliveryListItemViewModel> Items { get; set; } = new ObservableCollection<DeliveryListItemViewModel>();

		/// <summary>
		/// Returns <see langword="true"/> if the number of active deliveries (<see cref="Items"/>) 
		/// has reached the defined limit; otherwise, <see langword="true"/>.
		/// </summary>
		public bool ActiveDeliveriesAmountLimitReached => Items?.Count == 10;

		// TODO: fix incorrect statements in summary regarding synchronization context/threads.
		/// <summary>
		/// Synchronization context of the thread <see cref="DeliveryListViewModel"/> was created on.<br/>
		/// Post to this to change public properties of <see cref="DeliveryListViewModel"/> from other threads. 
		/// </summary>
		public SynchronizationContext DeliveryListSyncContext { get; } = SynchronizationContext.Current;

		#endregion
	}
}
