using System.Collections.Generic;
using Nancy;
using Nancy.TinyIoc;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Database;

namespace SerpoServer.Api
{
    public class StatManager
    {
        private readonly int site;
        private readonly IDatabase db = CallDb.GetDb();

        public StatManager()
        {
            var ctx = TinyIoCContainer.Current.Resolve<NancyContext>();
            if (ctx.Parameters != null)
                site = ctx.GetSite().site_id;
        }

        public IEnumerable<int> GetAvailWeeks()
        {
            return db.Query<int>("SELECT DISTINCT WEEK(day_date, 1) FROM day_days WHERE day_site = @0", site);
        }

        public IEnumerable<spo_day> GetWeek(int week)
        {
            return db.Query<spo_day>("SELECT * FROM spo_days WHERE day_site = @0 AND WEEK(day_date, 1) = @1", site,
                week);
        }
    }
}