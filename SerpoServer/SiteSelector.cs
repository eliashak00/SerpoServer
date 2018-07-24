using System;
using System.Collections.Generic;
using System.Reflection;
using Community.CsharpSqlite;
using Nancy;
using Nancy.Extensions;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using Nancy.TinyIoc;
using RazorEngine;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;
using RazorEngine.Templating;
using SerpoServer.Api;
using SerpoServer.Data.Models;
using Encoding = System.Text.Encoding;

namespace SerpoServer
{
    public static class SiteSelector
    {
        public static spo_site GetSite(this NancyContext ctx)
        {
            if (ctx.Items.TryGetValue("site", out var site))
            {
                return (spo_site) site;
            }

            return null;
        }

        public static void RequiresSite(this INancyModule mod)
        {
           
            mod.AddBeforeHookOrExecute(x =>
            {
                if (x.GetSite() != null) return null;
               return new RedirectResponse("/admin/site");
            });
         
        }

    }
}