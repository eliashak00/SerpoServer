using System.IO;
using Nancy;
using Nancy.Json.Simple;
using PetaPoco;
using SerpoServer.Data.Models;

namespace SerpoServer.Api
{
    public class SettingsManager
    {
        private IRootPathProvider root;
        public SettingsManager(IRootPathProvider root)
        {
            this.root = root;
        }

        public HttpStatusCode SaveChanges(spo_settings settings)
        {
            if (settings.connstring == null)
                return HttpStatusCode.BadRequest;
            ConfigurationProvider.UpdateFile(settings);
            return HttpStatusCode.OK;
        }
    }
}