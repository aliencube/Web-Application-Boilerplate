using System;

namespace Application.DataAccessFramework
{
	/// <summary>
	/// This provides interfaces to the AmeAuthenticationDataContext class.
	/// </summary>
	public interface IApplicationDataContext : IDisposable
	{
	}

	/// <summary>
	/// This represents the database context containing database object mapped entities.
	/// </summary>
	public partial class ApplicationDataContext : IApplicationDataContext
	{
	}
}
