
using System.Collections.Generic;
using Nancy;
using Nancy.TinyIoc;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Intepreter;

namespace SerpoServer.Api
{
    public class ServiceManager
    {
        private IDatabase db;
        private PyRuntime python;
        private int site;
        public ServiceManager(PyRuntime python, IDatabase db)
        {
            this.python = python;
            this.db = db;
            var ctx = TinyIoCContainer.Current.Resolve<NancyContext>();
            if(ctx.Parameters != null)
                site = ctx.Parameters.site;
        }

        public IEnumerable<spo_service> GetServices() => db.Query<spo_service>("SELECT * FROM spo_services");

        public IEnumerable<spo_service> GetSiteServices() => db.Query<spo_service>("SELECT * FROM spo_services WHERE service_site = @0",  site);
        
        public spo_service GetService(int id) =>
            db.SingleOrDefault<spo_service>("SELECT * FROM spo_services WHERE service_id = @0 ", id);

        
        public void ChangeStatus(int id)
        {
            var service = GetService(id);
            if(service == null) return;
            db.Execute("REPLACE INTO spo_service_rel VALUES sr_site = @0, sr_service = @1",site, id);
        }
        
        public HttpStatusCode CreateOrEdit(spo_service service)
        {
            if (service.service_script == null)
                return HttpStatusCode.BadRequest;

            python.Compile(service.service_script);
            
            if (service.service_id == 0)
            {
                db.Insert(service);
            }
            else
            {
                db.Update(service);
            }

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