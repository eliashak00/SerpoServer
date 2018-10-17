using System.Collections.Generic;
using Nancy;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Database;

namespace SerpoServer.Api
{
    public static class SiteManager
    {
        private static IDatabase db => CallDb.GetDb();

        public static IEnumerable<spo_site> GetSites()
        {
            return db.Query<spo_site>("SELECT * FROM spo_sites");
        }

        public static spo_site GetSiteById(int id)
        {
            return db.SingleOrDefault<spo_site>("SELECT * FROM spo_sites WHERE site_id = @0", id);
        }

        public static spo_site GetSiteByDom(string domain)
        {
            return db.SingleOrDefault<spo_site>("SELECT * FROM spo_sites WHERE site_domain = @0", domain);
        }

        public static spo_site GetSite(this NancyContext ctx)
        {
            return GetSiteById((int) ctx.Parameters.site);
        }
    }
}