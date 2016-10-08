using System.Web;
using System.Web.Optimization;

namespace AWeb
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            

            //bundles.Add(new StyleBundle("~/content/css/smartadmin").IncludeDirectory("~/content/css", "*.min.css"));
            bundles.Add(new StyleBundle("~/content/css/smartadmin").Include(
                //"~/content/css/bootstrap.min.css",
                //"~/content/css/font-awesome.min.css",

                "~/content/css/bootstrap.min.css",
                "~/content/css/font-awesome.min.css",

                "~/scripts/DataTables/DataTables-1.10.10/css/dataTables.bootstrap.min.css",
                //"~/scripts/DataTables/AutoFill-2.1.0/css/autoFill.bootstrap.css",
                "~/scripts/DataTables/Buttons-1.1.0/css/buttons.bootstrap.min.css",
                //"~/scripts/ColReorder-1.3.0/css/colReorder.bootstrap.min.css",
                //"~/scripts/FixedColumns-3.2.0/css/fixedColumns.bootstrap.min.css",
                //"~/scripts/FixedHeader-3.1.0/css/fixedHeader.bootstrap.min.css",
                //"~/scripts/KeyTable-2.1.0/css/keyTable.bootstrap.min.css",
                //"~/scripts/Responsive-2.0.0/css/responsive.bootstrap.min.css",
                //"~/scripts/RowReorder-1.1.0/css/rowReorder.bootstrap.min.css",
                //"~/scripts/Scroller-1.4.0/css/scroller.bootstrap.min.css",
                "~/scripts/Select-1.1.0/css/select.bootstrap.min.css",

                "~/content/css/select2.min.css",
                "~/content/css/select2-bootstrap.min.css",

                "~/content/css/invoice.min.css",
                "~/content/css/lockscreen.min.css",
                "~/content/css/smartadmin-production-plugins.min.css",
                "~/content/css/smartadmin-production.min.css",
                "~/content/css/smartadmin-rtl.backup.min.css",
                "~/content/css/smartadmin-rtl.min.css",
                "~/content/css/smartadmin-skins.min.css",
                "~/content/css/your_style.min.css"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                "~/scripts/jquery-2.1.4.min.js",
                "~/scripts/jquery-ui-1.11.4-utf8.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                "~/scripts/jquery-2.1.4.min.js",
                "~/scripts/jquery-ui-1.11.4-utf8.js",
                "~/Scripts/jquery.unobtrusive-ajax.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                "~/scripts/app.config.js",
                "~/scripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                "~/scripts/bootstrap.min.js",
                "~/scripts/notification/SmartNotification.min.js",
                "~/scripts/smartwidgets/jarvis.widget.min.js",
                "~/scripts/plugin/jquery-validate/jquery.validate.min.js",
                "~/scripts/plugin/masked-input/jquery.maskedinput.min.js",
                "~/scripts/plugin/select2/select2.min.js",
                "~/scripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
                "~/scripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
                "~/scripts/plugin/msie-fix/jquery.mb.browser.min.js",
                "~/scripts/plugin/fastclick/fastclick.min.js",
                "~/scripts/app.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/full-calendar").Include(
                "~/scripts/plugin/moment/moment.min.js",
                "~/scripts/plugin/fullcalendar/jquery.fullcalendar.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/charts").Include(
                "~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                "~/scripts/plugin/sparkline/jquery.sparkline.min.js",
                "~/scripts/plugin/morris/morris.min.js",
                "~/scripts/plugin/morris/raphael.min.js",
                "~/scripts/plugin/flot/jquery.flot.cust.min.js",
                "~/scripts/plugin/flot/jquery.flot.resize.min.js",
                "~/scripts/plugin/flot/jquery.flot.time.min.js",
                "~/scripts/plugin/flot/jquery.flot.fillbetween.min.js",
                "~/scripts/plugin/flot/jquery.flot.orderBar.min.js",
                "~/scripts/plugin/flot/jquery.flot.pie.min.js",
                "~/scripts/plugin/flot/jquery.flot.tooltip.min.js",
                "~/scripts/plugin/dygraphs/dygraph-combined.min.js",
                "~/scripts/plugin/chartjs/chart.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
                //"~/scripts/DataTables/JSZip-2.5.0/jszip.min.js",
                //"~/scripts/DataTables/pdfmake-0.1.18/build/pdfmake.min.js",
                //"~/scripts/DataTables/pdfmake-0.1.18/build/vfs_fonts.js",

                "~/scripts/DataTables/DataTables-1.10.10/js/jquery.dataTables.min.js",
                "~/scripts/DataTables/DataTables-1.10.10/js/dataTables.bootstrap.min.js",

                "~/scripts/DataTables/AutoFill-2.1.0/js/dataTables.autoFill.min.js",
                "~/scripts/DataTables/AutoFill-2.1.0/js/autoFill.bootstrap.min.js",

                "~/scripts/DataTables/Buttons-1.1.0/js/dataTables.buttons.min.js",
                "~/scripts/DataTables/Buttons-1.1.0/js/dataTables.bootstrap.min.js",
                "~/scripts/DataTables/Buttons-1.1.0/js/buttons.colVis.min.js",
                //"~/scripts/DataTables/Buttons-1.1.0/js/buttons.flash.min.js",
                "~/scripts/DataTables/Buttons-1.1.0/js/buttons.html5.min.js",
                //"~/scripts/DataTables/Buttons-1.1.0/js/buttons.print.min.js",

                //"~/scripts/DataTables/ColReorder-1.3.0/js/dataTables.colReorder.min.js",
                //"~/scripts/DataTables/FixedColumns-3.2.0/js/dataTables.fixedColumns.min.js",
                //"~/scripts/DataTables/FixedHeader-3.1.0/js/dataTables.fixedHeader.min.js",
                //"~/scripts/DataTables/KeyTable-2.1.0/js/dataTables.keyTable.min.js",

                "~/scripts/DataTables/Responsive-2.0.0/js/dataTables.responsive.min.js",
                "~/scripts/DataTables/Responsive-2.0.0/js/responsive.bootstrap.min.js",

                //"~/scripts/DataTables/RowReorder-1.1.0/js/dataTables.rowReorder.min.js",
                //"~/scripts/DataTables/Scroller-1.4.0/js/dataTables.scroller.min.js",

                "~/scripts/DataTables/Select-1.1.0/js/dataTables.select.min.js"

                //"~/scripts/DataTables/Editor-1.5.3/js/dataTables.editor.min.js",
                //"~/scripts/DataTables/Editor-1.5.3/js/editor.bootstrap.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
                "~/scripts/plugin/jqgrid/jquery.jqGrid.min.js",
                "~/scripts/plugin/jqgrid/grid.locale-en.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/forms").Include(
                "~/scripts/plugin/jquery-form/jquery-form.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
                "~/scripts/smart-chat-ui/smart.chat.ui.min.js",
                "~/scripts/smart-chat-ui/smart.chat.manager.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
                "~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
                "~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
                ));
            BundleTable.EnableOptimizations = true;

#if DEBUG
            BundleTable.EnableOptimizations = false;
#endif


        }
    }
}
