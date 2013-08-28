using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Boilerplate.Builder.Services.Interfaces;
using Boilerplate.Builder.Services.Utilities.Interfaces;
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
		public BuilderService(ISettings settings)
		{
			this._settings = settings;
			this._documentsPath = this.GetBoilerplatesDirectoryPath("Documents");
			this._librariesPath = this.GetBoilerplatesDirectoryPath("Libraries");
			this._sourceCodesPath = this.GetBoilerplatesDirectoryPath("SourceCodes");
		}

		#endregion Constructors

		#region Properties

		private readonly ISettings _settings;
		private readonly string _sourceCodesPath;
		private readonly string _documentsPath;
		private readonly string _librariesPath;

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the full directory path for the boilerplates.
		/// </summary>
		/// <param name="directoryName">Directory name</param>
		/// <returns>Returns the full directory path for the boilerplates.</returns>
		private string GetBoilerplatesDirectoryPath(string directoryName)
		{
			var path = String.Format(@"{0}\{1}", this._settings.BoilerplatePath, directoryName);
			return path;
		}

		/// <summary>
		/// Processes the requests.
		/// </summary>
		/// <param name="parameter">Parameter for boilerplate projects to apply.</param>
		public void ProcessRequests(ConsoleParameter parameter)
		{
			var ns = parameter.Namespace;
			this.ChangeNamespaceOnSolution(ns);
			this.ChangeNamespaceOnProjects(ns);
			this.ChangeNamespaceOnPackages(ns);
			this.ChangeNamespaceOnDirectories(ns);
		}

		/// <summary>
		/// Changes the contents of the solution file and its name with the given namespace.
		/// </summary>
		/// <param name="ns">Namespace to be applied.</param>
		private void ChangeNamespaceOnSolution(string ns)
		{
			var sln = Directory.GetFiles(this._sourceCodesPath).Single(p => p.EndsWith(".sln"));

			//	Changes namespace within the solution file.
			string converted;
			using (var reader = new StreamReader(sln))
			{
				var content = reader.ReadToEnd();
				converted = content.Replace("Application.", ns + ".");
			}
			using (var writer = new StreamWriter(sln, false, Encoding.UTF8))
			{
				writer.Write(converted);
			}
			//	Renames the solution file.
			File.Move(sln, sln.Replace("Application.", ns + "."));
		}

		/// <summary>
		/// Changes the contents of each project and its name with the given namespace.
		/// </summary>
		/// <param name="ns">Namespace to be applied.</param>
		private void ChangeNamespaceOnProjects(string ns)
		{
			var directories = this.GetProjectDirectories();
			var files = new List<string>();
			foreach (var directory in directories)
			{
				var fs = this.GetFilesFromDirectory(directory);
				files.AddRange(fs);

				var subdirectories = Directory.GetDirectories(directory)
				                              .Where(p => this._settings
				                                              .DirectoriesToExclude
				                                              .Contains(p.Split(new string[] {"\\"},
				                                                                StringSplitOptions.RemoveEmptyEntries)
				                                                         .Last()));
				foreach (var subdirectory in subdirectories)
				{
					fs = this.GetFilesFromDirectory(subdirectory);
					files.AddRange(fs);
				}
			}
		}

		/// <summary>
		/// Gets the list of directories within the project recursively.
		/// </summary>
		/// <param name="directory">Parent directory.</param>
		/// <returns>Returns the list of directories within the project recursively.</returns>
		private IList<string> GetDirectoriesInProject(string directory)
		{
			var subdirectories = Directory.GetDirectories(directory)
			                              .Where(p => this._settings
			                                              .DirectoriesToExclude
			                                              .Contains(p.Split(new string[] {"\\"},
			                                                                StringSplitOptions.RemoveEmptyEntries)
			                                                         .Last()))
			                              .ToList();
			var directories = subdirectories;
			foreach (var subdirectory in subdirectories)
				directories.AddRange(this.GetDirectoriesInProject(subdirectory));

			return directories;
		}

		/// <summary>
		/// Gets the list of files from the given directory path, except files to be excluded.
		/// </summary>
		/// <param name="directory">Directory path to get the list of files.</param>
		/// <returns>Returns the list of files from the given directory path, except files to be excluded.</returns>
		private IList<string> GetFilesFromDirectory(string directory)
		{
			var files = Directory.GetFiles(directory, "*.*")
			                     .Where(p => !this._settings
			                                      .FileExtensionsToExclude
			                                      .Contains(Path.GetExtension(p)))
			                     .ToList();
			return files;
		}

		/// <summary>
		/// Changes the path of the package with the given namespace.
		/// </summary>
		/// <param name="ns">Namespace to be applied.</param>
		private void ChangeNamespaceOnPackages(string ns)
		{
		}

		/// <summary>
		/// Changes directories containing projects with the given namespace.
		/// </summary>
		/// <param name="ns">Namespace to be applied.</param>
		private void ChangeNamespaceOnDirectories(string ns)
		{
			var directories = this.GetProjectDirectories();
			foreach (var directory in directories)
			{
				Directory.Move(directory, directory.Replace("Application.", ns + "."));
			}
		}

		/// <summary>
		/// Gets the list of project directories to apply namespace given.
		/// </summary>
		/// <returns>Returns the list of project directories to apply namespace given.</returns>
		private IEnumerable<string> GetProjectDirectories()
		{
			var directories = Directory.GetDirectories(this._sourceCodesPath)
									   .Where(p => p.StartsWith("Application."))
									   .ToList();
			return directories;
		}

		#endregion Methods
	}
}