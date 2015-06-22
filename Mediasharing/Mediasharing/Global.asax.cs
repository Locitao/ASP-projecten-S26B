using System;
using System.Web.Routing;
using System.Web.UI;

namespace Mediasharing
{
    /// <summary>
    /// This class manages the routing of webpages.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        #region Methods
        /// <summary>
        /// Register the route.
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Index", "Index/{id}", "~/index.aspx");
            routes.MapPageRoute("Item", "Item/{id}", "~/item.aspx");
            routes.MapPageRoute("PostMessage", "PostMessage/{id}", "~/PostMessage.aspx");
            routes.MapPageRoute("UploadItem", "UploadItem/{id}", "~/UploadItem.aspx");
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
                new ScriptResourceDefinition
                {
                    Path = "~/js/jquery.min.js"
                }
            );
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        #endregion
    }
}