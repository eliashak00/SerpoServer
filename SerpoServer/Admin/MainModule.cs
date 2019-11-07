using Nancy;
using Nancy.Extensions;
using Nancy.Security;
using Newtonsoft.Json;
using SerpoServer.Api;
using SerpoServer.Data.Models.View;
using SerpoServer.Security;

namespace SerpoServer.Admin
{
    public class MainModule : NancyModule
    {
        public MainModule(ModuleManager md, IRootPathProvider root) : base("/admin")
        {
            Get("/", x =>
            {
                this.RequiresAuthentication();
                this.RequiresSite();

                var left = md.ActiveModules(0);
                var right = md.ActiveModules(1);
                return View["main.html", new {LeftColumn = left, RightColumn = right}];
            });
            Post("/account/login", x =>
            {
                var data = JsonConvert.DeserializeObject<Login>(Request.Body.AsString());

                var result = Identity.Login(data.Email, data.Password);
                return result == null ? HttpStatusCode.BadRequest : Response.AsJson(new {token = result});
            });
            Get("/site", x => View["shared/site.html", new {Sites = SiteManager.GetSites()}]);

            Get("/site/get", x =>
            {
                this.RequiresAuthentication();
                this.RequiresSite();
                return Response.AsJson(new {Site = Context.GetSite()});
            });
            Post("/sites/select/{id}", x =>
            {
                var site = (int) x.id;
                return Response.AsJson(new {Site = SiteManager.GetSiteById(site)});
            });
        }
    }
}