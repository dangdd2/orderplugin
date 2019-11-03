using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using EPiServer.Logging;
using EPiServer.PlugIn;

namespace Dc.EpiServerOrderPlugin.Controllers
{
    [Authorize(Roles = "Administrators")]
    [GuiPlugIn(Area = PlugInArea.AdminMenu, UrlFromModuleFolder = "EpiServerOrderPlugin", DisplayName = "Order Integration")]
    public class DcOrderPluginController : Controller
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(DcOrderPluginController));

        private static readonly Regex cmsRegex = new Regex("/cms/$", RegexOptions.IgnoreCase);

        // NOTE: we are using module feature to prefix our controllers (see module.config in this project), in module config the controller prefix Swap will be removed in the routing
        // GuiPlugIn: UrlFromModuleFolder = "DebugViewLinks" => our controller without the "Swap" prefix. Url in admin will be: /EPiDebugViewLinks/DebugViewLinks (module name + controller name without the prefix)

        public ActionResult Index()
        {
            try
            {
                return View(GetViewLocation("Index"));
            }
            catch (Exception ex)
            {
                logger.Error("Failed to load plugin page.", ex);

                throw;
            }
        }

        private static string GetViewLocation(string viewName)
        {
            // Episerver has registered a razor view engine for our module with custom view locations like these:
            // ~/Modules/_Protected/EpiServerOrderPlugin/Views/OrderPlugin/[ACTION-NAME].cshtml
            // ~/Modules/_Protected/EpiServerOrderPlugin/Views/Shared/[ACTION-NAME].cshtml

            // BUT THOSE DON'T SEEM TO WORK, I BELIEVE THOSE URLS WORKED OCCASIONALLY DURING TESTING BUT MOST OF THE TIME NOT
            // SO THAT IS THE REASON WHY THIS CUSTOM METHOD TO GET VIEW LOCATIONS

            // ProtectedRootPath == from web.config episever.shell -> <protectedModules rootPath="~/YOUR-VALUE-HERE/">
            // but the views can be found also using the: ProtectedRootPath + our module name + the normal path inside our module (remember we will zip the module)
            // so if you config value is EPiServer you get: ~/EPiServer/EpiServerOrderPlugin/Views/OrderPlugin/[viewName].cshtml

            return $"{EPiServer.Shell.Paths.ProtectedRootPath}EPiDebugViewLinks/Views/OrderPlugin/{viewName}.cshtml";
        }
    }
}