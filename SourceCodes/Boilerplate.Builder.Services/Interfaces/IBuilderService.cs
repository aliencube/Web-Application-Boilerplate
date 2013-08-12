using Boilerplate.Builder.ViewModels;

namespace Boilerplate.Builder.Services.Interfaces
{
	/// <summary>
	/// This provides interfaces to BuilderService class.
	/// </summary>
	public interface IBuilderService
	{
		/// <summary>
		/// Processes the requests.
		/// </summary>
		/// <param name="parameter">Parameter for boilerplate projects to apply.</param>
		void ProcessRequests(ConsoleParameter parameter);
	}
}