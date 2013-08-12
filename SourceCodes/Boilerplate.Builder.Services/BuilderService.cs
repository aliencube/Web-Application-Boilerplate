using Boilerplate.Builder.Services.Interfaces;
using Boilerplate.Builder.Services.Utilities;
using Boilerplate.Builder.ViewModels;

namespace Boilerplate.Builder.Services
{
	/// <summary>
	/// This represents the builder service entity.
	/// </summary>
	public class BuilderService : IBuilderService
	{
		#region Constructors

		/// <summary>
		/// Initialises a new instance of the BuilderService object.
		/// </summary>
		/// <param name="settings">Configuration settings instance.</param>
		public BuilderService(Settings settings)
		{
			this._settings = settings;
		}

		#endregion Constructors

		#region Properties

		private Settings _settings;

		#endregion Properties

		#region Methods

		/// <summary>
		/// Processes the requests.
		/// </summary>
		/// <param name="parameter">Parameter for boilerplate projects to apply.</param>
		public void ProcessRequests(ConsoleParameter parameter)
		{
			throw new System.NotImplementedException();
		}

		#endregion Methods
	}
}