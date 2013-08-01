using System.Data.Entity;
using Application.DataAccessFramework;
using Application.IoC.Interfaces;

namespace Application.IoC
{
	// http://blogs.msdn.com/b/adonet/archive/2011/01/27/using-dbcontext-in-ef-feature-ctp5-part-1-introduction-and-model.aspx
	// http://blogs.msdn.com/b/adonet/archive/2011/03/15/ef-4-1-code-first-walkthrough.aspx
	/// <summary>
	/// This represents the Unit of Work manager entity.
	/// </summary>
	public class UnitOfWorkManager : IUnitOfWorkManager
	{
		#region Constructors
		/// <summary>
		/// Initialises a new instance of the UnitOfWorkManager object.
		/// </summary>
		/// <param name="context">Database context.</param>
		public UnitOfWorkManager(ApplicationDataContext context)
		{
			// http://stackoverflow.com/questions/3552000/entity-framework-code-only-error-the-model-backing-the-context-has-changed-sinc
			Database.SetInitializer<ApplicationDataContext>(null);

			this._context = context as ApplicationDataContext;
		}
		#endregion

		#region Properties
		private readonly ApplicationDataContext _context;
		private bool _disposed;

		#endregion
		/// <summary>
		/// Provides a new instance of a unit of work. This wrapping in the manager class helps keep concerns separated.
		/// </summary>
		/// <returns>Returns the UnitOfWork object created.</returns>
		public IUnitOfWork NewUnitOfWork()
		{
			return new UnitOfWork(this._context);
		}

		/// <summary>
		/// Disposes the session already opened. This will be called when the injected UnitOfWorkManager is disposed at the end of a request.
		/// </summary>
		public void Dispose()
		{
			if (this._disposed)
				return;

			this._context.Dispose();
			this._disposed = true;
		}
	}
}
