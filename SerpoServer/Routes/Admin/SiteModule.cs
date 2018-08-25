using Nancy;
using SerpoServer.Api;

namespace SerpoServer.Routes.Admin
{
    public class SiteModule : NancyModule
    {
        public SiteModule() : base("/admin/sites")
        {
            Get("/", x => View["site.html", new{Sites = SiteManager.GetSites()}]);
        }
    }
}