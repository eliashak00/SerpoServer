
using System.Collections.Generic;
using Nancy;
using Nancy.TinyIoc;
using PetaPoco;
using SerpoServer.Data;
using SerpoServer.Data.Models;

namespace SerpoServer.Api
{
    public class StatManager
    {
        private IDatabase db;
        private int site;
        public StatManager(IDatabase db)
        {
            this.db = db;
            var ctx = TinyIoCContainer.Current.Resolve<NancyContext>();
            if(ctx.Parameters != null)
                site = ctx.Parameters.site;
        }

        public IEnumerable<int> GetAvailWeeks() => db.Query<int>("SELECT DISTINCT WEEK(day_date) FROM day_days WHERE day_site = @0", site);

        public IEnumerable<spo_day> GetWeek(int week) => db.Query<spo_day>("SELECT * FROM day_days WHERE day_site = @0 AND WEEK(day_date) = @1", site, week);
        
    }
}