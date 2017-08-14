using System.Web.Optimization;

namespace Locs4Youth
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                         "~/Content/js/jquery.validate.min.js",
                         "~/Content/js/jquery.validate.unobtrusive.min.js",
                         "~/Content/js/jquery.validate-vsdoc.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/js/bootstrap.min.js",
                      "~/Content/js/respond.min.js"));

            bundles.Add(new StyleBundle("~/Content/css/bundle").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/library.css",
                      "~/Content/css/styles.css"));
        }
    }
}
