using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using MySql.Data.MySqlClient;
using Nancy.Extensions;
using PetaPoco;
using PetaPoco.Core;
using PetaPoco.Providers;

namespace SerpoServer.Database
{
    public class Connection : IDatabase
    {
        private readonly IDatabase db;

        public Connection()
        {
            var script = "";
            IDatabase result;
            if (ConfigurationProvider.ConfigurationFile.dbtype == "mysql")
            {
                result = DatabaseConfiguration.Build()
                    .UsingProvider<MySqlDatabaseProvider>()
                    .UsingConnectionString(ConfigurationProvider.ConfigurationFile.connstring)
                    .UsingCommandTimeout(10)
                    .Create();
                script = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("SerpoServer.Database.serposerver.sql")
                    .AsString();
            }
            else
            {
                result = DatabaseConfiguration.Build()
                    .UsingProvider<SqlServerDatabaseProvider>()
                    .UsingConnectionString(ConfigurationProvider.ConfigurationFile.connstring)
                    .UsingCommandTimeout(10)
                    .Create();
                script = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("SerpoServer.Database.serposervermssql.sql").AsString();
            }


            db = result;


            db?.Execute(script);
        }


        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            return db.Query<T>(sql, args);
        }

        public IEnumerable<T> Query<T>(Sql sql)
        {
            return db.Query<T>(sql);
        }

        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args)
        {
            return db.Query(cb, sql, args);
        }

        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args)
        {
            return db.Query(cb, sql, args);
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql,
            params object[] args)
        {
            return db.Query(cb, sql, args);
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, string sql,
            params object[] args)
        {
            return db.Query(cb, sql, args);
        }

        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql)
        {
            return db.Query(cb, sql);
        }

        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql)
        {
            return db.Query(cb, sql);
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql)
        {
            return db.Query(cb, sql);
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, Sql sql)
        {
            return db.Query(cb, sql);
        }

        public IEnumerable<T1> Query<T1, T2>(string sql, params object[] args)
        {
            return db.Query<T1, T2>(sql, args);
        }

        public IEnumerable<T1> Query<T1, T2, T3>(string sql, params object[] args)
        {
            return db.Query<T1, T2, T3>(sql, args);
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4>(string sql, params object[] args)
        {
            return db.Query<T1, T2, T3, T4>(sql, args);
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4, T5>(string sql, params object[] args)
        {
            return db.Query<T1, T2, T3, T4, T5>(sql, args);
        }

        public IEnumerable<T1> Query<T1, T2>(Sql sql)
        {
            return db.Query<T1, T2>(sql);
        }

        public IEnumerable<T1> Query<T1, T2, T3>(Sql sql)
        {
            return db.Query<T1, T2, T3>(sql);
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4>(Sql sql)
        {
            return db.Query<T1, T2, T3, T4>(sql);
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4, T5>(Sql sql)
        {
            return db.Query<T1, T2, T3, T4, T5>(sql);
        }

        public IEnumerable<TRet> Query<TRet>(Type[] types, object cb, string sql, params object[] args)
        {
            return db.Query<TRet>(types, cb, sql, args);
        }

        public List<T> Fetch<T>(string sql, params object[] args)
        {
            return db.Fetch<T>(sql, args);
        }

        public List<T> Fetch<T>(Sql sql)
        {
            return db.Fetch<T>(sql);
        }

        public Page<T> Page<T>(long page, long itemsPerPage, string sqlCount, object[] countArgs, string sqlPage,
            object[] pageArgs)
        {
            return db.Page<T>(page, itemsPerPage, sqlCount, countArgs, sqlPage, pageArgs);
        }

        public Page<T> Page<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return db.Page<T>(page, itemsPerPage, sql, args);
        }

        public Page<T> Page<T>(long page, long itemsPerPage, Sql sql)
        {
            return db.Page<T>(page, itemsPerPage, sql);
        }

        public Page<T> Page<T>(long page, long itemsPerPage, Sql sqlCount, Sql sqlPage)
        {
            return db.Page<T>(page, itemsPerPage, sqlCount, sqlPage);
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return db.Fetch<T>(page, itemsPerPage, sql, args);
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, Sql sql)
        {
            return db.Fetch<T>(page, itemsPerPage, sql);
        }

        public List<T> SkipTake<T>(long skip, long take, string sql, params object[] args)
        {
            return db.SkipTake<T>(skip, take, sql, args);
        }

        public List<T> SkipTake<T>(long skip, long take, Sql sql)
        {
            return db.SkipTake<T>(skip, take, sql);
        }

        public bool Exists<T>(object primaryKey)
        {
            return db.Exists<T>(primaryKey);
        }

        public bool Exists<T>(string sqlCondition, params object[] args)
        {
            return db.Exists<T>(sqlCondition, args);
        }

        public T Single<T>(object primaryKey)
        {
            return db.Single<T>(primaryKey);
        }

        public T Single<T>(string sql, params object[] args)
        {
            return db.Single<T>(sql, args);
        }

        public T Single<T>(Sql sql)
        {
            return db.Single<T>(sql);
        }

        public T SingleOrDefault<T>(Sql sql)
        {
            return db.SingleOrDefault<T>(sql);
        }

        public T SingleOrDefault<T>(object primaryKey)
        {
            return db.SingleOrDefault<T>(primaryKey);
        }

        public T SingleOrDefault<T>(string sql, params object[] args)
        {
            return db.SingleOrDefault<T>(sql, args);
        }

        public T First<T>(string sql, params object[] args)
        {
            return db.First<T>(sql, args);
        }

        public T First<T>(Sql sql)
        {
            return db.First<T>(sql);
        }

        public T FirstOrDefault<T>(string sql, params object[] args)
        {
            return db.FirstOrDefault<T>(sql, args);
        }

        public T FirstOrDefault<T>(Sql sql)
        {
            return db.FirstOrDefault<T>(sql);
        }

        public List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args)
        {
            return db.Fetch(cb, sql, args);
        }

        public List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args)
        {
            return db.Fetch(cb, sql, args);
        }

        public List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args)
        {
            return db.Fetch(cb, sql, args);
        }

        public List<TRet> Fetch<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, string sql,
            params object[] args)
        {
            return db.Fetch(cb, sql, args);
        }

        public List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql)
        {
            return db.Fetch(cb, sql);
        }

        public List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql)
        {
            return db.Fetch(cb, sql);
        }

        public List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql)
        {
            return db.Fetch(cb, sql);
        }

        public List<TRet> Fetch<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, Sql sql)
        {
            return db.Fetch(cb, sql);
        }

        public List<T1> Fetch<T1, T2>(string sql, params object[] args)
        {
            return db.Fetch<T1, T2>(sql, args);
        }

        public List<T1> Fetch<T1, T2, T3>(string sql, params object[] args)
        {
            return db.Fetch<T1, T2, T3>(sql, args);
        }

        public List<T1> Fetch<T1, T2, T3, T4>(string sql, params object[] args)
        {
            return db.Fetch<T1, T2, T3, T4>(sql, args);
        }

        public List<T1> Fetch<T1, T2, T3, T4, T5>(string sql, params object[] args)
        {
            return db.Fetch<T1, T2, T3, T4, T5>(sql, args);
        }

        public List<T1> Fetch<T1, T2>(Sql sql)
        {
            return db.Fetch<T1, T2>(sql);
        }

        public List<T1> Fetch<T1, T2, T3>(Sql sql)
        {
            return db.Fetch<T1, T2, T3>(sql);
        }

        public List<T1> Fetch<T1, T2, T3, T4>(Sql sql)
        {
            return db.Fetch<T1, T2, T3, T4>(sql);
        }

        public List<T1> Fetch<T1, T2, T3, T4, T5>(Sql sql)
        {
            return db.Fetch<T1, T2, T3, T4, T5>(sql);
        }

        public IGridReader QueryMultiple(Sql sql)
        {
            return db.QueryMultiple(sql);
        }

        public IGridReader QueryMultiple(string sql, params object[] args)
        {
            return db.QueryMultiple(sql, args);
        }

        public object Insert(string tableName, object poco)
        {
            return db.Insert(tableName, poco);
        }

        public object Insert(string tableName, string primaryKeyName, object poco)
        {
            return db.Insert(tableName, primaryKeyName, poco);
        }

        public object Insert(string tableName, string primaryKeyName, bool autoIncrement, object poco)
        {
            return db.Insert(tableName, primaryKeyName, autoIncrement, poco);
        }

        public object Insert(object poco)
        {
            return db.Insert(poco);
        }

        public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
        {
            return db.Update(tableName, primaryKeyName, poco, primaryKeyValue);
        }

        public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue,
            IEnumerable<string> columns)
        {
            return db.Update(tableName, primaryKeyName, poco, primaryKeyValue, columns);
        }

        public int Update(string tableName, string primaryKeyName, object poco)
        {
            return db.Update(tableName, primaryKeyName, poco);
        }

        public int Update(string tableName, string primaryKeyName, object poco, IEnumerable<string> columns)
        {
            return db.Update(tableName, primaryKeyName, poco, columns);
        }

        public int Update(object poco, IEnumerable<string> columns)
        {
            return db.Update(poco, columns);
        }

        public int Update(object poco)
        {
            return db.Update(poco);
        }

        public int Update(object poco, object primaryKeyValue)
        {
            return db.Update(poco, primaryKeyValue);
        }

        public int Update(object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            return db.Update(poco, primaryKeyValue, columns);
        }

        public int Update<T>(string sql, params object[] args)
        {
            return db.Update<T>(sql, args);
        }

        public int Update<T>(Sql sql)
        {
            return db.Update<T>(sql);
        }

        public int Delete(string tableName, string primaryKeyName, object poco)
        {
            return db.Delete(tableName, primaryKeyName, poco);
        }

        public int Delete(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
        {
            return db.Delete(tableName, primaryKeyName, poco, primaryKeyValue);
        }

        public int Delete(object poco)
        {
            return db.Delete(poco);
        }

        public int Delete<T>(object pocoOrPrimaryKey)
        {
            return db.Delete<T>(pocoOrPrimaryKey);
        }

        public int Delete<T>(string sql, params object[] args)
        {
            return db.Delete<T>(sql, args);
        }

        public int Delete<T>(Sql sql)
        {
            return db.Delete<T>(sql);
        }

        public bool IsNew(string primaryKeyName, object poco)
        {
            return db.IsNew(primaryKeyName, poco);
        }

        public bool IsNew(object poco)
        {
            return db.IsNew(poco);
        }

        public void Save(string tableName, string primaryKeyName, object poco)
        {
            db.Save(tableName, primaryKeyName, poco);
        }

        public void Save(object poco)
        {
            db.Save(poco);
        }

        public int Execute(string sql, params object[] args)
        {
            return db.Execute(sql, args);
        }

        public int Execute(Sql sql)
        {
            return db.Execute(sql);
        }

        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            return db.ExecuteScalar<T>(sql, args);
        }

        public T ExecuteScalar<T>(Sql sql)
        {
            return db.ExecuteScalar<T>(sql);
        }

        public IDbTransaction Transaction => db.Transaction;

        public ITransaction GetTransaction()
        {
            return db.GetTransaction();
        }

        public void BeginTransaction()
        {
            db.BeginTransaction();
        }

        public void AbortTransaction()
        {
            db.AbortTransaction();
        }

        public void CompleteTransaction()
        {
            db.CompleteTransaction();
        }

        public IMapper DefaultMapper => db.DefaultMapper;

        public string LastSQL => db.LastSQL;

        public object[] LastArgs => db.LastArgs;

        public string LastCommand => db.LastCommand;

        public bool EnableAutoSelect
        {
            get => db.EnableAutoSelect;
            set => db.EnableAutoSelect = value;
        }

        public bool EnableNamedParams
        {
            get => db.EnableNamedParams;
            set => db.EnableNamedParams = value;
        }

        public int CommandTimeout
        {
            get => db.CommandTimeout;
            set => db.CommandTimeout = value;
        }

        public int OneTimeCommandTimeout
        {
            get => db.OneTimeCommandTimeout;
            set => db.OneTimeCommandTimeout = value;
        }

        public IProvider Provider => db.Provider;

        public string ConnectionString => db.ConnectionString;

        public IsolationLevel? IsolationLevel
        {
            get => db.IsolationLevel;
            set => db.IsolationLevel = value;
        }

        public static bool DbExists()
        {
            DbConnection conn = null;
            switch (ConfigurationProvider.ConfigurationFile.dbtype)
            {
                case "mssql":
                    conn = new SqlConnection(ConfigurationProvider.ConfigurationFile.connstring);

                    break;
                case "mysql":
                    conn = new MySqlConnection(ConfigurationProvider.ConfigurationFile.connstring);
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