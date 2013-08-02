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

			//	Javascript bundles.
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-2.6.2.js"));
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Scripts/plugins.js", "~/Scripts/main.js"));
		}
	}
}
