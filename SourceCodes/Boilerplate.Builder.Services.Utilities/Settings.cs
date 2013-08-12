using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Boilerplate.Builder.Services.Utilities
{
	/// <summary>
	/// This represents the application configuration settings entity.
	/// </summary>
	public class Settings
	{
		#region Constructors

		/// <summary>
		///	Initialises a new instance of the Settings object as private.
		/// </summary>
		private Settings()
		{
		}

		#endregion Constructors

		#region Fields

		private static Settings _instance;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets the instance of the settings object.
		/// </summary>
		public static Settings Instance
		{
			get
			{
				if (_instance == null)
					_instance = new Settings();
				return _instance;
			}
		}

		/// <summary>
		/// Gets the directory path where boilerplate projects solution file is.
		/// </summary>
		public string BoilerplatePath
		{
			get
			{
				var value = ConfigurationManager.AppSettings["App.BoilerplatePath"];
				var path = GetFullPath(value);
				return path;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the full path of the given application root path.
		/// </summary>
		/// <param name="path">Application root path.</param>
		/// <returns>Returns the full path of the given application root path.</returns>
		private static string GetFullPath(string path)
		{
			//	Returns NULL, if the given path is NULL or empty.
			if (String.IsNullOrWhiteSpace(path))
				return null;

			//	Returns the original path, if the given path is UNC.
			if (Regex.IsMatch(path, @"[a-z]:\\", RegexOptions.Compiled | RegexOptions.IgnoreCase))
				return path;

			//	Sets the given path to be application root.
			if (!path.StartsWith("/"))
				path = "/" + path;
			if (!path.StartsWith("~"))
				path = "~" + path;

			var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (!String.IsNullOrEmpty(directory))
				path = String.Format("{0}\\{1}",
				                     directory.TrimEnd('/', '\\'),
				                     path.Replace("~/", "").Replace("/", "\\").Trim('/', '\\'));
			return path;
		}

		#endregion Methods
	}
}