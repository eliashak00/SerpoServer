// ################################################
// ##@project SerpoCMS.Core
// ##@filename Identity.cs
// ##@author Elias Håkansson
// ##@license MIT - License(see license.txt)
// ################################################

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using IronPython.SQLite;
using Jose;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Extensions;
using PetaPoco;
using SerpoCMS.Core.Security;
using SerpoServer.Data.Cache;
using SerpoServer.Data.Models;

namespace SerpoServer.Security
{
    public class Identity
    {
        private const int logoutTimeLimit = 2;
        private IDatabase db;
        private static readonly IList<Tuple<string, string, byte[]>> CurrentUsers =
            new List<Tuple<string, string, byte[]>>();

        public Identity(IDatabase db)
        {
            this.db = db;
        }
        public void Initialize(IPipelines pipelines)
        {
            
            try
            {
                if (db.FirstOrDefault<spo_user>("SELECT * FROM spo_users") != null)
                {
                    var memDb = MemoryDatabase.Current.GetCollection<spo_user>("userCache");
                    var users = db.Fetch<spo_user>("SELECT * FROM spo_users");
                    memDb.InsertBulk(users);
                }
            }
            catch
            {
            }

            var config = new StatelessAuthenticationConfiguration(x =>
            {
                var token = x.Request.IsAjaxRequest() ? x.Request.Headers.Authorization : (string) x.Request.Query.user;
                if (string.IsNullOrWhiteSpace(token))
                {
                    return null;
                }
                else
                {
                    var claim = GetUserClaim(token);
                    
                    if ((string) x.Request.Query.site == null || claim == null)
                    {
                        x.Response.Headers["Location"] = "/admin";
                        return null;
                    }
                
                    return claim;
                }
            });
            StatelessAuthentication.Enable(pipelines, config);
        }
        

        public IEnumerable<spo_user> GetAll
        {
            get
            {
                var data = db.Query<spo_user>("SELECT * FROM spo_users").ToList();
                data.ForEach(u => u.user_password = null);
                data.ForEach(u => u.user_salt = null);
                return data;
            }
        }


        public string Login(string email, string password)
        {
            var user = MemoryDatabase.Current.GetCollection<spo_user>("userCache").FindOne(u => u.user_email == email) ??
                       db.FirstOrDefault<spo_user>("SELECT * FROM spo_users WHERE user_email = @0", email);
            if (user == null) return null;
            var hashedPassword = Hashing.SHA512(password, user.user_salt);
            if (user.user_password != hashedPassword) return null;
            var key = Guid.NewGuid().ToString();
            var exp = DateTime.Now.AddHours(logoutTimeLimit);
            var objPayload = new Dictionary<string, object>
            {
                {"Email", user.user_email},
                {"Exp", exp},
                {"Key", key}
            };
            var secretKey = Hashing.RandomBytes(32);
            CurrentUsers.Add(new Tuple<string, string, byte[]>(user.user_email, key, secretKey));
            return JWT.Encode(objPayload, secretKey, JweAlgorithm.DIR, JweEncryption.A128CBC_HS256);
        }


        private static spo_user GetUser(string key)
        {
            foreach (var user in CurrentUsers)
                try
                {
                    var token = JWT.Decode<JwtToken>(key, user.Item3, JweAlgorithm.DIR, JweEncryption.A128CBC_HS256);
                    if (DateTime.Parse(token.Exp).CompareTo(DateTime.Now) >
                        logoutTimeLimit) continue;
                    return MemoryDatabase.Current.GetCollection<spo_user>("userCache").FindOne(u => u.user_email == token.Email);
                }
                catch
                {
                }

            return null;
        }

        public static ClaimsPrincipal GetUserClaim(string key)
        {
            var user = GetUser(key);
            return user == null ? null : new ClaimsPrincipal(new ClaimsIdentity(user.user_email));
        }

        public void Logout(string key)
        {
            foreach (var user in CurrentUsers)
                try
                {
                    var token = JWT.Decode<JwtToken>(key, user.Item3, JweAlgorithm.DIR, JweEncryption.A128CBC_HS256);
                    if (user.Item2 == token.Key && user.Item1 == token.Email)
                        CurrentUsers.Remove(user);
                }
                catch
                {
                }
        }

        public void CreateOrEdit(spo_user info)
        {
            var storedUser = info.user_id > 0
                ? MemoryDatabase.Current.GetCollection<spo_user>("userCache").FindOne(u => u.user_email == info.user_email)
                : new spo_user();

            if (info.user_password != null)
            {
                storedUser.user_password = Hashing.SHA512(info.user_password);
                storedUser.user_salt = Convert.ToBase64String(Hashing.RandomBytes());
            }

            storedUser.user_avatar = info.user_avatar ?? "stockavatar.jpg";

            if (info.user_nickname != null)
                storedUser.user_nickname = info.user_nickname;

            if (info.user_email != null)
                storedUser.user_email = info.user_email;

            if (info.user_id > 0)
            {
                db.Update(storedUser);
            }
            else
            {
                storedUser.user_registerd = DateTime.Now;

                db.Insert("spo_users", storedUser);
            }
        }

        public void Delete(int id)
        {
            db.Delete<spo_user>("DELETE FROM spo_users WHERE user_id = @0", id);
        }


        public spo_user GetUserById(int id)
        {
            return db.Single<spo_user>("SELECT * user_id, user_email, user_avatar FROM spo_users WHERE user_id = @0",
                id);
        }
    }
}