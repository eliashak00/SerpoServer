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
using Jose;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Extensions;
using PetaPoco;
using SerpoCMS.Core.Security;
using SerpoServer.Data.Models;
using SerpoServer.Database;

namespace SerpoServer.Security
{
    public static class Identity
    {
        private const string MEMDB_NAME = "userCache";

        private static readonly IList<Tuple<string, string, byte[]>> CurrentUsers =
            new List<Tuple<string, string, byte[]>>();

        private static readonly MemoryDatabase MemoryDatabase = new MemoryDatabase();
        private static IDatabase Db => CallDb.GetDb();


        public static IEnumerable<spo_user> GetAll
        {
            get
            {
                var data = Db.Query<spo_user>("SELECT * FROM spo_users").ToList();
                data.ForEach(u => u.user_password = null);
                data.ForEach(u => u.user_salt = null);
                return data;
            }
        }

        public static void Initialize(IPipelines pipelines)
        {
            try
            {
                var memDb = MemoryDatabase.Current.GetCollection<spo_user>(MEMDB_NAME);
                if (Db.FirstOrDefault<spo_user>("SELECT * FROM spo_users") != null)
                    foreach (var usr in Db.Query<spo_user>("SELECT * FROM spo_users"))
                        memDb.Insert(usr.user_email, usr);
            }
            catch
            {
            }

            var config = new StatelessAuthenticationConfiguration(x =>
            {
                var token = "";
                if (x.Request.IsAjaxRequest())
                {
                    token = x.Request.Headers.Authorization;
                }
                else
                {
                    if (x.Request.Cookies.TryGetValue("auth", out var cookie))
                        token = cookie;
                }

                if (string.IsNullOrWhiteSpace(token)) return null;

                string key;
                if (token.Contains('/'))
                {
                    var tokenObj = token.Split('/');
                    key = tokenObj.First();
                }
                else
                {
                    key = token;
                }


                return GetUserClaim(key);
            });
            StatelessAuthentication.Enable(pipelines, config);
        }


        public static string Login(string email, string password)
        {
            var user = MemoryDatabase.Current.GetCollection<spo_user>(MEMDB_NAME).FindById(email) ??
                       Db.FirstOrDefault<spo_user>("SELECT * FROM spo_users WHERE user_email = @0", email);
            if (user == null) return null;
            var hashedPassword = Hashing.SHA512(password, user.user_salt);
            if (user.user_password != hashedPassword) return null;
            var key = Guid.NewGuid().ToString();
            var exp = System.DateTime.Now.AddHours(ConfigurationProvider.ConfigurationFile.logoutHours);
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
                    if (System.DateTime.Parse(token.Exp).CompareTo(System.DateTime.Now) >
                        ConfigurationProvider.ConfigurationFile.logoutHours) continue;

                    return MemoryDatabase.Current.GetCollection<spo_user>("userCache").FindById(token.Email);
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

        public static void Logout(string key)
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

        public static void CreateOrEdit(spo_user info)
        {
            var storedUser = info.user_id > 0
                ? MemoryDatabase.Current.GetCollection<spo_user>(MEMDB_NAME).FindById(info.user_email)
                : info;
            if (info.user_password != null)
            {
                var salt = Convert.ToBase64String(Hashing.RandomBytes());
                storedUser.user_password = Hashing.SHA512(info.user_password, salt);
                storedUser.user_salt = salt;
            }

            storedUser.user_avatar = info.user_avatar ?? "stockavatar.jpg";
            if (info.user_nick != null)
                storedUser.user_nick = info.user_nick;

            if (info.user_email != null)
                storedUser.user_email = info.user_email;

            if (info.user_id > 0)
            {
                Db.Update(storedUser);
            }
            else
            {
                storedUser.user_registerd = System.DateTime.Now;
                Db.Insert("spo_users", storedUser);
            }
        }

        public static void Delete(int id)
        {
            Db.Delete<spo_user>("DELETE FROM spo_users WHERE user_id = @0", id);
        }


        public static spo_user GetUserById(int id)
        {
            return Db.Single<spo_user>("SELECT * FROM spo_users WHERE user_id = @0",
                id);
        }
    }
}