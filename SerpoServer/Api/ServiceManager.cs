
using System.Collections.Generic;
using Nancy;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Intepreter;

namespace SerpoServer.Api
{
    public class ServiceManager
    {
        private IDatabase db;
        private PyRuntime python;
        
        public ServiceManager(PyRuntime python)
        {
            this.python = python;
            db = new Data.Connection().Get();
        }

        public IEnumerable<spo_service> GetServices() => db.Query<spo_service>("SELECT * FROM spo_services");

        public spo_service GetService(int id) =>
            db.SingleOrDefault<spo_service>("SELECT * FROM spo_services WHERE service_id = @0", id);
        
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