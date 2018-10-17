using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Newtonsoft.Json.Linq;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Database;

namespace SerpoServer.Api
{
    public class CrudManager
    {
        private readonly IDatabase db = CallDb.GetDb();

        public IEnumerable<spo_crud> AllModules => db.Query<spo_crud>(
            "SELECT crud_id,crud_table, crud_password,(SELECT page_route FROM spo_pages WHERE page_crud = spo_cruds.crud_id) as crud_route,(crud_password IS NOT NULL) AS crud_haspsw, (crud_json IS NOT NULL) AS crud_data FROM spo_cruds");


        public spo_crud GetModule(string name)
        {
            return db.FirstOrDefault<spo_crud>(
                "SELECT *, (SELECT page_route FROM spo_pages WHERE page_crud = spo_cruds.crud_id) as crud_route FROM spo_cruds WHERE crud_name = @0",
                name);
        }

        public spo_crud GetModule(int id)
        {
            return db.FirstOrDefault<spo_crud>(
                "SELECT *, (SELECT page_route FROM spo_pages WHERE page_crud = spo_cruds.crud_id) as crud_route FROM spo_cruds WHERE crud_id = @0",
                id);
        }

        public HttpStatusCode CreateOrEdit(spo_crud module)
        {
            if (string.IsNullOrWhiteSpace(module.crud_table)) module.crud_table = Guid.NewGuid().ToString();
            if (module.crud_auth && string.IsNullOrWhiteSpace(module.crud_password))
                module.crud_password = Guid.NewGuid().ToString();


            var page = db.FirstOrDefault<spo_page>(
                "SELECT * FROM spo_pages INNER JOIN spo_cruds ON spo_cruds.crud_id = spo_pages.page_crud");
            if (module.crud_id != 0 && page != null)
            {
                page.page_route = module.crud_route;
                page.page_crud = module.crud_id;
                page.page_response = ResponseMethods.Crud;
                db.Update(page);
            }
            else
            {
                page = new spo_page
                {
                    page_id = 0,
                    page_view = null,
                    page_route = module.crud_route,
                    page_crud = module.crud_id,
                    page_response = ResponseMethods.Crud
                };
                db.Insert(page);
            }


            if (module.crud_id == 0)
                db.Insert(module);
            else
                db.Update(module);


            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            if (ConfigurationProvider.ConfigurationFile.dbtype == "mysql")
                db.Execute("CALL remove_crud(@0)", id);

            else
                db.Execute("EXEC remove_crud @id = " + id);
            return HttpStatusCode.OK;
        }

        public void InsertOrUpdate(spo_crud crud, string jsonItem)
        {
            var crudData =
                db.FirstOrDefault<string>("SELECT crud_json FROM spo_cruds WHERE crud_id = @0", crud.crud_id);
            var json = JArray.Parse(crudData);
            var jnew = JToken.Parse(jsonItem);
            var struc = JArray.Parse(crud.crud_struct);
            var item = struc.FirstOrDefault(f => f.Value<string>("Type") == "id");
            var keyName = item.Value<string>("Name");
            var newKeyVal = jnew.Value<string>(keyName);
            var jval = json.FirstOrDefault(t => t.Value<string>(keyName) == newKeyVal);
            if (jval == null)
                json.Add(jnew);
            else
                json[jval] = jnew;
        }

        public void Delete(spo_crud crud, string primaryKeyVal)
        {
            var crudData =
                db.FirstOrDefault<string>("SELECT crud_json FROM spo_cruds WHERE crud_id = @0", crud.crud_id);
            var json = JArray.Parse(crudData);
            var struc = JArray.Parse(crud.crud_struct);
            var item = struc.FirstOrDefault(f => f.Value<string>("Type") == "id");
            var keyName = item.Value<string>("Name");
            var jval = json.FirstOrDefault(t => t.Value<dynamic>(keyName) == primaryKeyVal);
            if (jval != null) json[jval] = null;
            db.Update(crud);
        }

        public string Get(spo_crud crud, dynamic primaryKeyVal)
        {
            var crudData =
                db.FirstOrDefault<string>("SELECT crud_json FROM spo_cruds WHERE crud_id = @0", crud.crud_id);
            var json = JArray.Parse(crudData);
            var struc = JArray.Parse(crud.crud_struct);
            var item = struc.FirstOrDefault(f => f.Value<string>("Type") == "id");
            var keyName = item?.Value<string>("Name");
            return json.FirstOrDefault(t => t.Value<dynamic>(keyName) == primaryKeyVal)?.ToString();
        }
    }
}