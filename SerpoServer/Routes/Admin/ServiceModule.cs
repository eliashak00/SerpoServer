using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using SerpoServer.Api;
using SerpoServer.Data.Models;

namespace SerpoServer.Routes.Admin
{
    public class ServiceModule : NancyModule
    {
        public ServiceModule(ServiceManager svrc) : base("/admin/services")
        {
            this.RequiresAuthentication();
            Get("/", x => View["admin/views/services.html", new{availServices = svrc.GetServices(), active = svrc.GetSiteServices()}]);
            Get("/createoredit", x => View["admin/views/service-editor.html", svrc.GetService((int)Request.Query.id)]);
            Post("/createoredit", x =>
            {
                var service = this.BindAndValidate<spo_service>();
                return service == null ? HttpStatusCode.BadRequest : svrc.CreateOrEdit(service);
            });
            Delete("/delete/{id}", x =>
            {
                var id = (int) x.id;
                return svrc.Delete(id);
            });
            Put("/status/{id}", x =>
            {
                var id = (int) x.id;
                svrc.ChangeStatus(id);
                return HttpStatusCode.OK;
                
            });
        }
    }
}