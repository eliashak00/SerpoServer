using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Responses;
using Nancy.TinyIoc;
using RazorEngine;
using SerpoServer.Api;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Errors;
using Encoding = System.Text.Encoding;

namespace SerpoServer
{
    public class Bootstrap : DefaultNancyBootstrapper
    {
        
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            pipelines.OnError.AddItemToEndOfPipeline((x, e) =>
            {
               Logger.Write(e);
                if (ConfigurationProvider.ConfigurationFile.telemetry)
                {
                    Logger.Telemetry(e);
                }

                var pm = container.Resolve<PageManager>();
                var page = pm.GetPage(x.Request.Url.HostName, RequestMethods.Get, "ERROR");
                if (page != null)
                {
                    var pageBytes = Encoding.UTF8.GetBytes(page.page_view);
                   x.Response = new HtmlResponse(HttpStatusCode.InternalServerError, s => s.Write(pageBytes, 0, pageBytes.Length));
                }
                else
                {
                    x.Response = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(),
                        "SerpoServer.Errors.500.html", "500.html");
                }

                return e;
            });
         
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.ViewLocationConventions.Add((s,d,c) => "AdminViews/" + s);
            nancyConventions.StaticContentsConventions.AddDirectory("AdminViews/Static/");
           
            base.ConfigureConventions(nancyConventions);
        }

        protected override IDiagnostics GetDiagnostics()
        {
            return new DisabledDiagnostics();
        }

        public override void Configure(INancyEnvironment environment)
        {
            environment.Views(true, true);
            base.Configure(environment);
        }
    }
}