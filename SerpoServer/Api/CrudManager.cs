using System;
using System.Collections.Generic;
using System.Linq;
using IronPython.Modules;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            if (string.IsNullOrWhiteSpace(module.crud_table))
            {
                module.crud_table = Guid.NewGuid().ToString();
            }
            if(module.crud_auth && string.IsNullOrWhiteSpace(module.crud_password)){
                module.crud_password = Guid.NewGuid().ToString();
            }

    
            spo_page page = null;
            if (module.crud_id != 0 && db.Exists<spo_page>("SELECT * FROM spo_pages WHERE page_crud = @0", module.crud_id)){
                page = db.FirstOrDefault<spo_page>("SELECT * FROM spo_pages WHERE page_crud = @0", module.crud_id);
                page.page_route = module.crud_route;
                page.page_crud = module.crud_id;
                page.page_response = Data.Models.Enums.ResponseMethods.Crud;
            }
            else{
                page = new spo_page
                {
                    page_id = 0,
                    page_view = null,
                    page_route = module.crud_route,
                    page_crud = module.crud_id,
                    page_response = Data.Models.Enums.ResponseMethods.Crud
                };
            }
            if(page == null){ 

                 db.Insert(page);
            }
            else{
                db.Update(page);
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
            db.Delete<spo_crud>(id);
            return HttpStatusCode.OK;
        }

        public void InsertOrUpdate(spo_crud crud, string jsonItem){
                        var json = JArray.Parse(crud.crud_json);
            var jnew = JToken.Parse(jsonItem);
            var struc = JArray.Parse(crud.crud_struct);
            var item = struc.FirstOrDefault(f => f.Value<string>("Type") == "id");   
            var keyName = item.Value<string>("Name");
            var newKeyVal = jnew.Value<string>(keyName);
            var jval = json.FirstOrDefault(t => t.Value<string>(keyName) == newKeyVal);
            if(jval == null){
                json.Add(jnew);
            }
            else{
                json[jval] = jnew;
            }
        }
        public void Delete(spo_crud crud, string primaryKeyVal){
             var json = JArray.Parse(crud.crud_json);
            var struc = JArray.Parse(crud.crud_struct);
            var item = struc.FirstOrDefault(f => f.Value<string>("Type") == "id");   
            var keyName = item.Value<string>("Name");
            var jval = json.FirstOrDefault(t => t.Value<dynamic>(keyName) == primaryKeyVal);
            if(jval != null){
                json[jval] = null;
            }
            db.Update(crud);
        }
        public string Get(spo_crud crud, string primaryKeyVal){
            var structure = JArray.Parse(crud.crud_struct).
            FirstOrDefault(t => t.Value<string>("type") == "id").
            Values<string>().
            Last();
            var crudData = db.FirstOrDefault<string>("SELECT crud_json FROM spo_cruds WHERE crud_id = @0", crud.crud_id);
            var json = JArray.Parse(crudData);
            return json.FirstOrDefault(t => t.Value<string>(structure) == primaryKeyVal).ToString();

        }
    }
}