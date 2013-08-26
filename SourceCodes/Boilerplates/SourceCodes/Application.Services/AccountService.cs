using Application.DataAccessFramework;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;

namespace Application.Services
{
	/// <summary>
	/// This represents the account service entity.
	/// </summary>
	public class AccountService : BaseService, IAccountService
	{
		#region Constructors

		/// <summary>
		/// Initialises a new instance of the AccountService object.
		/// </summary>
		/// <param name="context">Data context instance.</param>
		/// <param name="contextService">HttpContext service instance.</param>
		/// <param name="customerRepository">Customer repository instance.</param>
		public AccountService(IApplicationDataContext context, IHttpContextService contextService, ICustomerRepository customerRepository)
			: base(contextService)
		{
			this._context = context as ApplicationDataContext;
			this._customerRepository = customerRepository;
		}

		#endregion Constructors

		#region Properties

		private readonly ApplicationDataContext _context;
		private readonly ICustomerRepository _customerRepository;

		/// <summary>
		/// Gets the value that specifies whether the user is logged into the application or not.
		/// </summary>
		public bool IsAuthenticated
		{
			get { return this.Settings.IsAuthenticated; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the customer.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>Returns the customer.</returns>
		public Customer GetCustomer(int customerId)
		{
			return this._customerRepository.Get<Customer>(customerId);
		}

		#endregion Methods
	}
}