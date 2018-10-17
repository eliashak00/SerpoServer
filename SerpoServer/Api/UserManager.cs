using System;
using System.Net;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Database;
using SerpoServer.Security;

namespace SerpoServer.Api
{
    public class UserManager
    {
        private readonly IDatabase db = CallDb.GetDb();

        private bool SiteExists(int siteId)
        {
            return db.Exists<spo_site>("SELECT * FROM spo_sites WHERE site_id = @0", siteId);
        }

        public HttpStatusCode CreateOrEdit(spo_user user)
        {
            if (!SiteExists(user.user_site))
                return HttpStatusCode.BadRequest;

            if (!string.IsNullOrWhiteSpace(user.user_password))
            {
                var salt = Convert.ToBase64String(Hashing.RandomBytes());
                var hashedPsw = Hashing.SHA512(user.user_password, salt);
                user.user_password = hashedPsw;
                user.user_salt = salt;
            }

            if (user.user_id == 0)
                db.Insert(user);
            else
                db.Update(user);

            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            db.Delete<spo_user>("DELETE FROM spo_users WHERE user_id = @0", id);
            return HttpStatusCode.OK;
        }
    }
}