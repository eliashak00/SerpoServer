using System.Collections.Generic;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Extensions;
using Nancy.Responses;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using PetaPoco;
using RazorEngine;
using SerpoServer.Api;
using SerpoServer.Data;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Errors;
using SerpoServer.Intepreter;
using SerpoServer.Security;
using Encoding = System.Text.Encoding;

namespace SerpoServer
{
    public class Bootstrap : DefaultNancyBootstrapper
    {

        private static IPipelines _pipelines;
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {     
            var site = container.Resolve<SiteManager>();
            var reqSite = context.Request.Url.Path.Contains("/admin") ? site.GetSite((int)context.Request.Query.site) : site.GetSite(context.Request.Url.BasePath);
            if(reqSite != null)
                context.Parameters.site = reqSite;
            base.RequestStartup(container, pipelines, context);
        }


        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            
            //debug
            this.Conventions.ViewLocationConventions.Add((viewName, model, context) =>
                {
                    return string.Concat("AdminViews/views/", viewName);
                });


            Nancy.Embedded.Conventions.EmbeddedStaticContentConventionBuilder.AddDirectory("/static",
                Assembly.GetExecutingAssembly(), "SerpoServer.EmbeddedStatic", ".css", ".js", ".png");
            
            pipelines.OnError.AddItemToStartOfPipeline((x, e) =>
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
     
            if (string.IsNullOrWhiteSpace(ConfigurationProvider.ConfigurationFile.connstring))
            {
                container.Register<IDatabase>(new NullDatabase());
                pipelines.BeforeRequest.InsertBefore("installBlock",c =>
                {
     
                    if (c.IsAjaxRequest() && c.Request.Path.Contains("/install/"))
                        return null;
                    c.Response = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(), "SerpoServer.Install.install.html", ".html");
                    return c.Response;
                });
         
            }
            else
            {
                container.Register<IDatabase>(new Connection());
            }

            _pipelines = pipelines;
        }

        public static void DisableBlock()
        {
            _pipelines.BeforeRequest.RemoveByName("instalBlock");
            TinyIoCContainer.Current.Unregister<IDatabase>();
            TinyIoCContainer.Current.Register<IDatabase>(new Connection());
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