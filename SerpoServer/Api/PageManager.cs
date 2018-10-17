using System;
using System.Collections.Generic;
using Nancy;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Database;
using SerpoServer.Intepreter;

namespace SerpoServer.Api
{
    public class PageManager
    {
        private static IList<Tuple<string, RequestMethods, spo_page>> PageCache =
            new List<Tuple<string, RequestMethods, spo_page>>();

        private readonly Python py;

        private readonly IDatabase db = CallDb.GetDb();

        public PageManager(Python py)
        {
            this.py = py;
        }

        public IEnumerable<spo_page> GetPages(string domain)
        {
            return db.Query<spo_page>(
                "SELECT * FROM spo_pages INNER JOIN spo_sites ON spo_sites.site_id = spo_pages.page_site WHERE spo_sites.site_domain = @0",
                domain);
        }

        public IEnumerable<spo_page> GetPages(string domain, RequestMethods method)
        {
            return db.Query<spo_page>(
                "SELECT * FROM spo_pages INNER JOIN spo_sites ON spo_sites.site_id = spo_pages.page_site WHERE spo_sites.site_domain = @0 AND spo_pages.page_methods = @1",
                domain, method);
        }

        public spo_page GetPage(string domain, RequestMethods method, string path = "/")
        {
            return db.SingleOrDefault<spo_page>(
                "SELECT * FROM spo_pages LEFT JOIN spo_sites ON site_id = page_site WHERE site_domain = @0 AND page_methods = @1 AND page_route = @2",
                domain, path, method);
        }

        public spo_page GetPage(int id)
        {
            return db.SingleOrDefault<spo_page>(
                "SELECT * FROM spo_pages WHERE page_id = @0", id);
        }

        public HttpStatusCode CreateOrEdit(spo_page data)
        {
            if (data.page_script != null)
                if (!py.IsValid(data.page_script))
                    return HttpStatusCode.BadRequest;

            if (data.page_id == 0)
                db.Insert(data);
            else
                db.Update(data);
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            var page = db.SingleOrDefault<spo_page>("SELECT * FROM spo_pages WHERE page_id = @0", id);
            if (page == null) return HttpStatusCode.BadRequest;
            db.Delete(page);
            return HttpStatusCode.OK;
        }
    }
}