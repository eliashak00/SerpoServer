using Nancy;
using Nancy.ModelBinding;
using SerpoServer.Api;
using SerpoServer.Data.Models;

namespace SerpoServer.Routes.Admin
{
    public class ServiceModule : NancyModule
    {
        public ServiceModule(ServiceManager svrc) : base("/admin/services")
        {
            Get("/", x => View["services.html", svrc.GetServices()]);
            Get("/createoredit", x => View["service-editor.html", svrc.GetService((int)Request.Query.id)]);
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
        }
    }
}