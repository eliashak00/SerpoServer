using System.Linq;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Nancy.Security;
using Newtonsoft.Json;
using SerpoServer.Api;
using SerpoServer.Data.Models;
using SerpoServer.Security;

namespace SerpoServer.Routes.Admin
{
    public class AccountModule : NancyModule
    {
        public AccountModule(UserManager usr) : base("/admin/account")
        {
            this.RequiresAuthentication();
            this.RequiresSite();
            Get("/", x => View["account.html", new{Users = Identity.GetAll}]);
            Get("/createoredit", x =>
            {
                var id = (string) Request.Query.user;
                return id == null ? View["account-edit.html", new spo_user()] : View["account-edit.html", Identity.GetUserById(int.Parse(id))];
            });
            Post("/createoredit", x =>
            {
                var user = JsonConvert.DeserializeObject<spo_user>(Request.Body.AsString());
                if(user == null) return HttpStatusCode.BadRequest;
                return usr.CreateOrEdit(user);     
            });
            Get("/email/{email}", x =>
            {
                var email = (string) x.email;
                return Identity.GetAll.FirstOrDefault(e => e.user_email == email) == null
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.OK;
            });
            Delete("/remove/{id}", x =>
            {
                if(x.id == null)
                    return HttpStatusCode.BadRequest;
                return usr.Delete((int) x.id);
            });
            Delete("/logout", x =>
            {
                Identity.Logout(Request.Headers.Authorization);
                return HttpStatusCode.OK;
            });
        }
    }
}