using System;
using System.IO;
using Nancy;
using Nancy.Json.Simple;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using PetaPoco;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.View;

namespace SerpoServer.Api
{
    public class SettingsManager
    {
        private IDatabase db => TinyIoCContainer.Current.Resolve<IDatabase>();
        public HttpStatusCode SaveChanges(SettingsViewModel data)
        {

            if (data.settings_connstring == null)
                return HttpStatusCode.BadRequest;
            var site = new spo_site
            {
                site_id = data.site_id,
                site_domain = data.site_domain,
                site_name = data.site_name,
                site_ssl = data.site_ssl
            };
            var settings = new spo_settings
            {
                connstring = data.settings_connstring,
                dbtype = data.settings_dbtype,
                emailserver = data.settings_emailserver,
                emailuser = data.settings_emailuser,
                emailpsw = data.settings_emailpsw,
                emailport = data.settings_emailport
            };
            db.Update(site);
            ConfigurationProvider.UpdateFile(settings);
            return HttpStatusCode.OK;
        }


    }
}