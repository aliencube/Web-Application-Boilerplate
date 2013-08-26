using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Application.Web.UI.App_Start.BundleConfig), "RegisterBundles")]

namespace Application.Web.UI.App_Start
{
	public class BundleConfig
	{
		public static void RegisterBundles()
		{
			//	CSS bundles.
			BundleTable.Bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/main.css"));
			BundleTable.Bundles.Add(new StyleBundle("~/Content/placeholders").Include("~/Content/jquery.Placeholders.monkey.patch.css"));

			//	Javascript bundles.
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-2.6.2.js"));
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js",
			                                                                     "~/Scripts/jquery.validate.js",
			                                                                     "~/Scripts/jquery.validate.unobtrusive.js",
			                                                                     "~/Scripts/jquery.unobtrusive-ajax.js",
			                                                                     "~/Scripts/jquery.cookie.js"));
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/es5-shim").Include("~/Scripts/es5-shim.js"));
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/placeholders").Include("~/Scripts/Placeholders.js",
			                                                                           "~/Scripts/jquery.Placeholders.monkey.patch.js"));
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/linq").Include("~/Scripts/linq.js",
			                                                                   "~/Scripts/linq.jquery.js"));
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Scripts/plugins.js",
			                                                                 "~/Scripts/main.js"));
		}
	}
}