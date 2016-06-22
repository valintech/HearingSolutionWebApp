using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace HearingSolutionWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/common.js",
                "~/Scripts/app/app.js",
                "~/Scripts/app/appRouteProvider.js",
                "~/Scripts/app/filters.js",
                "~/Scripts/app/KioskConnectionFactory.js",
                "~/Scripts/app/UserConnectionFactory.js",
                "~/Scripts/app/kioskRowController.js",
                "~/Scripts/app/KiosksController.js",
                "~/Scripts/app/MainController.js",
                "~/Scripts/app/OverviewController.js",
                "~/Scripts/app/TransactionsController.js",
                "~/Scripts/app/UsersController.js"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/respond.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/ui-bootstrap-tpls-1.3.3.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css",
                "~/Content/xeditable.min.css"
                 ));


            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-route.min.js",
                "~/Scripts/jquery.signalR-2.2.0.min.js",
                "~/Scripts/xeditable.min.js")
                );
        }
    }
}
