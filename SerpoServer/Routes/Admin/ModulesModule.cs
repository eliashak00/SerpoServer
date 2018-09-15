using System.Collections.Generic;
using System.Linq;
using IronPython.Modules;
using Nancy;
using Nancy.Extensions;
using Nancy.Json.Simple;
using Nancy.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            Get("/createoredit", x =>
            {
                this.RequiresAuthentication();
                return View["module-editor.html", new spo_module()];
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

                var reqS = Request.Body.AsString();
                var data = JArray.Parse(reqS);
                
                IList<spo_module> modules = new List<spo_module>();
                foreach (var item in data)
                {
                    var obj = new spo_module
                    {
                        module_active = item["module_active"].Value<bool>(),
                        module_id = item["module_id"].Value<int>(),
                        module_lat = item["module_lat"].Value<int>(),
                        module_name = item["module_name"].Value<string>(),
                        module_js = item["module_js"].Value<string>(),
                        module_pos = item["module_pos"].Value<int>()
                    };
                    modules.Add(obj);
                }
                manager.CreateOrEdit(modules);
                return HttpStatusCode.OK;
            });
        }
    }
}