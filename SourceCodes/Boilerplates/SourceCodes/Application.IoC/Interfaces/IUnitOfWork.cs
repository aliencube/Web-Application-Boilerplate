using System;

namespace Application.IoC.Interfaces
{
	/// <summary>
	/// This provides interfaces to the UnitOfWork class.
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
		/// <summary>
		/// Saves the changes.
		/// </summary>
		void SaveChanges();

		/// <summary>
		/// Commits the changes.
		/// </summary>
		void Commit();

		/// <summary>
		/// Rollbacks the changes.
		/// </summary>
		void Rollback();
	}
}