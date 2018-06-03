using System.Linq;
using Nancy;
using Nancy.Security;
using SerpoServer.Api;

namespace SerpoServer.Routes.Admin
{
    public class StatModule : NancyModule
    {
        public StatModule(StatManager sts) : base("/admin/stats")
        {
            this.RequiresAuthentication();
            Get("/", x => View["admin/views/stats.html", sts.GetAvailWeeks()]);
            Get("/week/{number}", x =>
            {
                var week = sts.GetWeek((int) x.number);
                if (week == null) return HttpStatusCode.BadRequest;
                var total = 0;
                week.ToList().ForEach(w => total = +w.day_views);
                return Response.AsJson(new{totalViews = total,weeks = week});
            });
        }
    }
}