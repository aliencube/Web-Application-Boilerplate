using System.IO;
using System.Linq;
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
			var ns = parameter.Namespace;
			this.ChangeNamespaceOnDirectories(ns);
			this.ChangeNamespaceOnSolution(ns);
			this.ChangeNamespaceOnProjects(ns);
			this.ChangeNamespaceOnPackages(ns);
		}

		private void ChangeNamespaceOnDirectories(string ns)
		{
			var directories = Directory.GetDirectories(this._settings.BoilerplatePath)
			                           .Where(p => p.StartsWith("Application."))
			                           .ToList();
			foreach (var directory in directories)
			{
				Directory.Move(directory, directory.Replace("Application.", ns + "."));
			}
		}

		private void ChangeNamespaceOnSolution(string ns)
		{
			var sln = Directory.GetFiles(this._settings.BoilerplatePath).Single(p => p.EndsWith(".sln"));
			File.Move(sln, sln.Replace("Application.", ns + "."));
		}

		private void ChangeNamespaceOnProjects(string ns)
		{
		}

		private void ChangeNamespaceOnPackages(string ns)
		{
		}

		#endregion Methods
	}
}