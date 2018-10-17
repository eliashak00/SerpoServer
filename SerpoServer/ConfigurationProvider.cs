// ################################################
// ##@project SerpoCMS.Core
// ##@filename ConfigurationProvider.cs
// ##@author Elias Håkansson
// ##@license MIT - License(see license.txt)
// ################################################

using System.IO;
using Nancy;
using Newtonsoft.Json;
using SerpoServer.Database.Models;

namespace SerpoServer
{
    public class ConfigurationProvider
    {
        public static volatile spo_settings file;
        private static IRootPathProvider RootPath => new RootPathProvider();

        private static string FilePath => Path.Combine(RootPath.GetRootPath(), "config.json");

        public static spo_settings ConfigurationFile
        {
            get
            {
                if (file != null) return file;
                var fileContent = File.ReadAllText(FilePath);
                var json = JsonConvert.DeserializeObject<spo_settings>(fileContent);
                file = json;
                if (file == null) UpdateFile(new spo_settings());
                return file;
            }
        }

        public static void UpdateFile(spo_settings settings)
        {
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(FilePath, json);
            file = settings;
        }
    }
}