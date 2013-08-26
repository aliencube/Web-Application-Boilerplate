using Application.Services.Interfaces;
using System.Web;

namespace Application.Services
{
	/// <summary>
	/// This represents the HttpContext entity.
	/// </summary>
	public class HttpContextService : IHttpContextService
	{
		private HttpContextBase _httpContext;

		/// <summary>
		/// Gets or sets the current HttpContext instance.
		/// </summary>
		public HttpContextBase HttpContext
		{
			get
			{
				if (this._httpContext == null)
					this._httpContext = new HttpContextWrapper(System.Web.HttpContext.Current);
				return this._httpContext;
			}
			set { this._httpContext = value; }
		}
	}
}