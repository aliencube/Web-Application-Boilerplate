using Application.DataAccessFramework;
using Application.IoC.Interfaces;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Mvc;
using Unity.Mvc3;

namespace Application.IoC
{
	public static class Bootstrapper
	{
		public static void Initialise()
		{
			var container = BuildUnityContainer();

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
			GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
		}

		private static IUnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			// register all your components with the container here
			// e.g. container.RegisterType<ITestService, TestService>();

			//	Registers the database context.
			container.RegisterType<IApplicationDataContext, ApplicationDataContext>(new HierarchicalLifetimeManager());
			container.RegisterType<IUnitOfWorkManager, UnitOfWorkManager>(new HierarchicalLifetimeManager());

			//	Registers the services.
			container.RegisterType<IAccountService, AccountService>(new HierarchicalLifetimeManager());

			//	Registers the repositories.
			container.RegisterType<ICustomerRepository, CustomerRepository>(new HierarchicalLifetimeManager());

			return container;
		}
	}
}