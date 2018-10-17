using System.Reflection;
using Nancy;
using SerpoServer.Api;
using SerpoServer.Data.Models.Enums;

namespace SerpoServer.Routes
{
    public class DynamicModule : NancyModule
    {
        private Assembly asm = typeof(DynamicModule).Assembly;

        public DynamicModule(RequestManager rm)
        {
            Get("/", x =>
            {
                var path = (string) x.path;

                return rm.GenerateResponse(Context, RequestMethods.Get);
            });
            Post("/", x =>
            {
                var path = (string) x.path;
                return rm.GenerateResponse(Context, RequestMethods.Post);
            });
            Put("/", x =>
            {
                var path = (string) x.path;
                return rm.GenerateResponse(Context, RequestMethods.Put);
            });
            Delete("/", x =>
            {
                var path = (string) x.path;
                return rm.GenerateResponse(Context, RequestMethods.Delete);
            });
            Get("/*", x =>
            {
                var path = (string) x.path;
                return rm.GenerateResponse(Context, RequestMethods.Get);
            });
            Post("/*", x =>
            {
                var path = (string) x.path;
                return rm.GenerateResponse(Context, RequestMethods.Post);
            });
            Put("/*", x =>
            {
                var path = (string) x.path;
                return rm.GenerateResponse(Context, RequestMethods.Put);
            });
            Delete("/*", x =>
            {
                var path = (string) x.path;
                return rm.GenerateResponse(Context, RequestMethods.Delete);
            });
        }
    }
}