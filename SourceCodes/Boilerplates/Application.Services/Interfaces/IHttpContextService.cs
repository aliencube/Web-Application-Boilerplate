using System.Web;

namespace Application.Services.Interfaces
{
	/// <summary>
	/// This provides interfaces to the HttpContextService class.
	/// </summary>
	public interface IHttpContextService
	{
		/// <summary>
		/// Gets or sets the current HttpContext instance.
		/// </summary>
		HttpContextBase HttpContext { get; set; }
	}
}