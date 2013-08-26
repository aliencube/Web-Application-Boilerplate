using System.Web.Mvc;

namespace Application.Web.UI.Controllers
{
	public class HomeController : Controller
	{
		//
		// GET: /Home/

		public ActionResult Index()
		{
			return View();
		}
	}
}