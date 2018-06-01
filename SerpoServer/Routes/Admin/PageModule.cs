using System;
using Community.CsharpSqlite;
using Nancy;
using Nancy.ModelBinding;
using SerpoServer.Api;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;

namespace SerpoServer.Routes.Admin
{
    public class PageModule : NancyModule
    {
        public PageModule(PageManager pge) : base("/admin/pages")
        {
            Get("/", x => View["pages.html", pge.GetPages((string)Request.Query.domain)]);
            Get("/createoredit", x => View["page-editor.html", pge.GetPage((string)Request.Query.domain, Enum.Parse<RequestMethods>(Request.Query.method), (string)Request.Query.path)]);
            Post("/createoredit", x =>
            {
                var page = this.BindAndValidate<spo_page>();
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