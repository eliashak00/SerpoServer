using System;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using SerpoServer.Api;
using SerpoServer.Data.Models;

namespace SerpoServer.Routes.Admin
{
    public class SettingsModule : NancyModule
    {
        public SettingsModule(SettingsManager settings) : base("/admin/settings")
        {
            this.RequiresAuthentication();
            Get("/", x => View["admin/views/settings.html", ConfigurationProvider.ConfigurationFile]);
            Post("/save", x =>
            {
                var data = this.Bind<spo_settings>();
                return settings.SaveChanges(data);
            });
        }
    }
}