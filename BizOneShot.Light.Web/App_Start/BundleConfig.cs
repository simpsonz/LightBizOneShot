using System.Web;
using System.Web.Optimization;

namespace BizOneShot.Light.Web
{
    public class BundleConfig
    {
        // 번들 작성에 대한 자세한 내용은 http://go.microsoft.com/fwlink/?LinkId=301862 링크를 참조하십시오.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                       "~/Scripts/jquery-ui-1.11.4/jquery-ui-1.11.4.js"));

            bundles.Add(new StyleBundle("~/css/jquery-ui").Include(
                      "~/Scripts/jquery-ui-themes-1.11.4/themes/smoothness/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Modernizr의 개발 버전을 사용하여 개발하고 배우십시오. 그런 다음
            // 프로덕션할 준비가 되면 http://modernizr.com 링크의 빌드 도구를 사용하여 필요한 테스트만 선택하십시오.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/cert").Include(
                      "~/js/cert.js",
                      "~/cert/js/TSToolkitConfig.js",
                      "~/cert/js/TSToolkitObject.js"));

            bundles.Add(new ScriptBundle("~/bundles/localstorage").Include(
                      "~/js/localstorage.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/css/common").Include(
                      "~/css/common.css"));

            bundles.Add(new StyleBundle("~/css/print").Include(
                      "~/css/print.css"));

            bundles.Add(new StyleBundle("~/css/zip_common").Include(
                      "~/css/zip_common.css"));

            bundles.Add(new ScriptBundle("~/bundles/highcharts").Include(
                     "~/Scripts/highcharts.js",
                     "~/Scripts/highcharts-more.js",
                     "~/Scripts/modules/exporting.js"));
        }
    }
}
