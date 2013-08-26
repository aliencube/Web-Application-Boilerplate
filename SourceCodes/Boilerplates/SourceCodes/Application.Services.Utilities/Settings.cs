using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Application.Services.Utilities
{
	/// <summary>
	/// This represents the application configuration settings entity.
	/// </summary>
	public class Settings
	{
		#region Constants

		#endregion Constants

		#region Constructors

		/// <summary>
		///	Initialises a new instance of the Settings object as private.
		/// </summary>
		private Settings()
		{
		}

		#endregion Constructors

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

		private static Settings _instance;

		private HttpContextBase _context;

		/// <summary>
		/// Gets or sets the current HttpContext instance.
		/// </summary>
		public HttpContextBase Context
		{
			get
			{
				if (this._context == null)
					this._context = new HttpContextWrapper(HttpContext.Current);
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		/// <summary>
		/// Gets or sets the current locale.
		/// </summary>
		public string Locale
		{
			get
			{
				var context = this.Context;
				var cookie = context.Request.Cookies["locale"];
				if (cookie == null)
				{
					cookie = new HttpCookie("locale") { Value = "en", Expires = DateTime.Now.AddDays(1) };
					context.Response.Cookies.Add(cookie);
				}
				var locale = cookie.Value;
				if (String.IsNullOrWhiteSpace(locale))
				{
					locale = "en";
					cookie = new HttpCookie("locale") { Value = "en", Expires = DateTime.Now.AddDays(1) };
					context.Response.Cookies.Set(cookie);
				}
				return locale;
			}

			set
			{
				var context = this.Context;
				var cookie = context.Request.Cookies["locale"];
				if (cookie == null)
				{
					cookie = new HttpCookie("locale") { Value = value, Expires = DateTime.Now.AddDays(1) };
					context.Response.Cookies.Add(cookie);
				}
				else
				{
					cookie = new HttpCookie("locale") { Value = value, Expires = DateTime.Now.AddDays(1) };
					context.Response.Cookies.Set(cookie);
				}
			}
		}

		/// <summary>
		/// Gets the list of connection strings.
		/// </summary>
		public Dictionary<string, string> ConnectionStrings
		{
			get
			{
				if (this._connectionStrings == null || !this._connectionStrings.Any())
				{
					this._connectionStrings = new Dictionary<string, string>();
					foreach (ConnectionStringSettings setting in ConfigurationManager.ConnectionStrings)
						this._connectionStrings.Add(setting.Name, setting.ConnectionString);
				}
				return this._connectionStrings;
			}
		}

		private Dictionary<string, string> _connectionStrings;

		/// <summary>
		/// Gets the directory path where the localisation resources are placed.
		/// </summary>
		public string LocalisationPath
		{
			get
			{
				var value = ConfigurationManager.AppSettings["Web.LocalisationPath"];
				if (String.IsNullOrEmpty(value))
					value = "~/Localisation";
				value = this.GetFullPath(value);
				return value;
			}
		}

		/// <summary>
		/// Gets the directory path for email template.
		/// </summary>
		public string EmailTemplatePath
		{
			get
			{
				var value = ConfigurationManager.AppSettings["Web.EmailTemplatePath"];
				if (String.IsNullOrEmpty(value))
					value = "~/Templates/Emails";
				value = this.GetFullPath(value);
				return value;
			}
		}

		/// <summary>
		/// Gets the directory path for email template.
		/// </summary>
		public string ErrorTemplatePath
		{
			get
			{
				var value = ConfigurationManager.AppSettings["Web.ErrorTemplatePath"];
				if (String.IsNullOrEmpty(value))
					value = "~/Templates/Errors";
				value = this.GetFullPath(value);
				return value;
			}
		}

		/// <summary>
		/// Gets the directory path to store log files.
		/// </summary>
		public string LogPath
		{
			get
			{
				var value = ConfigurationManager.AppSettings["Web.LogPath"];
				if (String.IsNullOrEmpty(value))
					value = "~/Logs";
				value = this.GetFullPath(value);
				return value;
			}
		}

		/// <summary>
		/// Gets the directory path to store temporary files.
		/// </summary>
		public string TempPath
		{
			get
			{
				var value = ConfigurationManager.AppSettings["Web.TempPath"];
				if (String.IsNullOrEmpty(value))
					value = "~/Temp";
				value = this.GetFullPath(value);
				return value;
			}
		}

		/// <summary>
		/// Gets the list of years backwards from this year.
		/// </summary>
		public IList<int> Years
		{
			get
			{
				var years = new List<int>();
				for (var i = DateTime.Now.Year; i > DateTime.Now.Year - 10; i--)
					years.Add(i);
				return years;
			}
		}

		/// <summary>
		/// Gets the list of months based on locale.
		/// </summary>
		public IDictionary<int, string> Months
		{
			get
			{
				var months = new Dictionary<int, string>();
				var culture = CultureInfo.CreateSpecificCulture(this.Locale);
				var names = DateTimeFormatInfo.GetInstance(culture).MonthNames.Where(p => !String.IsNullOrWhiteSpace(p)).ToList();
				for (var i = 0; i < names.Count; i++)
					months.Add(i + 1, names[i]);
				return months;
			}
		}

		/// <summary>
		/// Gets the value that specifies whether the user is logged into the application or not.
		/// </summary>
		public bool IsAuthenticated
		{
			get
			{
				var context = this.Context;
				return context.User.Identity.IsAuthenticated;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the full path of the given application root path.
		/// </summary>
		/// <param name="path">Application root path.</param>
		/// <returns>Returns the full path of the given application root path.</returns>
		private string GetFullPath(string path)
		{
			var context = this.Context;
			if (context == null)
			{
				var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				if (!String.IsNullOrEmpty(directory))
					path = String.Format("{0}\\{1}",
										 directory.TrimEnd('/', '\\'),
										 path.Replace("~/", "").Replace("/", "\\").Trim('/', '\\'));
			}
			else
				path = context.Server.MapPath(path);
			return path;
		}

		/// <summary>
		/// Gets the locale from the filename provided.
		/// </summary>
		/// <param name="filename">Filename.</param>
		/// <returns>Returns the local derived from the filename provided.</returns>
		private string GetLocaleFromFilename(string filename)
		{
			if (String.IsNullOrWhiteSpace(filename))
				return null;

			var segments = filename.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
			if (segments.Length <= 2)
				return null;

			var locale = segments[segments.Length - 2];
			return locale;
		}

		#endregion Methods
	}
}