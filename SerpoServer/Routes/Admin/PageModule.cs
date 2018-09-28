﻿using System;
using System.Collections.Generic;
using System.Linq;
using Community.CsharpSqlite;
using Google.Protobuf.WellKnownTypes;
using Nancy;
using Nancy.Extensions;
using Nancy.Json.Simple;
using Nancy.ModelBinding;
using Nancy.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SerpoServer.Api;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using Enum = System.Enum;

namespace SerpoServer.Routes.Admin
{
    public class PageModule : NancyModule
    {
        public PageModule(PageManager pge, CrudManager cr) : base("/admin/pages")
        {
            this.RequiresAuthentication();
            this.RequiresSite();
            Get("/", x =>
            {
                var sort = (string)Request.Query.sort;
                if (sort == "crud")
                {
                    return View["pages.html", new {Cruds = cr.AllModules, Pages = Enumerable.Empty<spo_page>()}];
                }
                else
                {
                    var pages = sort != null? pge.GetPages(Context.GetSite().site_domain, Enum.Parse<RequestMethods>(sort, true)) : pge.GetPages(Context.GetSite().site_domain);
                    return View["pages.html", new {Cruds = Enumerable.Empty<spo_crud>(), Pages = pages}];
                }
                
            });
            Get("/data/{id}", x =>
            {
                var data = cr.GetModule((int) x.id);
                return View["crud-data.html", new {Data = data}];
            });
            Get("/createoredit", x =>
            {
                if (Request.Query.type == "crud")
                {
                    return Request.Query.page == null ? View["page-crud.html", new spo_crud()] : View["page-crud.html", cr.GetModule((int) Request.Query.page)];
                }
                else
                {
                    return Request.Query.page != null
                        ? View["page-editor.html", pge.GetPage((int) Request.Query.page)]
                        : View["page-editor.html", new spo_page()];
                }

         
            });
            Post("/createoredit", x =>
            {
                if (Request.Query.type == "crud")
                {
                   var crud = JsonConvert.DeserializeObject<spo_crud>(Request.Body.AsString());
                   return cr.CreateOrEdit(crud);
                }
                else
                {
                    
               
                var page = JsonConvert.DeserializeObject<spo_page>(Request.Body.AsString());
                page.page_site = Context.GetSite().site_id;
                return pge.CreateOrEdit(page);
                }
            });
            Delete("/delete/{id}", x =>
            {
                var id = (int) x.id;
                return Request.Query.type == "crud" ? cr.Delete(id) : pge.Delete(id);
            });
            Get("/route", x =>
            {
                var route = (string) Request.Query.path;
          
                return pge.GetPages(Context.GetSite().site_domain).FirstOrDefault(s => s.page_route == route) != null
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.OK;
            });
        }
    }
}