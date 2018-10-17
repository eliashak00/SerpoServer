using System;
using System.Reflection;
using System.Text;
using Nancy;
using Nancy.Extensions;
using Nancy.Responses;
using Newtonsoft.Json;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Database;
using SerpoServer.Intepreter;
using SerpoServer.Lib;

namespace SerpoServer.Api
{
    public class RequestManager
    {
        private readonly CrudManager crud;
        private readonly IDatabase db = CallDb.GetDb();
        private MemoryDatabase MemDB = new MemoryDatabase();
        private readonly PageManager pm;
        private readonly Python py;

        public RequestManager(PageManager pm, CrudManager cru, Python py)
        {
            this.pm = pm;
            crud = cru;
            this.py = py;
        }

        private void GenerateStatusCode(int code, NancyContext ctx, ref Response result)
        {
            var page = pm.GetPage(ctx.Request.Url.SiteBase, RequestMethods.Get, code.ToString());
            if (page == null)
                result = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(),
                    "SerpoServer.Errors", $"{code}.html");
            else
                page.page_response = ctx.IsAjaxRequest() ? ResponseMethods.Rest : ResponseMethods.View;

            Enum.TryParse<HttpStatusCode>(code.ToString(), out var status);
            result.StatusCode = status;
        }


        public Response GenerateResponse(NancyContext ctx, RequestMethods method)
        {
            var result = new Response();
            var path = ctx.Request.Url.Path.ToLower();
            var domain = ctx.Request.Url.HostName;
            var page = string.IsNullOrWhiteSpace(path) ? pm.GetPage(domain, method) : pm.GetPage(domain, method, path);
            if (page == null)
            {
                GenerateStatusCode(404, ctx, ref result);
                goto ReturnPoint;
            }

            if (page.page_response == ResponseMethods.Crud)
            {
                var crud = db.FirstOrDefault<spo_crud>(
                    "SELECT crud_auth, crud_password, crud_table, crud_json, crud_struct FROM spo_cruds WHERE crud_id = @0",
                    page.page_crud);
                if (crud == null) goto ReturnPoint;
                if (crud.crud_auth)
                    if (crud.crud_password != ctx.Request.Headers.Authorization)
                    {
                        GenerateStatusCode(401, ctx, ref result);
                        goto ReturnPoint;
                    }

                var data = ctx.Request.Body.AsString();
                var urlId = ctx.Request.Query.id;
                switch (page.page_methods)
                {
                    case RequestMethods.Post:
                        this.crud.InsertOrUpdate(crud, data);
                        break;
                    case RequestMethods.Get:
                        this.crud.Get(crud, urlId);
                        break;
                    case RequestMethods.Put:
                        break;
                    case RequestMethods.Delete:
                        this.crud.Delete(crud, urlId);
                        break;
                    default:
                        GenerateStatusCode(404, ctx, ref result);
                        goto ReturnPoint;
                }
            }
            else
            {
                try
                {
                    object model = null;
                    if (page.page_script != null)
                    {
                        var scriptResult = py.Execute<dynamic>(page.page_script);
                        if (scriptResult != null) model = scriptResult;
                    }

                    if (page.page_response == ResponseMethods.View && !string.IsNullOrWhiteSpace(page.page_view))
                    {
                        var viewString = new ViewEngineConverter().Render(page.page_view, model);
                        var bytes = Encoding.UTF8.GetBytes(viewString);
                        result.Contents = s => s.Write(bytes, 0, bytes.Length);
                        result.ContentType = "text/html";
                    }
                    else
                    {
                        result.ContentType = "application/json";
                        if (model != null)
                        {
                            var json = JsonConvert.SerializeObject(model);
                            var bytes = Encoding.UTF8.GetBytes(json);
                            result.Contents = s => s.Write(bytes, 0, bytes.Length);
                        }
                        else
                        {
                            result.Contents = null;
                        }

                        result.StatusCode = HttpStatusCode.OK;
                    }
                }
                catch
                {
                    GenerateStatusCode(500, ctx, ref result);
                }
            }

            ReturnPoint:
            return result;
        }
    }
}