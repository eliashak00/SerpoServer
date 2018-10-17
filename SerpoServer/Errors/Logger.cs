using System;
using System.IO;
using System.Net.Http;
using Nancy;
using Nancy.Json.Simple;

namespace SerpoServer.Errors
{
    public static class Logger
    {
        private static readonly IRootPathProvider RootPath = new DefaultRootPathProvider();

        public static void Write(Exception e)
        {
            var path = Path.Combine(RootPath.GetRootPath(),
                "error.log");
            if (!File.Exists(path))
            {
                var configFile = File.Create(path);
                if (!configFile.CanWrite)
                    throw new Exception("Logfile not writeable");
            }

            var file = File.ReadAllText(path);
            file += Environment.NewLine + e;
            File.WriteAllText(file, path);
        }

        public static void Telemetry(Exception e)
        {
            using (var http = new HttpClient())
            {
                var json = SimpleJson.SerializeObject(e);
                http.PostAsync("http://serpocms.net/api/telemetry", new StringContent(json));
            }
        }
    }
}