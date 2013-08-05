using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;
using NUnit.Framework;

namespace Application.Web.UI.Tests
{
	[TestFixture]
	public class HomeControllerTest
	{
		private HttpContextBase _context;

		#region SetUp / TearDown

		[SetUp]
		public void Init()
		{
			RouteTable.Routes.Clear();
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			//	Mocking HttpContextBase.
			//	http://stackoverflow.com/questions/9624242/setting-the-httpcontext-current-session-in-unit-test
			//	http://blogs.planetcloud.co.uk/mygreatdiscovery/post/ASPNET-MVC-Outbound-Url-tests-with-NSubstitute.aspx
			//	http://geekswithblogs.net/Nettuce/archive/2011/02/15/fluent-mvc-route-testing-helper.aspx

			this._context = Substitute.For<HttpContextBase>();
			var request = Substitute.For<HttpRequestBase>();
			//request.Url.Returns(new Uri("http://aliencube.org"));
			//request.UrlReferrer.Returns(new Uri("http://aliencube.org"));
			//request.RequestContext = new RequestContext(this._context, new RouteData());

			var response = Substitute.For<HttpResponseBase>();
			response.ApplyAppPathModifier(Arg.Any<string>())
			            .Returns(p => p.Arg<string>());

			var session = Substitute.For<HttpSessionStateBase>();
			var server = Substitute.For<HttpServerUtilityBase>();
			var identity = Substitute.For<IIdentity>();
			identity.Name.Returns(ConfigurationManager.AppSettings["Mock.Identity.Name"]);
			identity.IsAuthenticated.Returns(true);

			var user = Substitute.For<IPrincipal>();
			user.Identity.Returns(identity);

			var requestContext = new RequestContext(this._context, new RouteData());
			this._context.Request.Returns(request);
			this._context.Response.Returns(response);
			this._context.Session.Returns(session);
			this._context.Server.Returns(server);
			this._context.User.Returns(user);
		}

		[TearDown]
		public void Dispose()
		{
		}

		#endregion SetUp / TearDown

		#region Tests

		[Test]
		public void Test()
		{
		}

		#endregion Tests
	}
}