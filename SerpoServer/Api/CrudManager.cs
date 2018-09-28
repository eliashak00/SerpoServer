﻿using System;
using System.Collections.Generic;
using IronPython.Modules;
using Nancy;
using PetaPoco;
using SerpoServer.Data;
using SerpoServer.Data.Models;

namespace SerpoServer.Api
{
    public class CrudManager
    {
        private IDatabase db;
        public CrudManager(Connection db)
        {
            this.db = db;
        }

        public IEnumerable<spo_crud> AllModules =>  db.Query<spo_crud>("SELECT crud_id,crud_table, crud_route, (crud_json IS NULL) AS crud_data FROM spo_cruds");
        
        
        public spo_crud GetModule(string name)
        {
            return db.FirstOrDefault<spo_crud>("SELECT * FROM spo_cruds WHERE crud_name = @0", name);
        }
        public spo_crud GetModule(int id)
        {
            return db.FirstOrDefault<spo_crud>("SELECT * FROM spo_cruds WHERE crud_id = @0", id);
        }

        public HttpStatusCode CreateOrEdit(spo_crud module)
        {
            if (module.crud_table == null)
            {
                module.crud_table = Guid.NewGuid().ToString();
            }
            if (module.crud_id == 0)
            {
                db.Insert(module);
            }
            else
            {
                db.Update(module);
            }

            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            db.Delete<spo_crud>("DELETE FROM spo_cruds WHERE crud_id = @0", id);
            return HttpStatusCode.OK;
        }
    }
}