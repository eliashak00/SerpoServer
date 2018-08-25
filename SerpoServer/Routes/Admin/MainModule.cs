using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.Cookies;
using Nancy.Extensions;
using Nancy.Json.Simple;
using Nancy.Responses;
using Nancy.Security;
using Newtonsoft.Json;
using SerpoServer.Api;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.View;
using SerpoServer.Security;

namespace SerpoServer.Routes.Admin
{
    public class MainModule : NancyModule
    {
        public MainModule(ModuleManager md) : base("/admin")
        {
   
            Get("/", x =>
            {
                this.RequiresAuthentication();
                this.RequiresSite();
               
                return View["main.html", new {LeftColumn = md.ActiveModules(0), RightModule = md.ActiveModules(1)}];
                
            });
               Post("/account/login", x =>
               {
                   var data = JsonConvert.DeserializeObject<Login>(Request.Body.AsString());
                   var result = Identity.Login(data.Email, data.Password);
                   return result == null ? HttpStatusCode.BadRequest : Response.AsJson(new {token = result});
               });
            Get("/site", x => View["shared/site.html", new {Sites = SiteManager.GetSites()}]);
            
            Get("/site/get",x =>
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
            Get("/static/{file}", x => new EmbeddedFileResponse(Assembly.GetExecutingAssembly(), "SerpoServer.AdminViews.Static",
                (string) x.path));
        }

   
    }
}