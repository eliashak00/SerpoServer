using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.Extensions;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;

namespace SerpoServer.Errors
{
    public class StatusCodes : IStatusCodeHandler
    {
        private static IEnumerable<int> _checks = new List<int>()
        {
            401,
            404,
            500
        };



        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return (_checks.Any(x => x == (int) statusCode));
        }

        public static void AddCode(int code)
        {
            AddCode(new List<int>() {code});
        }

        private static void AddCode(IEnumerable<int> code)
        {
            _checks = _checks.Union(code);
        }

        public static void RemoveCode(int code)
        {
            RemoveCode(new List<int>() { code });
        }
        public static void RemoveCode(IEnumerable<int> code)
        {
            _checks = _checks.Except(code);
        }

        public static void Disable()
        {
            _checks = new List<int>();
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            try
            {
                if (context.IsAjaxRequest())
                {
                    context.Response.StatusCode = statusCode;
                }
                else
                {
                    var response = new EmbeddedFileResponse(Assembly.GetExecutingAssembly(),
                        "SerpoServer.Errors", ((int) statusCode) + ".html") {StatusCode = statusCode};
                    context.Response = response;
                }
            }
            catch (Exception)
            {

                RemoveCode((int)statusCode);
                context.Response.StatusCode = statusCode;
            }
        }
    }
}