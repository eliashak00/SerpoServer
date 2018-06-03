using System.Collections.Generic;
using PetaPoco;
using SerpoServer.Data.Models;

namespace SerpoServer.Api
{
    public class SiteManager
    {
        private IDatabase db;
        public SiteManager(IDatabase db)
        {
            this.db = db;
        }

        public IEnumerable<spo_site> GetSites() => db.Query<spo_site>("SELECT * FROM spo_sites");

        public spo_site GetSite(int id) =>
            db.SingleOrDefault<spo_site>("SELECT * FROM spo_sites WHERE site_id = @0", id);
        
        public spo_site GetSite(string domain) => 
            db.SingleOrDefault<spo_site>("SELECT * FROM spo_sites WHERE site_domain = @0", domain);
    }
}