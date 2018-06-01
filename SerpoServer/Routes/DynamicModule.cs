using Nancy;
using PetaPoco;
using SerpoServer.Api;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Intepreter;

namespace SerpoServer.Routes
{
    public class DynamicModule : NancyModule
    {
        public DynamicModule(PageManager pm)
        {
            Get("/{path}", x =>
            {
                var dom = Request.Url.HostName;
                var path = (string) x.path;
                return pm.GenereateResponse(dom, RequestMethods.Get, path);
            });
            Post("/{path}", x =>
            {
                var dom = Request.Url.HostName;
                var path = (string) x.path;
                return pm.GenereateResponse(dom , RequestMethods.Post, path);
            });
            Put("/{path}", x =>
            {
                var dom = Request.Url.HostName;
                var path = (string) x.path;
                return pm.GenereateResponse(dom , RequestMethods.Put, path);
            });
            Delete("/{path}", x =>
            {
                var dom = Request.Url.HostName;
                var path = (string) x.path;
                return pm.GenereateResponse(dom , RequestMethods.Delete, path);
            });
        }
    }
}