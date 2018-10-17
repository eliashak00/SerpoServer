using System.Collections.Generic;
using Nancy;
using Nancy.TinyIoc;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Database;

namespace SerpoServer.Api
{
    public class ServiceManager
    {
        private readonly int site;

        private readonly IDatabase db = CallDb.GetDb();

        public ServiceManager()
        {
            var ctx = TinyIoCContainer.Current.Resolve<NancyContext>();
            if (ctx.Parameters != null)
                site = ctx.Parameters.site;
        }

        public IEnumerable<spo_service> GetServices()
        {
            return db.Query<spo_service>("SELECT * FROM spo_services");
        }

        public IEnumerable<spo_service> GetSiteServices()
        {
            return db.Query<spo_service>("SELECT * FROM spo_services WHERE service_site = @0", site);
        }

        public spo_service GetService(int id)
        {
            return db.SingleOrDefault<spo_service>("SELECT * FROM spo_services WHERE service_id = @0 ", id);
        }


        public void ChangeStatus(int id)
        {
            var service = GetService(id);
            if (service == null) return;
            db.Execute("REPLACE INTO spo_service_rel VALUES sr_site = @0, sr_service = @1", site, id);
        }

        public HttpStatusCode CreateOrEdit(spo_service service)
        {
            if (service.service_script == null)
                return HttpStatusCode.BadRequest;


            if (service.service_id == 0)
                db.Insert(service);
            else
                db.Update(service);

            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            if (id == 0) return HttpStatusCode.BadRequest;
            db.Delete<spo_service>("DELETE FROM spo_services WHERE service_id = @0", id);
            return HttpStatusCode.OK;
        }
    }
}