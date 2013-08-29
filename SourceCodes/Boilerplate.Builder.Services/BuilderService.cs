using Boilerplate.Builder.Services.Interfaces;
using Boilerplate.Builder.Services.Utilities.Interfaces;
using Boilerplate.Builder.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
			var path = String.Format(@"{0}\{1}",
			                         this._settings.BoilerplatePath,
			                         directoryName);
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
			var file = Directory.GetFiles(this._sourceCodesPath)
			                    .Single(p => p.EndsWith(".sln"));

			this.ChangeNamespaceOnFile(ns, file);
		}

		/// <summary>
		/// Changes the contents of each project and its name with the given namespace.
		/// </summary>
		/// <param name="ns">Namespace to be applied.</param>
		private void ChangeNamespaceOnProjects(string ns)
		{
			var directories = this.GetProjectDirectories();
			foreach (var directory in directories)
			{
				var files = new List<string>(this.GetFilesFromDirectory(directory));

				var subdirectories = this.GetSubdirectories(directory);
				foreach (var subdirectory in subdirectories)
					files.AddRange(this.GetFilesFromDirectory(subdirectory));

				//	Changes namespace on each file.
				this.ChangeNamespaceOnFiles(ns, files);
			}
		}

		/// <summary>
		/// Changes the path of the package with the given namespace.
		/// </summary>
		/// <param name="ns">Namespace to be applied.</param>
		private void ChangeNamespaceOnPackages(string ns)
		{
			var file = Directory.GetFiles(this._sourceCodesPath + @"\packages")
			                    .Single(p => p.EndsWith(".config"));

			this.ChangeNamespaceOnFile(ns, file);
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
				Directory.Move(directory,
				               directory.Replace("Application.", String.Format("{0}.", ns)));
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

		/// <summary>
		/// Gets the list of subdirectories under the given directory.
		/// </summary>
		/// <param name="directory">Directory path.</param>
		/// <param name="recursive">Value that specifies whether to search subdirectories recursively or not.</param>
		/// <returns>Returns the list of subdirectories under the given directory.</returns>
		public IList<string> GetSubdirectories(string directory, bool recursive = true)
		{
			var subdirectories = Directory.GetDirectories(directory)
										  .Where(p => !this._settings
														   .DirectoriesToExclude
														   .Contains(p.Split(new string[] { "\\" },
																			 StringSplitOptions.RemoveEmptyEntries)
																	  .Last()))
										  .ToList();
			if (!recursive)
				return subdirectories;

			var directories = new List<string>(subdirectories);
			foreach (var subdirectory in subdirectories.Where(p => Directory.GetDirectories(p).Any()))
				directories.AddRange(this.GetSubdirectories(subdirectory, recursive));

			return directories;
		}

		/// <summary>
		/// Gets the list of files from the given directory path, except files to be excluded.
		/// </summary>
		/// <param name="directory">Directory path to get the list of files.</param>
		/// <returns>Returns the list of files from the given directory path, except files to be excluded.</returns>
		public IList<string> GetFilesFromDirectory(string directory)
		{
			var files = Directory.GetFiles(directory, "*.*")
								 .Where(p => !this._settings
												  .FileExtensionsToExclude
												  .Contains(Path.GetExtension(p)))
								 .ToList();
			return files;
		}

		/// <summary>
		/// Changes namespace on a file.
		/// </summary>
		/// <param name="ns">Namespace to be applied.</param>
		/// <param name="file">Filepath.</param>
		private void ChangeNamespaceOnFile(string ns, string file)
		{
			this.ChangeNamespaceOnFiles(ns, new List<string>() { file });
		}

		/// <summary>
		/// Changes namespace on list of files.
		/// </summary>
		/// <param name="ns">Namespace to be applied.</param>
		/// <param name="files">List of filepaths.</param>
		private void ChangeNamespaceOnFiles(string ns, IEnumerable<string> files)
		{
			foreach (var file in files)
			{
				//	Changes namespaces on contents of the file.
				string converted;
				using (var reader = new StreamReader(file))
				{
					var content = reader.ReadToEnd();
					converted = content.Replace("Application.", String.Format("{0}.", ns));
				}
				using (var writer = new StreamWriter(file, false, Encoding.UTF8))
				{
					writer.Write(converted);
				}

				//	Renames if the filename itself contains namespace.
				var filename = Path.GetFileName(file);
				if (filename == null || !filename.Contains("Application."))
					continue;

				var path = Path.GetDirectoryName(file);
				File.Move(file, String.Format("{0}\\{1}",
				                              path,
				                              filename.Replace("Application.", String.Format("{0}.", ns))));
			}
		}

		#endregion Methods
	}
}