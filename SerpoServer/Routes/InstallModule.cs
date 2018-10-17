using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Nancy;
using Nancy.Extensions;
using Nancy.Json.Simple;
using Newtonsoft.Json;
using SerpoServer.Data.Models;
using SerpoServer.Security;

namespace SerpoServer.Routes
{
    public class InstallModule : NancyModule
    {
        public InstallModule() : base("/install")
        {
            Post("/step1", x =>
            {
                var db = SimpleJson.DeserializeObject<DbRequest>(Request.Body.AsString());
                if (db.Equals(default(DbRequest))) return HttpStatusCode.BadRequest;
                string connectionString;
                switch (db.dbtype)
                {
                    case "mysql":
                        var mysql = new MySqlConnectionStringBuilder
                        {
                            Password = db.dbpsw,
                            UserID = db.dbuser,
                            Server = db.dbhost,
                            Port = uint.Parse(db.dbport),
                            Database = db.dbname
                        };
                        connectionString = mysql.ConnectionString;
                        break;
                    case "mssql":
                        var mssql = new SqlConnectionStringBuilder
                        {
                            Password = db.dbpsw,
                            UserID = db.dbuser,
                            DataSource = db.dbhost + "," + db.dbport,
                            InitialCatalog = db.dbname
                        };
                        connectionString = mssql.ConnectionString;
                        break;
                    default:
                        return null;
                }

                if (!DbExists(connectionString, db.dbtype)) return HttpStatusCode.BadRequest;
                var conf = ConfigurationProvider.ConfigurationFile;
                conf.connstring = connectionString;
                conf.dbtype = db.dbtype;
                ConfigurationProvider.UpdateFile(conf);

                Bootstrap.DisableBlock();
                return HttpStatusCode.OK;
            });
            Post("/step2", x =>
            {
                var usr = JsonConvert.DeserializeObject<UsrRequest>(Request.Body.AsString());
                if (usr.Equals(default(UsrRequest))) return HttpStatusCode.BadRequest;
                if (usr.psw != usr.confpsw) return HttpStatusCode.BadRequest;

                var usrObj = new spo_user
                {
                    user_email = usr.email,
                    user_nick = "John Doe",
                    user_password = usr.psw,
                    user_registerd = System.DateTime.Now
                };

                Identity.CreateOrEdit(usrObj);

                return HttpStatusCode.OK;
            });
        }

        private bool DbExists(string connStr, string type)
        {
            DbConnection conn = null;
            if (type == "mssql")
                conn = new SqlConnection(connStr);
            else if (type == "mysql") conn = new MySqlConnection(connStr);

            if (conn == null) return false;
            try
            {
                conn.Open();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Dispose();
            }
        }

        private class DbRequest
        {
            public string dbhost;
            public string dbname;
            public string dbport;
            public string dbpsw;
            public string dbtype;
            public string dbuser;
        }

        private class UsrRequest
        {
            public string confpsw;
            public string email;
            public string psw;
        }
    }
}