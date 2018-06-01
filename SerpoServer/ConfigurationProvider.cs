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
        private static readonly IRootPathProvider RootPath = TinyIoCContainer.Current.CanResolve<IRootPathProvider>()
            ? TinyIoCContainer.Current.Resolve<IRootPathProvider>()
            : new DefaultRootPathProvider();

        static ConfigurationProvider()
        {
            updateFileVal();
        }

        public static string file { get; private set; }

        public static spo_settings ConfigurationFile
        {
            get
            {
                spo_settings fileObj;

                ParseFile();
                if (fileObj != null) return fileObj;
                //retry
                updateFileVal();
                ParseFile();
                if (fileObj == null) throw new ConfigurationException("No Config file");
                return fileObj;

                void ParseFile()
                {
                    var jsonObject = SimpleJson.DeserializeObject<spo_settings>(file);
                    fileObj = jsonObject;
                }
            }
        }

        private static void updateFileVal()
        {
            var path = Path.Combine(RootPath.GetRootPath(),
                "config.json");
            if (!File.Exists(path))
            {
                var configFile = File.Create(path);
                if (!configFile.CanWrite)
                    throw new Exception("Config file not writeable");
            }

            file = File.ReadAllText(path);
        }
    }
}