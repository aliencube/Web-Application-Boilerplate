namespace Application.Services.Interfaces
{
	/// <summary>
	/// This provides interfaces to the BaseService class.
	/// </summary>
	public interface IBaseService
	{
		/// <summary>
		/// Saves the string value into cookie with the given period.
		/// </summary>
		/// <param name="name">Cookie name.</param>
		/// <param name="value">Cookie value.</param>
		/// <param name="period">Period for expiry in days.</param>
		void SaveCookie(string name, string value, int period);

		/// <summary>
		/// Clears the cookie stored.
		/// </summary>
		/// <param name="name">Cookie name.</param>
		void ClearCookie(string name);

		/// <summary>
		/// Saves the object value into the server cache with the given period.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="value">Object to store.</param>
		/// <param name="period">Period for expiry in minutes.</param>
		void SaveCache(string key, object value, int period);

		/// <summary>
		/// Clears the cache from the server.
		/// </summary>
		/// <param name="key">Cache key.</param>
		void ClearCache(string key);
	}
}