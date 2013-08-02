using Application.Services.Interfaces;
using Application.Services.Utilities;
using System;
using System.Web;
using System.Web.Caching;

namespace Application.Services
{
	/// <summary>
	/// This represents the BaseService entity inherited to all service classes.
	/// </summary>
	public abstract class BaseService : IBaseService
	{
		#region Constructors

		/// <summary>
		/// Initialises a new instance of the BaseService object.
		/// </summary>
		/// <param name="contextService">HttpContext service instance.</param>
		protected BaseService(IHttpContextService contextService)
		{
			this.Settings = Settings.Instance;
			this.Settings.Context = contextService.HttpContext;
			this.Context = contextService.HttpContext;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Gets the configuration settings instance.
		/// </summary>
		protected Settings Settings { get; private set; }

		/// <summary>
		/// Gets the current HttpContext instance.
		/// </summary>
		protected HttpContext Context { get; private set; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Saves the string value into cookie with the given period.
		/// </summary>
		/// <param name="name">Cookie name.</param>
		/// <param name="value">Cookie value.</param>
		/// <param name="period">Period for expiry in days.</param>
		public void SaveCookie(string name, string value, int period = 1)
		{
			var context = this.Context;
			var cookie = context.Request.Cookies[name];
			if (cookie != null)
			{
				context.Response.Cookies.Set(new HttpCookie(name) { Expires = DateTime.Now.AddDays(-1) });
				context.Response.Cookies.Remove(name);
			}
			cookie = new HttpCookie(name) { Value = HttpUtility.UrlEncode(value), Expires = DateTime.Now.AddDays(period) };
			context.Response.Cookies.Add(cookie);
		}

		/// <summary>
		/// Clears the cookie stored.
		/// </summary>
		/// <param name="name">Cookie name.</param>
		public void ClearCookie(string name)
		{
			this.SaveCookie(name, String.Empty, -1);
		}

		/// <summary>
		/// Saves the object value into the server cache with the given period.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="value">Object to store.</param>
		/// <param name="period">Period for expiry in minutes.</param>
		public void SaveCache(string key, object value, int period)
		{
			var context = this.Context;
			var cache = context.Cache[key];
			if (cache != null)
				context.Cache.Remove(key);
			context.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(period), Cache.NoSlidingExpiration);
		}

		/// <summary>
		/// Clears the cache from the server.
		/// </summary>
		/// <param name="key">Cache key.</param>
		public void ClearCache(string key)
		{
			var context = this.Context;
			var cache = context.Cache[key];
			if (cache != null)
				context.Cache.Remove(key);
		}

		#endregion Methods
	}
}