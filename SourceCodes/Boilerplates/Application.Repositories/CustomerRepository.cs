using Application.DataAccessFramework;
using Application.DataAccessFramework.Exceptions;
using Application.Repositories.Interfaces;
using System;
using System.Data;
using System.Linq;

namespace Application.Repositories
{
	/// <summary>
	/// This represents the customer repository.
	/// </summary>
	public class CustomerRepository : BaseRepository, ICustomerRepository
	{
		#region Constructors

		/// <summary>
		/// Initialises a new instance of the CustomerRepository object.
		/// </summary>
		/// <param name="context"></param>
		public CustomerRepository(IApplicationDataContext context)
		{
			this._context = context as ApplicationDataContext;
		}

		#endregion Constructors

		#region Properties

		private readonly ApplicationDataContext _context;

		#endregion Properties

		#region Methods

		/// <summary>
		/// Adds a new Customer object to the repository.
		/// </summary>
		/// <param name="entity">Customer object.</param>
		/// <returns>Returns the Customer object added.</returns>
		public override T Add<T>(T entity)
		{
			this._context.Customers.Add(entity as Customer);
			return entity;
		}

		/// <summary>
		/// Gets the Customer object by entityId.
		/// </summary>
		/// <param name="entityId">Unique Id.</param>
		/// <returns>Returns the Customer object.</returns>
		public override T Get<T>(int entityId)
		{
			var item = this._context
						   .Customers
						   .SingleOrDefault(p => p.CustomerID == entityId);
			return (T)Convert.ChangeType(item, typeof(T));
		}

		/// <summary>
		/// Updates the Customer object.
		/// </summary>
		/// <param name="entity">Customer object.</param>
		public override void Update<T>(T entity)
		{
			var customer = entity as Customer;
			if (this._context.Customers.Local.Select(p => p.CustomerID == customer.CustomerID).Any())
				throw new DataContextAlreadyExistException(String.Format("The {0} object already exists in the context. Update doesn't need to be called. Save occurs on commit.", typeof(T).Name));
			this._context.Entry(customer).State = EntityState.Modified;
		}

		/// <summary>
		/// Deletes the Customer object from the repository.
		/// </summary>
		/// <param name="entity">Customer object.</param>
		public override void Delete<T>(T entity)
		{
			this._context.Customers.Remove(entity as Customer);
		}

		#endregion Methods
	}
}