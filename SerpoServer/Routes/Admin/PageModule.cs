using System;
using System.Linq;
using Community.CsharpSqlite;
using Nancy;
using Nancy.Extensions;
using Nancy.Json.Simple;
using Nancy.ModelBinding;
using Nancy.Security;
using SerpoServer.Api;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;

namespace SerpoServer.Routes.Admin
{
    public class PageModule : NancyModule
    {
        public PageModule(PageManager pge) : base("/admin/pages")
        {
            this.RequiresAuthentication();
            Get("/", x =>
            {
                var site = Context.Items.FirstOrDefault(y => y.Key == "site");
                return site.Value == null?  View["shared/site.html", new{Sites = SiteManager.GetSites()}] : View["pages.html", new{Pages = pge.GetPages(((spo_site)site.Value).site_domain)}];
            });
            Get("/createoredit", x =>
            {
                var site = Context.Items.FirstOrDefault(y => y.Key == "site");
                if(site.Value == null)
                    return View["shared/site.html", new {Sites = SiteManager.GetSites()}];
                return Request.Query.page != null ? View["page-editor.html", pge.GetPage((int)Request.Query.page)] : View["page-editor.html", new spo_page()];

            });
            Post("/createoredit", x =>
            {

                var page = this.Bind<spo_page>();
                
                return page == null ? HttpStatusCode.BadRequest : pge.CreateOrEdit(page);
            });
            Delete("/delete/{id}", x =>
            {
                var id = (int) x.id;
                return pge.Delete(id);
            });
        }
    }
}