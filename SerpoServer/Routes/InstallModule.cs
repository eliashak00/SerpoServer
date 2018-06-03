using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ModelBinding;
using Nancy.TinyIoc;
using SerpoServer.Data;
using SerpoServer.Data.Models;
using SerpoServer.Intepreter;
using SerpoServer.Security;

namespace SerpoServer.Routes
{
    public class InstallModule : NancyModule
    {
        public InstallModule(TinyIoCContainer container) : base("/install")
        {
            Get("/", x => View["install.html"]);

            Post("/step1", x =>
            {
                var db = this.Bind<dbRequest>();
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

                var conf = ConfigurationProvider.ConfigurationFile;
                conf.connstring = connectionString;
                conf.dbtype = db.dbtype;
                ConfigurationProvider.UpdateFile(conf);
                container.Register<PyRuntime>();
                return HttpStatusCode.OK;
                
            });
            Post("/step2", x =>
            {
                var usr = this.Bind<usrRequest>();
                if (usr == null) return HttpStatusCode.BadRequest;
                if (usr.psw != usr.confpsw) return HttpStatusCode.BadRequest;
                var ident = container.Resolve<Identity>();
                var hash = Convert.ToBase64String(Hashing.RandomBytes());
                var hashedPsw = Hashing.SHA512(usr.psw, hash);
                var usrObj = new spo_user
                {
                    user_email = usr.email,
                    user_password = hashedPsw,
                    user_salt = hash,
                    user_nickname = "John Doe",
                    user_registerd = DateTime.Now
                };
                ident.CreateOrEdit(usrObj);
                Bootstrap.DisableBlock();
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
    }
}