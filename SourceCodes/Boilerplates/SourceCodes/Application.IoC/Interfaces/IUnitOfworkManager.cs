using System;

namespace Application.IoC.Interfaces
{
	/// <summary>
	/// This provides interfaces to the UnitOfWorkManager class.
	/// </summary>
	public interface IUnitOfWorkManager : IDisposable
	{
		/// <summary>
		/// Provides a new instance of a unit of work. This wrapping in the manager class helps keep concerns separated.
		/// </summary>
		/// <returns>Returns the UnitOfWork object created.</returns>
		IUnitOfWork NewUnitOfWork();
	}
}