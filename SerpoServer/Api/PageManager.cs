using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Nancy;
using Nancy.Extensions;
using Nancy.Json.Simple;
using Nancy.Responses;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using PetaPoco;
using RazorEngine;
using RazorEngine.Templating;
using SerpoServer.Data;
using SerpoServer.Data.Cache;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Intepreter;
using Encoding = System.Text.Encoding;

namespace SerpoServer.Api
{
    public class PageManager
    {
        private IDatabase db;
        private PyRuntime python;
        private CrudManager crud;
        private static IList<Tuple<string, RequestMethods, spo_page>> PageCache = new List<Tuple<string, RequestMethods, spo_page>>();
        
        public PageManager(PyRuntime python, Connection db, CrudManager crud)
        {
            this.python = python;
            this.db = db;
            this.crud = crud;

        }
        public IEnumerable<spo_page> GetPages(string domain) =>
            db.Query<spo_page>(
                "SELECT * FROM spo_pages INNER JOIN spo_sites ON spo_sites.site_id = spo_pages.page_site WHERE spo_sites.site_domain = @0",
                domain);
        
        public IEnumerable<spo_page> GetPages(string domain, RequestMethods method) =>
            db.Query<spo_page>(
                "SELECT * FROM spo_pages INNER JOIN spo_sites ON spo_sites.site_id = spo_pages.page_site WHERE spo_sites.site_domain = @0 AND spo_pages.page_methods = @1",
                domain, method);
        public spo_page GetPage(string domain, RequestMethods method, string path = "/") =>  db.SingleOrDefault<spo_page>("SELECT * FROM spo_pages LEFT JOIN spo_sites ON site_id = page_site WHERE site_domain = @0 AND page_route = @1 AND page_methods = @2",
            domain, path, method);

        public spo_page GetPage(int id) => db.SingleOrDefault<spo_page>(
            "SELECT * FROM spo_pages WHERE page_id = @0", id);
        public HttpStatusCode CreateOrEdit(spo_page data)
        {
            if (data.page_script != null)
            {
                var compiled = python.Compile(data.page_script);
            }

            if (data.page_id == 0)
                db.Insert(data);
            else
                db.Update(data);
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            var page = db.SingleOrDefault<spo_page>("SELECT * FROM spo_pages WHERE page_id = @0", id);
            if (page == null) return HttpStatusCode.BadRequest;
            db.Delete(page);
            return HttpStatusCode.OK;
        }

        public Response GenereateResponse(NancyContext ctx, string domain, RequestMethods method, string path)
        {
            var result = new Response();
   
            var page = string.IsNullOrWhiteSpace(path)? GetPage(domain, method) : GetPage(domain,method, "/" + path);
            
            if(page == null)
            {               
                page = GetPage(domain, RequestMethods.Get, "NOTFOUND");
                if(page == null)
                    result = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(),
                        "SerpoServer.Errors", "404.html");
                else
                    page.page_response = ResponseMethods.View;
                
                result.StatusCode = HttpStatusCode.NotFound;
                goto ReturnPoint;
            }
            if(page.page_response == ResponseMethods.Crud){
                var crud = db.FirstOrDefault<spo_crud>("SELECT crud_auth, crud_password, crud_table, crud_json, crud_struct FROM spo_cruds WHERE crud_id = @0", page.page_crud);
                if (crud == null) goto ReturnPoint;
                if(crud.crud_auth){
                    if (crud.crud_password != ctx.Request.Headers.Authorization){
                        result.StatusCode = HttpStatusCode.Unauthorized;
                        goto ReturnPoint;
                    }
                }
                var data = ctx.Request.Body.AsString();
                var urlId = ctx.Request.Query.id;
                switch(page.page_methods){
                    case RequestMethods.Post:
                        this.crud.Insert(crud, data);
                    break;
                    case RequestMethods.Get:
                        this.crud.Get(crud, urlId);
                    break;
                    case RequestMethods.Put:
                    break;
                    case RequestMethods.Delete:
                        this.crud.Delete(crud, urlId);
                    break;
                }

            }

     



            if (page.page_response == ResponseMethods.View && !string.IsNullOrWhiteSpace(page.page_view))
            {
                string response = null;
                if (!string.IsNullOrWhiteSpace(page.page_script))
                {
                    var scriptResult = python.Compile(page.page_script).Execute<object>();;
                   
                    response = Engine.Razor.RunCompile(page.page_view, typeof(object),scriptResult);
                }
                else
                {

                    response = page.page_view;
                }
                var bytes = Encoding.UTF8.GetBytes(response);
                result.Contents = s => s.Write(bytes, 0, bytes.Length);
                result.ContentType = "text/html";

            }
            else
            {
                var scriptResult = python.Execute<object>(page.page_script);
                var json = JsonConvert.SerializeObject(scriptResult);
                var bytes = Encoding.UTF8.GetBytes(json);
                result.ContentType = "application/json";
                result.Contents = s => s.Write(bytes, 0, bytes.Length);

            }
            ReturnPoint:
                return result;
        }
}
}