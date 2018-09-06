using System.Collections.Generic;
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

        public IEnumerable<spo_modules> ActiveModules(int pos) =>  db.Query<spo_modules>("SELECT * FROM spo_modules WHERE module_active = 1 AND module_pos = @0", pos);
        public IEnumerable<spo_modules> AllModules =>  db.Query<spo_modules>("SELECT * FROM spo_modules");
        
        
        public spo_modules GetModule(string name)
        {
            return db.FirstOrDefault<spo_modules>("SELECT * FROM spo_modules WHERE module_name = @0", name);
        }
        public spo_modules GetModule(int id)
        {
            return db.FirstOrDefault<spo_modules>("SELECT * FROM spo_modules WHERE module_id = @0", id);
        }

        public void CreateOrEdit(spo_modules module)
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

        public void Delete(int id)
        {
            db.Delete<spo_modules>("DELETE FROM spo_modules WHERE module_id = @0", id);
        }
        
    }
}