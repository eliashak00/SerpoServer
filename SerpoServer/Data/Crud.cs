using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using PetaPoco;
using SerpoServer.Data.Models;

namespace SerpoServer.Data
{
    public class Crud<T>
    {
        private string tableName;
        private IDatabase db;
        public Crud(string tableName)
        {
            db = TinyIoCContainer.Current.Resolve<Connection>();
            this.tableName = tableName;

            if (db.FirstOrDefault<spo_crud>("SELECT crud_table FROM spo_cruds WHERE crud_table = @0", tableName) == null)
            {
                db.Insert(new spo_crud
                {
                    crud_json = "",
                    crud_table = tableName
                });
            }
        }

        public IEnumerable<T> Select()
        {
            var jdata = db.ExecuteScalar<spo_crud>("SELECT crud_json FROM spo_cruds WHERE crud_table = @0", tableName);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jdata.crud_json);
        }
        
        public T Select(Func<T, bool> parameters)
        {
            var data = Select();
            return data.FirstOrDefault(parameters);
        }

        public void Insert(T obj)
        {
            var data = Select().ToList();
            if (data.Exists(x => x.Equals(obj)))
            {
                data.Remove(obj);
            }
            data.Add(obj);     
            var json = JsonConvert.SerializeObject(data);
            db.Execute("INSERT INTO spo_cruds VALUES crud_table = @0, crud_json = @1", tableName, json);
        }


        public void Remove(Func<T, bool> parameters)
        {
            var data = Select(parameters);
            var list = Select().ToList();
            list.Remove(data);
            var json = JsonConvert.SerializeObject(list);
            db.Execute("INSERT INTO spo_cruds VALUES crud_table = @0, crud_json = @1", tableName, json);
        }
    }
}