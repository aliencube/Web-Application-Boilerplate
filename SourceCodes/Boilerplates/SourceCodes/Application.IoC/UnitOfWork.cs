using Application.DataAccessFramework;
using Application.IoC.Interfaces;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace Application.IoC
{
	/// <summary>
	/// This represents the Unit of Work entity.
	/// </summary>
	public class UnitOfWork : IUnitOfWork
	{
		//http://msdn.microsoft.com/en-us/library/bb738523.aspx
		//http://stackoverflow.com/questions/815586/entity-framework-using-transactions-or-savechangesfalse-and-acceptallchanges

		#region Constructors

		/// <summary>
		/// Initialises a new instance of the UnitOfWork object.
		/// </summary>
		/// <param name="context">Database context.</param>
		public UnitOfWork(ApplicationDataContext context)
		{
			this._context = context;

			// In order to make calls that are overidden in the caching ef-wrapper, we need to use
			// transactions from the connection, rather than TransactionScope.
			// This results in our call e.g. to commit() being intercepted
			// by the wrapper so the cache can be adjusted.
			// This won't work with the dbcontext because it handles the connection itself, so we must use the underlying ObjectContext.
			// http://blogs.msdn.com/b/diego/archive/2012/01/26/exception-from-dbcontext-api-entityconnection-can-only-be-constructed-with-a-closed-dbconnection.aspx
			this._objectContext = ((IObjectContextAdapter)_context).ObjectContext;

			if (this._objectContext.Connection.State == ConnectionState.Open)
				return;

			this._objectContext.Connection.Open();
			this._transaction = this._objectContext.Connection.BeginTransaction();
		}

		#endregion Constructors

		#region Properties

		private readonly ApplicationDataContext _context;
		private readonly IDbTransaction _transaction;
		private readonly ObjectContext _objectContext;

		#endregion Properties

		#region Methods

		/// <summary>
		/// Saves the changes.
		/// </summary>
		public void SaveChanges()
		{
			this._context.SaveChanges();
		}

		/// <summary>
		/// Commits the changes.
		/// </summary>
		public void Commit()
		{
			this._context.SaveChanges();
			this._transaction.Commit();
		}

		/// <summary>
		/// Rollbacks the changes.
		/// </summary>
		public void Rollback()
		{
			this._transaction.Rollback();

			// http://blog.oneunicorn.com/2011/04/03/rejecting-changes-to-entities-in-ef-4-1/

			foreach (var entry in this._context.ChangeTracker.Entries())
			{
				switch (entry.State)
				{
					case EntityState.Modified:
						entry.State = EntityState.Unchanged;
						break;

					case EntityState.Added:
						entry.State = EntityState.Detached;
						break;

					case EntityState.Deleted:
						// Note - problem with deleted entities:
						// When an entity is deleted its relationships to other entities are severed.
						// This includes setting FKs to null for nullable FKs or marking the FKs as conceptually null (don't ask!)
						// if the FK property is not nullable. You'll need to reset the FK property values to
						// the values that they had previously in order to re-form the relationships.
						// This may include FK properties in other entities for relationships where the
						// deleted entity is the principal of the relationship–e.g. has the PK
						// rather than the FK. I know this is a pain–it would be great if it could be made easier in the future, but for now it is what it is.
						entry.State = EntityState.Unchanged;
						break;
				}
			}
		}

		/// <summary>
		/// Disposes the connection.
		/// </summary>
		public void Dispose()
		{
			if (this._objectContext.Connection.State == ConnectionState.Open)
				this._objectContext.Connection.Close();
		}

		#endregion Methods
	}
}