using Boilerplate.Builder.Services.Events;
using Boilerplate.Builder.ViewModels;
using System;

namespace Boilerplate.Builder.Services.Interfaces
{
	/// <summary>
	/// This provides interfaces to BuilderService class.
	/// </summary>
	public interface IBuilderService
	{
		#region Events

		/// <summary>
		/// Occurs when status changed event is raised.
		/// </summary>
		event EventHandler<StatusChangedEventArgs> StatusChanged;

		/// <summary>
		/// Occurs when exception thrown event is raised.
		/// </summary>
		event EventHandler<ExceptionThrownEventArgs> ExceptionThrown;

		#endregion Events

		#region Methods

		/// <summary>
		/// Processes the requests.
		/// </summary>
		/// <param name="parameter">Parameter for boilerplate projects to apply.</param>
		void ProcessRequests(ConsoleParameter parameter);

		#endregion Methods
	}
}