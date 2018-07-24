﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Community.CsharpSqlite;
using IronPython.Runtime.Operations;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Embedded.Conventions;
using Nancy.Extensions;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using PetaPoco;
using RazorEngine;
using RazorEngine.Templating;
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

           spo_site urlSite = null;
            if (context.IsAjaxRequest())
            {
                var key = context.Request.Headers.Authorization;
                if (key.Contains("/"))
                {
                    var site = int.Parse(key.Split("/").Last());
                    urlSite = SiteManager.GetSiteById(site);
                }
            }
            else
            {
                dynamic siteId = null;
                if (context.Request.Url.Path.Contains("/admin"))
                {
                    if(context.Request.Cookies.TryGetValue("site", out var site) && !string.IsNullOrWhiteSpace(site))
                        siteId = int.Parse(site);
                }
                else
                {
                    siteId = context.Request.Url.HostName;
                }

                if(siteId != null)
                    urlSite = siteId is int ? SiteManager.GetSiteById((int)siteId) : SiteManager.GetSiteByDom((string)siteId);
            }

            context.Items.Add("site", urlSite);

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
                        "SerpoServer.Errors", "500.html");
                }
                return e;
            });
            container.Register(typeof(IDatabase) ,typeof(Connection));
            pipelines.BeforeRequest.InsertBefore("installBlock",c =>
            {
                if (!install) return null;
     
                if (c.Request.Path.Contains("/install/") || c.Request.Path.Contains("/static/"))
                    return null;

                   
                c.Response = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(), "SerpoServer.Install", "install.html");
                return c.Response;
            });
            install = string.IsNullOrWhiteSpace(ConfigurationProvider.ConfigurationFile.connstring);

            if (!install)
            {
                Identity.Initialize(pipelines);
            }
            _pipelines = pipelines;
        }

        private static bool install;
       
        public static void DisableBlock()
        {
            
            install = false;
            Identity.Initialize(_pipelines);
            
        }

        protected override IRootPathProvider RootPathProvider => new RootPathProvider();

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.ViewLocationConventions.Add((s,d,c) => "AdminViews/views/" + s);
          
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