using System;
using Nancy;
using Nancy.Extensions;
using Nancy.Json.Simple;
using Newtonsoft.Json;
using SerpoServer.Intepreter;

namespace SerpoServer.Routes.Admin
{
    public class ToolsModule : NancyModule
    {
        public ToolsModule(PyRuntime runtime) : base("/admin/tools")
        {
            Post("/script/debug", x =>
            {
                var script = JsonConvert.DeserializeObject<scriptData>(Request.Body.AsString());
                if (script == null || string.IsNullOrWhiteSpace(script.script)) return HttpStatusCode.BadRequest;
                try
                {
                    var comp = runtime.Compile(script.script);
                    return HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new{errors = ex.Message});
                }

           
            });
        }

        class scriptData
        {
            public string script;
        }
    }
}