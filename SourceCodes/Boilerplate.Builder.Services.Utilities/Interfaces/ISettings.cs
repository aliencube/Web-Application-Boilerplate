using System.Collections.Generic;

namespace Boilerplate.Builder.Services.Utilities.Interfaces
{
	/// <summary>
	/// This provides interfaces to the Settings class.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Gets the directory path where boilerplate projects solution file is.
		/// </summary>
		string BoilerplatePath { get; }

		/// <summary>
		/// Gets the list of directories to exclude building boilerplates.
		/// </summary>
		IList<string> DirectoriesToExclude { get; }

		/// <summary>
		/// Gets the list of file extensions to exclude building boilerplates.
		/// </summary>
		IList<string> FileExtensionsToExclude { get; }
	}
}