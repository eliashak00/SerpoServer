using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using SerpoServer.Data.Models;
using SerpoServer.Security;

namespace SerpoServer.Routes.Admin
{
    public class AccountModule : NancyModule
    {
        public AccountModule(Identity idt) : base("/admin/account")
        {
            this.RequiresAuthentication();
            Get("/", x => View["admin/views/account.html", idt.GetAll]);
            Post("/createoredit", x =>
            {
                var user = this.BindAndValidate<spo_user>();
                if(user == null) return HttpStatusCode.BadRequest;
                idt.CreateOrEdit(user);
                return HttpStatusCode.OK;
            });
            Delete("/remove/{id}", x =>
            {
                if(x.id == null)
                    return HttpStatusCode.BadRequest;
                idt.Delete((int)x.id);
                return HttpStatusCode.OK;
            });
            Delete("/logout", x =>
            {
                idt.Logout(Request.Headers.Authorization);
                return HttpStatusCode.OK;
            });
        }
    }
}