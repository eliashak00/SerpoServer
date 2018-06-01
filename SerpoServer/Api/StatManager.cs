
using System.Collections.Generic;
using PetaPoco;
using SerpoServer.Data;
using SerpoServer.Data.Models;

namespace SerpoServer.Api
{
    public class StatManager
    {
        private IDatabase db;
        private int site;
        public StatManager(int site)
        {
            db = new Connection().Get();
            this.site = site;
        }

        public IEnumerable<int> GetAvailWeeks() => db.Query<int>("SELECT DISTINCT WEEK(day_date) FROM day_days WHERE day_site = @0", site);

        public IEnumerable<spo_days> GetWeek(int week) => db.Query<spo_days>("SELECT * FROM day_days WHERE day_site = @0 AND WEEK(day_date) = @1", site, week);
        
    }
}