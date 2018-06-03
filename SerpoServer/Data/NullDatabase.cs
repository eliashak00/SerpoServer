using System;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
using PetaPoco.Core;

namespace SerpoServer.Data
{
    public class NullDatabase : IDatabase
    {
        public void Dispose()
        {
        }

        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<T> Query<T>(Sql sql)
        {
            return null;
        }

        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql)
        {
            return null;
        }

        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql)
        {
            return null;
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql)
        {
            return null;
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, Sql sql)
        {
            return null;
        }

        public IEnumerable<T1> Query<T1, T2>(string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<T1> Query<T1, T2, T3>(string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4>(string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4, T5>(string sql, params object[] args)
        {
            return null;
        }

        public IEnumerable<T1> Query<T1, T2>(Sql sql)
        {
            return null;
        }

        public IEnumerable<T1> Query<T1, T2, T3>(Sql sql)
        {
            return null;
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4>(Sql sql)
        {
            return null;
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4, T5>(Sql sql)
        {
            return null;
        }

        public IEnumerable<TRet> Query<TRet>(Type[] types, object cb, string sql, params object[] args)
        {
            return null;
        }

        public List<T> Fetch<T>(string sql, params object[] args)
        {
            return null;
        }

        public List<T> Fetch<T>(Sql sql)
        {
            return null;
        }

        public Page<T> Page<T>(long page, long itemsPerPage, string sqlCount, object[] countArgs, string sqlPage, object[] pageArgs)
        {
            return null;
        }

        public Page<T> Page<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return null;
        }

        public Page<T> Page<T>(long page, long itemsPerPage, Sql sql)
        {
            return null;
        }

        public Page<T> Page<T>(long page, long itemsPerPage, Sql sqlCount, Sql sqlPage)
        {
            return null;
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return null;
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, Sql sql)
        {
            return null;
        }

        public List<T> SkipTake<T>(long skip, long take, string sql, params object[] args)
        {
            return null;
        }

        public List<T> SkipTake<T>(long skip, long take, Sql sql)
        {
            return null;
        }

        public bool Exists<T>(object primaryKey)
        {
            return false;
        }

        public bool Exists<T>(string sqlCondition, params object[] args)
        {
            return false;
        }

        public T Single<T>(object primaryKey)
        {
            return default(T);
        }

        public T Single<T>(string sql, params object[] args)
        {
            return default(T);
        }

        public T Single<T>(Sql sql)
        {
            return default(T);
        }

        public T SingleOrDefault<T>(Sql sql)
        {
            return default(T);
        }

        public T SingleOrDefault<T>(object primaryKey)
        {
            return default(T);
        }

        public T SingleOrDefault<T>(string sql, params object[] args)
        {
            return default(T);
        }

        public T First<T>(string sql, params object[] args)
        {
            return default(T);
        }

        public T First<T>(Sql sql)
        {
            return default(T);
        }

        public T FirstOrDefault<T>(string sql, params object[] args)
        {
            return default(T);
        }

        public T FirstOrDefault<T>(Sql sql)
        {
            return default(T);
        }

        public List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args)
        {
            return null;
        }

        public List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args)
        {
            return null;
        }

        public List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args)
        {
            return null;
        }

        public List<TRet> Fetch<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, string sql, params object[] args)
        {
            return null;
        }

        public List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql)
        {
            return null;
        }

        public List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql)
        {
            return null;
        }

        public List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql)
        {
            return null;
        }

        public List<TRet> Fetch<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, Sql sql)
        {
            return null;
        }

        public List<T1> Fetch<T1, T2>(string sql, params object[] args)
        {
            return null;
        }

        public List<T1> Fetch<T1, T2, T3>(string sql, params object[] args)
        {
            return null;
        }

        public List<T1> Fetch<T1, T2, T3, T4>(string sql, params object[] args)
        {
            return null;
        }

        public List<T1> Fetch<T1, T2, T3, T4, T5>(string sql, params object[] args)
        {
            return null;
        }

        public List<T1> Fetch<T1, T2>(Sql sql)
        {
            return null;
        }

        public List<T1> Fetch<T1, T2, T3>(Sql sql)
        {
            return null;
        }

        public List<T1> Fetch<T1, T2, T3, T4>(Sql sql)
        {
            return null;
        }

        public List<T1> Fetch<T1, T2, T3, T4, T5>(Sql sql)
        {
            return null;
        }

        public IGridReader QueryMultiple(Sql sql)
        {
            return null;
        }

        public IGridReader QueryMultiple(string sql, params object[] args)
        {
            return null;
        }

        public object Insert(string tableName, object poco)
        {
            return null;
        }

        public object Insert(string tableName, string primaryKeyName, object poco)
        {
            return null;
        }

        public object Insert(string tableName, string primaryKeyName, bool autoIncrement, object poco)
        {
            return null;
        }

        public object Insert(object poco)
        {
            return null;
        }

        public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
        {
            return 0;
        }

        public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            return 0;
        }

        public int Update(string tableName, string primaryKeyName, object poco)
        {
            return 0;
        }

        public int Update(string tableName, string primaryKeyName, object poco, IEnumerable<string> columns)
        {
            return 0;
        }

        public int Update(object poco, IEnumerable<string> columns)
        {
            return 0;
        }

        public int Update(object poco)
        {
            return 0;
        }

        public int Update(object poco, object primaryKeyValue)
        {
            return 0;
        }

        public int Update(object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            return 0;
        }

        public int Update<T>(string sql, params object[] args)
        {
            return 0;
        }

        public int Update<T>(Sql sql)
        {
            return 0;
        }

        public int Delete(string tableName, string primaryKeyName, object poco)
        {
            return 0;
        }

        public int Delete(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
        {
            return 0;
        }

        public int Delete(object poco)
        {
            return 0;
        }

        public int Delete<T>(object pocoOrPrimaryKey)
        {
            return 0;
        }

        public int Delete<T>(string sql, params object[] args)
        {
            return 0;
        }

        public int Delete<T>(Sql sql)
        {
            return 0;
        }

        public bool IsNew(string primaryKeyName, object poco)
        {
            return false;
        }

        public bool IsNew(object poco)
        {
            return false;
        }

        public void Save(string tableName, string primaryKeyName, object poco)
        {
   
        }

        public void Save(object poco)
        {
        
        }

        public int Execute(string sql, params object[] args)
        {
            return 0;
        }

        public int Execute(Sql sql)
        {
            return 0;
        }

        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            return default(T);
        }

        public T ExecuteScalar<T>(Sql sql)
        {
            return default(T);
        }

        public IDbTransaction Transaction => null;

        public ITransaction GetTransaction()
        {
            return null;
        }

        public void BeginTransaction()
        {
          
        }

        public void AbortTransaction()
        {
      
        }

        public void CompleteTransaction()
        {
       
        }

        public IMapper DefaultMapper => null;

        public string LastSQL =>null;

        public object[] LastArgs =>  null;

        public string LastCommand => null;

        public bool EnableAutoSelect
        {
            get => false;
            set {}
        }

        public bool EnableNamedParams
        {
            get => false;
            set { }
        }

        public int CommandTimeout
        {
            get => 0;
            set { }
        }

        public int OneTimeCommandTimeout
        {
            get => 0;
            set { }
        }

        public IProvider Provider =>  null;

        public string ConnectionString =>  null;

        public IsolationLevel? IsolationLevel
        {
            get =>null;
            set { }
        }
    }
}