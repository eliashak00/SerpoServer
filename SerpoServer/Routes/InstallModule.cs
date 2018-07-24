using System;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Extensions;
using Nancy.Json.Simple;
using Nancy.ModelBinding;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using SerpoServer.Data;
using SerpoServer.Data.Models;
using SerpoServer.Intepreter;
using SerpoServer.Security;

namespace SerpoServer.Routes
{
    public class InstallModule : NancyModule
    {
        public InstallModule() : base("/install")
        {

            
            Post("/step1", x =>
            {
                var db = SimpleJson.DeserializeObject<dbRequest>(Request.Body.AsString());
                if (db == null) return HttpStatusCode.BadRequest;
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
                var usr = JsonConvert.DeserializeObject<usrRequest>(Request.Body.AsString());
                if (usr == null) return HttpStatusCode.BadRequest;
                if (usr.psw != usr.confpsw) return HttpStatusCode.BadRequest;
                
                var usrObj = new spo_user
                {
                    user_email = usr.email,
                    user_nick = "John Doe",
                    user_password = usr.psw,
                    user_registerd = DateTime.Now
                };
                
                Identity.CreateOrEdit(usrObj);
            
                return HttpStatusCode.OK;
            });
        }

        class dbRequest
        {
            public string dbtype;
            public string dbhost;
            public string dbname;
            public string dbpsw;
            public string dbport;
            public string dbuser;
        }

        class usrRequest
        {
            public string email;
            public string psw;
            public string confpsw;
        }
        private bool DbExists(string connStr, string type)
        {
            DbConnection conn = null;
            switch (type)
            {
                case "mssql":
                    conn = new SqlConnection(connStr);

                    break;
                case "mysql":
                    conn = new MySqlConnection(connStr);
                    break;
            }

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
    }
    
}