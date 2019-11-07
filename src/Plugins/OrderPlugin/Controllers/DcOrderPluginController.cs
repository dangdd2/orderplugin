using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Mvc;
using EPiServer.Logging;
using EPiServer.PlugIn;

namespace Dc.EpiServerOrderPlugin.Controllers
{
    public class OrderIntegrationViewModel
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
    }


    [Authorize(Roles = "Administrators")]
    [GuiPlugIn(Area = PlugInArea.AdminMenu, UrlFromModuleFolder = "OrderPlugin", DisplayName = "Order Integration")]
    public class DcOrderPluginController : Controller
    {
        // keys for the appSettings
        private const string urlKey = "EPi.OrderIntegration.Url";
        private const string apiKeyKey = "EPi.OrderIntegration.ApiKey";

        public ActionResult Index(string save, string url, string apiKey)
        {

            if (!string.IsNullOrWhiteSpace(save))
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("/");

                AddOrUpdateSetting(config.AppSettings.Settings, urlKey, url);
                AddOrUpdateSetting(config.AppSettings.Settings, apiKeyKey, apiKey);

                config.Save();
            }

            var model = new OrderIntegrationViewModel
            {
                Url = WebConfigurationManager.AppSettings.Get(urlKey) ?? "<not specific>",
                ApiKey = WebConfigurationManager.AppSettings.Get(apiKeyKey) ?? "<not specific>"
            };

            return View(GetViewLocation("Index"), model);
        }

        private void AddOrUpdateSetting(KeyValueConfigurationCollection settings, string key, string value)
        {
            var existing = settings[key];
            if (existing == null)
            {
                settings.Add(key, value);
            }
            else
            {
                existing.Value = value;
            }
        }


        /// <summary>
        /// return View(GetViewLocation("Index"));
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
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

            return $"{EPiServer.Shell.Paths.ProtectedRootPath}EpiServerOrderPlugin/Views/OrderPlugin/{viewName}.cshtml";
        }
    }
}