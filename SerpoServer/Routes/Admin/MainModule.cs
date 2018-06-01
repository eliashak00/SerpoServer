using Nancy;
using SerpoServer.Security;

namespace SerpoServer.Routes.Admin
{
    public class MainModule : NancyModule
    {
        public MainModule() : base("/admin")
        {
            Get("/", x =>
            {
                if (Context.CurrentUser == null)
                    return View["auth.html"];
                if (((string) Request.Query.site) == null)
                    return View["site.html"];
                return View["main.html"];
            });
            
        }
    }
}