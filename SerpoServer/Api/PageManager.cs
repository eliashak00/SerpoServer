using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Encodings.Web;
using Nancy;
using Nancy.Json.Simple;
using Nancy.Responses;
using Nancy.TinyIoc;
using PetaPoco;
using RazorEngine;
using RazorEngine.Templating;
using SerpoServer.Data;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Intepreter;
using Encoding = System.Text.Encoding;

namespace SerpoServer.Api
{
    public class PageManager
    {
        private IDatabase db => TinyIoCContainer.Current.Resolve<Connection>();
        private PyRuntime python;
        public PageManager(PyRuntime python)
        {
            this.python = python;

        }
        public IEnumerable<spo_page> GetPages(string domain) =>
            db.Query<spo_page>(
                "SELECT * FROM spo_pages INNER JOIN spo_sites ON spo_sites.site_id = spo_pages.page_site WHERE spo_sites.site_domain = @0",
                domain);

        public spo_page GetPage(string domain, RequestMethods method, string path = "/") => db.SingleOrDefault<spo_page>(
            "SELECT * FROM spo_pages INNER JOIN spo_sites ON spo_sites.site_id = spo_pages.page_site WHERE spo_sites.site_domain = @0 AND spo_pages.page_route = @1 AND page_method = @2",
            domain, path, method);

        public spo_page GetPage(int id) => db.SingleOrDefault<spo_page>(
            "SELECT * FROM spo_pages WHERE page_id = @0", id);
        public HttpStatusCode CreateOrEdit(spo_page data)
        {
            if (data.page_script != null)
            {
                var compiled = python.Compile(data.page_script);
            }
            db.Insert("spo_pages", data);
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            var page = db.SingleOrDefault<spo_page>("SELECT * FROM spo_pages WHERE page_id = @0", id);
            if (page == null) return HttpStatusCode.BadRequest;
            db.Delete(page);
            return HttpStatusCode.OK;
        }

        public Response GenereateResponse(string domain, RequestMethods method, string path)
        {
            var result = new Response();
            var page = GetPage(domain,method, path);
            if(page == null)
            {               
                page = GetPage(domain, RequestMethods.Get, "NOTFOUND");
                if(page == null)
                    result = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(),
                        "SerpoServer.Errors", "404.html");
                else
                    page.page_resposne = ResponseMethods.View;
                
                result.StatusCode = HttpStatusCode.NotFound;
                goto ReturnPoint;
            }
            else
            {
                if (page.page_methods != method)
                {
                    result.StatusCode = HttpStatusCode.MethodNotAllowed;
                }

            }

     



            if (page.page_resposne == ResponseMethods.View)
            {
                string response = null;
                if (page.page_script != null)
                {
                    var scriptResult = python.Execute<dynamic>(page.page_script);
                    if (scriptResult is object)
                    {
                        response = Engine.Razor.RunCompile(page.page_view, typeof(object), (object) scriptResult);
                        
                    }
                }
                else
                {
                    response = Engine.Razor.RunCompile(page.page_view, typeof(object), null);
                }
                var bytes = Encoding.UTF8.GetBytes(response);
                result.Contents = s => s.Write(bytes, 0, bytes.Length);
                result.ContentType = "text/html";

            }
            else
            {
                var scriptResult = python.Execute<object>(page.page_script);
                var json = SimpleJson.SerializeObject(scriptResult);
                var bytes = Encoding.UTF8.GetBytes(json);
                result.ContentType = "application/json";
                result.Contents = s => s.Write(bytes, 0, bytes.Length);

            }
            ReturnPoint:
                return result;
        }
}
}