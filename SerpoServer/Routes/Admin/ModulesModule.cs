using System.Linq;
using Nancy;
using Nancy.Extensions;
using Nancy.Security;
using Newtonsoft.Json;
using SerpoServer.Api;
using SerpoServer.Data.Models;

namespace SerpoServer.Routes.Admin
{
    public class ModulesModule : NancyModule
    {
        public ModulesModule(ModuleManager manager) : base("/admin/modules")
        {
     
            Get("/all", x => Response.AsJson(manager.AllModules.Where(m => m.module_active)));
      
            Delete("/delete/{id}", x =>
            {
                this.RequiresAuthentication();
                var id = (int) x.id;
                manager.Delete(id);
                return HttpStatusCode.OK;
            });

            Get("/", x =>
            {
                this.RequiresAuthentication();
                return View["modules.html", new {Modules = manager.AllModules}];
            });
            Get("/createoredit}", x =>
            {
                this.RequiresAuthentication();
                return View["module-editor.html", new spo_modules()];
            });
            Get("/createoredit/{id}", x =>
            {
                this.RequiresAuthentication();
                var id = (int) x.id;
                return View["module-editor.html", manager.GetModule(id)];
            });
            Post("/createoredit", x =>
            {
                this.RequiresAuthentication();
                var data = JsonConvert.DeserializeObject<spo_modules>(Request.Body.AsString());
                manager.CreateOrEdit(data);
                return HttpStatusCode.OK;
            });
        }
    }
}