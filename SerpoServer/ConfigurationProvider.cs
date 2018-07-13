// ################################################
// ##@project SerpoCMS.Core
// ##@filename ConfigurationProvider.cs
// ##@author Elias Håkansson
// ##@license MIT - License(see license.txt)
// ################################################

using System;
using System.IO;
using Nancy;
using Nancy.Json.Simple;
using Nancy.TinyIoc;
using SerpoServer.Data.Models;

namespace SerpoServer
{
    public class ConfigurationProvider
    {
        private static IRootPathProvider RootPath => new RootPathProvider();

        private static string FilePath => Path.Combine(RootPath.GetRootPath(), "config.json");
        public static volatile spo_settings file;

        public static void UpdateFile(spo_settings settings)
        {
            var json = SimpleJson.SerializeObject(settings);
            File.WriteAllText(FilePath, json);
            file = settings;
        }

        public static spo_settings ConfigurationFile
        {
            get
            {
                if (file != null) return file;
                var fileContent = File.ReadAllText(FilePath);
                var json = SimpleJson.DeserializeObject<spo_settings>(fileContent);
                file = json;
                if (file == null)
                {
                    UpdateFile(new spo_settings());
                }
                return file;
            }
        }
    }
}