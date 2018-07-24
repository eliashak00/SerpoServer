using System.Linq;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Nancy.Security;
using Newtonsoft.Json;
using SerpoServer.Data.Models;
using SerpoServer.Security;

namespace SerpoServer.Routes.Admin
{
    public class AccountModule : NancyModule
    {
        public AccountModule() : base("/admin/account")
        {
            this.RequiresAuthentication();
            Get("/", x => View["account.html", Identity.GetAll]);
            Post("/createoredit", x =>
            {
                var user = JsonConvert.DeserializeObject<spo_user>(Request.Body.AsString());
                if(user == null) return HttpStatusCode.BadRequest;
                Identity.CreateOrEdit(user);
                return HttpStatusCode.OK;
            });
            Delete("/remove/{id}", x =>
            {
                if(x.id == null)
                    return HttpStatusCode.BadRequest;
                Identity.Delete((int)x.id);
                return HttpStatusCode.OK;
            });
            Delete("/logout", x =>
            {
                Identity.Logout(Request.Headers.Authorization);
                return HttpStatusCode.OK;
            });
        }
    }
}