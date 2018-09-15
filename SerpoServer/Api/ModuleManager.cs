﻿using System.Collections.Generic;
using PetaPoco;
using SerpoServer.Data;
using SerpoServer.Data.Models;

namespace SerpoServer.Api
{
    public class ModuleManager
    {
        private IDatabase db;
        public ModuleManager(Connection db)
        {
            this.db = db;
        }

        public IEnumerable<spo_module> ActiveModules(int pos) =>  db.Query<spo_module>("SELECT * FROM spo_modules WHERE module_active = 1 AND module_pos = @0", pos);
        public IEnumerable<spo_module> AllModules =>  db.Query<spo_module>("SELECT * FROM spo_modules");
        
        
        public spo_module GetModule(string name)
        {
            return db.FirstOrDefault<spo_module>("SELECT * FROM spo_modules WHERE module_name = @0", name);
        }
        public spo_module GetModule(int id)
        {
            return db.FirstOrDefault<spo_module>("SELECT * FROM spo_modules WHERE module_id = @0", id);
        }

        public void CreateOrEdit(IEnumerable<spo_module> modules)
        {
            foreach (var module in modules)
            {
                if (module.module_id == 0)
                {
                    db.Insert(module);
                }
                else
                {
                    db.Update(module);
                }
            }

    }

        public void Delete(int id)
        {
            db.Delete<spo_module>("DELETE FROM spo_modules WHERE module_id = @0", id);
        }
        
    }
}