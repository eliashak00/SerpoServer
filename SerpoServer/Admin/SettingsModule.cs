using Nancy;
using Nancy.Extensions;
using Nancy.Security;
using Newtonsoft.Json;
using SerpoServer.Api;
using SerpoServer.Data.Models.View;

namespace SerpoServer.Admin
{
    public class SettingsModule : NancyModule
    {
        public SettingsModule(SettingsManager settings) : base("/admin/settings")
        {
            this.RequiresAuthentication();
            Get("/",
                x => View["settings.html",
                    new {site = Context.GetSite(), settings = ConfigurationProvider.ConfigurationFile}]);
            Post("/save", x =>
            {
                var jsonObj = JsonConvert.DeserializeObject<SettingsViewModel>(Request.Body.AsString());
                return settings.SaveChanges(jsonObj);
            });
        }
    }
}