using Boilerplate.Builder.Services;
using Boilerplate.Builder.Services.Interfaces;
using Boilerplate.Builder.Services.Utilities;
using Boilerplate.Builder.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Boilerplate.Builder.Console
{
	/// <summary>
	/// This represents the main entry point of the program entity.
	/// </summary>
	public class Program
	{
		private static readonly ILog _log = LogManager.GetLogger("WebApplication Boilerplate Builder Console");

		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <param name="args">List of arguments.</param>
		public static void Main(string[] args)
		{
			ShowSplash();
			try
			{
				ProcessRequests(args);
				System.Console.WriteLine();
			}
			catch (Exception ex)
			{
				if (_log.IsErrorEnabled)
				{
					var sb = new StringBuilder();
					sb.AppendLine("ERROR:");
					sb.AppendLine();
					sb.AppendLine(ex.Message);
					sb.AppendLine();
					sb.AppendLine(ex.StackTrace);

					_log.Error(sb.ToString());
				}

				System.Console.WriteLine();
				ShowUsage();
			}
		}

		/// <summary>
		/// Shows the splash message.
		/// </summary>
		private static void ShowSplash()
		{
			var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
			var sb = new StringBuilder();
			sb.AppendLine(String.Format("{0} v{1}", fvi.ProductName, fvi.FileVersion));
			sb.AppendLine("------------------------------");

			System.Console.WriteLine(sb.ToString());
		}

		/// <summary>
		/// Shows the usage of the app message.
		/// </summary>
		private static void ShowUsage()
		{
			var sb = new StringBuilder();
			sb.AppendLine("Usage:");
			sb.AppendLine("  WebApplicationBoilerplateBuilderConsole.exe /n:[Default Namespace]");
			sb.AppendLine();
			sb.AppendLine("Parameter:");
			sb.AppendLine("  /ns:[Namespace]  Sets the default namespace of the boilerplate.");
			sb.AppendLine("                   If empty, the default namespace will be \"Application\".");
			sb.AppendLine();

			System.Console.WriteLine(sb.ToString());
		}

		/// <summary>
		/// Processes the requests based on the argument provided.
		/// </summary>
		/// <param name="args">List of arguments.</param>
		private static void ProcessRequests(IList<string> args)
		{
			if (args == null || !args.Any())
				throw new ArgumentException("No argument found");

			var param = GetParameter(args);

			var settings = Settings.Instance;

			if (_log.IsInfoEnabled)
				_log.Info("Build started...");

			IBuilderService service = new BuilderService(settings);
			service.ProcessRequests(param);

			if (_log.IsInfoEnabled)
				_log.Info("Build completed");
		}

		/// <summary>
		/// Gets the parameter for boilerplate projects to apply.
		/// </summary>
		/// <param name="args">List of arguments.</param>
		/// <returns>Returns the parameter for boilerplate projects to apply.</returns>
		private static ConsoleParameter GetParameter(IList<string> args)
		{
			var param = new ConsoleParameter();

			var ns = args[0];
			if (!ns.ToLower().StartsWith("/ns:"))
				throw new ArgumentException("Invalid arguments");

			param.Namespace = Regex.Replace(ns.Trim(),
			                                "^/ns:(.*)$",
			                                "$1", RegexOptions.Compiled | RegexOptions.IgnoreCase)
			                       .Trim();
			return param;
		}

		#endregion Methods
	}
}