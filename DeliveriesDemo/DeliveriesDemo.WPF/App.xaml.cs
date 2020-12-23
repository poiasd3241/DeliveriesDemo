using DeliveriesDemo.Core;
using DeliveriesDemo.Core.ViewModels;
using DeliveriesDemo.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace DeliveriesDemo.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// Setup DI.
			IServiceProvider serviceProvider = DI.Setup();

			// Prepare ShellView.
			Window window = new ShellView
			{
				DataContext = serviceProvider.GetService<ShellViewModel>()
			};

			// Show ShellView.
			window.Show();
		}
	}
}
