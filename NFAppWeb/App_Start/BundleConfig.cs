﻿using System.Web;
using System.Web.Optimization;

namespace NFAppWeb
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            */

            //SCRIPT
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery/jQuery-2.1.4.min.js",
                "~/Scripts/jquery/jquery-ui.js",
                "~/Scripts/jquery/jquery.dataTables.min.js",
                "~/Scripts/jquery/jquery.slimscroll.min.js",
                "~/Scripts/jquery/jquery.maskMoney.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap/bootstrap.min.js",
                "~/Scripts/bootstrap/bootstrapValidator.min.js",
                "~/Scripts/bootstrap/dataTables.bootstrap.min.js",
                "~/Scripts/bootstrap/bootstrap-duallistbox.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                "~/Scripts/datepicker/datepicker.min.js",
                "~/Scripts/datepicker/datepicker.pt-BR.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/selectize").Include(
                "~/Scripts/selectize/selectize.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/mask").Include(
                "~/Scripts/mask/jquery.mask.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jsPlumb").Include(
                "~/Scripts/jsPlumb/jsPlumbToolkit-1.0.20.js",
                "~/Scripts/jsPlumb/syntax-highlighter.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/admin-lte").Include(
                "~/Scripts/admin-lte/app.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                "~/Scripts/Site.js"
                ));
            //SCRIPT

            //STYLE
            bundles.Add(new StyleBundle("~/bundles/style-jquery").Include(
                "~/Content/jquery/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/bundles/style-bootstrap").Include(
                "~/Content/bootstrap/css/bootstrap.min.css",
                "~/Content/bootstrap/css/dataTables.bootstrap.css",
                "~/Content/bootstrap/css/bootstrap-duallistbox.css"));

            bundles.Add(new StyleBundle("~/bundles/style-font").Include(
                "~/Content/font-awesome/css/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/bundles/style-datepicker").Include(
                "~/Content/datepicker/css/datepicker.min.css"));

            bundles.Add(new StyleBundle("~/bundles/style-selectize").Include(
                "~/Content/selectize/selectize.css"));

            bundles.Add(new StyleBundle("~/bundles/style-jsPlumb").Include(
                "~/Content/jsPlumb/jsPlumb.css"));

            bundles.Add(new StyleBundle("~/bundles/style-admin-lte").Include(
                "~/Content/admin-lte/css/AdminLTE.min.css",
                "~/Content/admin-lte/css/skin-blue-light.min.css"));

            bundles.Add(new StyleBundle("~/bundles/style-site").Include(
                "~/Content/Site.css"));
            //STYLE

        }
    }
}
