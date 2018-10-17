using System;
using SerpoServer.Api;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using SerpoServer.Http;

namespace SerpoServer.Intepreter
{
    public class SerpoLib
    {
        private readonly CrudManager CRUD;

        public SerpoLib(CrudManager crud)
        {
            CRUD = crud;
        }

        public spo_crud GetTable(string name)
        {
            return CRUD.GetModule(name);
        }

        public void InsertOrEdit(spo_crud crud, string item)
        {
            CRUD.InsertOrUpdate(crud, item);
        }

        public string Get(spo_crud crud, string item)
        {
            return CRUD.Get(crud, item);
        }

        public void Delete(spo_crud crud, string item)
        {
            CRUD.Delete(crud, item);
        }

        public object HttpSend(string rec, bool secret, string key, string method, string data = null)
        {
            IHttp httpProvider;
            if (secret)
                httpProvider = new HttpPrivate();
            else
                httpProvider = new Http.Http();

            return !Enum.TryParse<RequestMethods>(method.ToLower(), out var meth)
                ? null
                : httpProvider.Send(rec, key, data, meth);
        }
    }
}