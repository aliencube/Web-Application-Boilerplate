using Application.DataAccessFramework;

namespace Application.Services.Interfaces
{
	/// <summary>
	/// This provides interfaces to the AccountService class.
	/// </summary>
	public interface IAccountService : IBaseService
	{
		#region Properties

		/// <summary>
		/// Gets the value that specifies whether the user is logged into the application or not.
		/// </summary>
		bool IsAuthenticated { get; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the customer.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>Returns the customer.</returns>
		Customer GetCustomer(int customerId);

		#endregion Methods
	}
}