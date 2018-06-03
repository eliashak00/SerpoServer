using Nancy;
using Nancy.Security;
using SerpoServer.Security;

namespace SerpoServer.Routes.Admin
{
    public class MainModule : NancyModule
    {
        public MainModule() : base("/admin")
        {
            this.RequiresAuthentication();
            Get("/", x =>
            {
                if (Context.CurrentUser == null)
                    return View["auth.html"];
                return !Request.Cookies.TryGetValue("site", out var site)? View["admin/site.html"] : View["admin/views/main.html"];
            });
       
            
        }
    }
}