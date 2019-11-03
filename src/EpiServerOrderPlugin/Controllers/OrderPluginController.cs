using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using EPiServer.Logging;
using EPiServer.PlugIn;

namespace EpiServerOrderPlugin.Controllers
{
    [Authorize(Roles = "Administrators")]
    [GuiPlugIn(Area = PlugInArea.AdminMenu, UrlFromModuleFolder = "OrderPlugin", DisplayName = "Order Integration")]
    public class OrderPluginController : Controller
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(OrderPluginController));

        private static readonly Regex cmsRegex = new Regex("/cms/$", RegexOptions.IgnoreCase);
        
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