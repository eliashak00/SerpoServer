using System.Collections.Generic;
using Nancy;
using Nancy.TinyIoc;
using PetaPoco;
using SerpoServer.Data;
using SerpoServer.Data.Models;

namespace SerpoServer.Api
{
    public static class SiteManager
    {
        private static IDatabase db => TinyIoCContainer.Current.Resolve<Connection>();

        public static IEnumerable<spo_site> GetSites() => db.Query<spo_site>("SELECT * FROM spo_sites");

        public static spo_site GetSiteById(int id) =>
            db.SingleOrDefault<spo_site>("SELECT * FROM spo_sites WHERE site_id = @0", id);
        
        public static spo_site GetSiteByDom(string domain) => 
            db.SingleOrDefault<spo_site>("SELECT * FROM spo_sites WHERE site_domain = @0", domain);

        public static spo_site GetSite(this NancyContext ctx) => GetSiteById((int) ctx.Parameters.site);
    }
}