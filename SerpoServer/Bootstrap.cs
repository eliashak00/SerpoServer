using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Extensions;
using Nancy.Responses;
using Nancy.TinyIoc;
using SerpoServer.Api;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Errors;
using SerpoServer.Intepreter;
using SerpoServer.Security;
using SerpoServer.Sheduling;

namespace SerpoServer
{
    public class Bootstrap : DefaultNancyBootstrapper
    {
        private static IPipelines _pipelines;

        private static bool install;

        private readonly List<string> _routeWhitelist = new List<string>
        {
            "/static",
            "/adminviews",
            "/install",
            "/adminviews/static"
        };

        protected override IRootPathProvider RootPathProvider => new RootPathProvider();

        protected override void RegisterInstances(TinyIoCContainer container,
            IEnumerable<InstanceRegistration> instanceRegistrations)
        {
            container.Register(typeof(IIntepreter), typeof(Python));

            base.RegisterInstances(container, instanceRegistrations);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            try
            {
                spo_site urlSite = null;

                if (context.Request.Url.Path.Contains("/admin"))
                {
                    if (context.IsAjaxRequest() && !string.IsNullOrWhiteSpace(context.Request.Headers.Authorization))
                    {
                        var key = context.Request.Headers.Authorization;
                        if (key.Contains("/"))
                        {
                            var site = int.Parse(key.Split('/').Last());
                            urlSite = SiteManager.GetSiteById(site);
                        }
                    }
                    else
                    {
                        dynamic siteId = null;
                        if (context.Request.Cookies.TryGetValue("site", out var site) &&
                            !string.IsNullOrWhiteSpace(site))
                            siteId = int.Parse(site);
                        if (siteId != null)
                            urlSite = SiteManager.GetSiteById((int) siteId);
                    }
                }
                else if (_routeWhitelist.Exists(x => context.Request.Url.Path.ToLower().Contains(x)))
                {
                    goto ReturnPoint;
                }
                else
                {
                    if (!install)
                        urlSite = SiteManager.GetSiteByDom(context.Request.Url.HostName);
                }


                if (urlSite != null)
                    context.Items.Add("site", urlSite);
            }
            catch
            {
                pipelines.BeforeRequest.AddItemToEndOfPipeline(async (x, r) => new EmbeddedFileResponse(
                    Assembly.GetExecutingAssembly(),
                    "SerpoServer.Errors", "500.html"));
            }

            ReturnPoint:
            base.RequestStartup(container, pipelines, context);
        }


        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            //debug
            Conventions.ViewLocationConventions.Add((viewName, model, context) =>
                string.Concat("Admin/views/", viewName));

            pipelines.OnError.AddItemToEndOfPipeline((x, e) =>
            {
                Logger.Write(e);
                if (ConfigurationProvider.ConfigurationFile.telemetry) Logger.Telemetry(e);

                var pm = container.Resolve<PageManager>();
                var page = pm.GetPage(x.Request.Url.HostName, RequestMethods.Get, "ERROR");
                if (page != null)
                {
                    var pageBytes = Encoding.UTF8.GetBytes(page.page_view);
                    x.Response = new HtmlResponse(HttpStatusCode.InternalServerError,
                        s => s.Write(pageBytes, 0, pageBytes.Length));
                }
                else
                {
                    x.Response = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(),
                        "SerpoServer.Errors", "500.html");
                }

                return e;
            });


            pipelines.BeforeRequest.InsertBefore("installBlock", c =>
            {
                if (!install) return null;

                if (c.Request.Path.Contains("/install/") || c.Request.Path.Contains("/static/"))
                    return null;


                c.Response = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(), "SerpoServer.Install",
                    "install.html");
                return c.Response;
            });
            install = string.IsNullOrWhiteSpace(ConfigurationProvider.ConfigurationFile.connstring);

            if (!install) Identity.Initialize(pipelines);
            _pipelines = pipelines;

            ShedulerBase.StartSheduler();
        }

        public static void DisableBlock()
        {
            install = false;
            Identity.Initialize(_pipelines);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.ViewLocationConventions.Add((s, d, c) => Path.Combine("AdminContent", "views", s));
            nancyConventions.StaticContentsConventions.AddDirectory("static/", "Static/");
            nancyConventions.StaticContentsConventions.AddDirectory("admin/static/", "AdminContent/Static/");
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