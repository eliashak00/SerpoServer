using System;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json;
using SerpoServer.Intepreter;

namespace SerpoServer.Admin
{
    public class ToolsModule : NancyModule
    {
        public ToolsModule(Python py) : base("/admin/tools")
        {
            Post("/script/debug", x =>
            {
                var script = JsonConvert.DeserializeObject<scriptData>(Request.Body.AsString());
                if (script == null || string.IsNullOrWhiteSpace(script.script)) return HttpStatusCode.BadRequest;
                try
                {
                    py.Execute<dynamic>(script.script);
                    return HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new {errors = ex.Message});
                }
            });
        }

        private class scriptData
        {
            public string script;
        }
    }
}