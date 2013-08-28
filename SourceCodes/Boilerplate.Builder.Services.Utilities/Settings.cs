using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Boilerplate.Builder.Services.Utilities.Interfaces;

namespace Boilerplate.Builder.Services.Utilities
{
	/// <summary>
	/// This represents the application configuration settings entity.
	/// </summary>
	public class Settings : ISettings
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

		/// <summary>
		/// Gets the list of directories to exclude building boilerplates.
		/// </summary>
		public IList<string> DirectoriesToExclude
		{
			get
			{
				var value = ConfigurationManager.AppSettings["App.DirectoriesToExclude"];
				if (String.IsNullOrWhiteSpace(value))
					value = "App_Data,bin,Content,Localisation,Logs,obj,Scripts,Temp,Templates";

				var results = value.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries)
				                   .ToList();
				return results;
			}
		}

		/// <summary>
		/// Gets the list of file extensions to exclude building boilerplates.
		/// </summary>
		public IList<string> FileExtensionsToExclude
		{
			get
			{
				var value = ConfigurationManager.AppSettings["App.FileExtensionsToExclude"];
				if (String.IsNullOrWhiteSpace(value))
					value = "gif,ico,jpg,ldf,mdf,png,user,xml";

				var results = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
								   .ToList();
				return results;
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