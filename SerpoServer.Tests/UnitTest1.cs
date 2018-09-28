using System;
using System.Globalization;
using Nancy.Json.Simple;
using SerpoServer.Data.Models;
using SerpoServer.Data.Models.Enums;
using Xunit;

namespace SerpoServer.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void JsonDes()
        {
            var obj = new spo_page
            {
                page_id = 0,
                page_methods = RequestMethods.Get,
                page_response = ResponseMethods.View,
                page_route = "/",
                page_script = null,
                page_site = 0,
                page_view = "test"
            };
            var data = SimpleJson.SerializeObject(obj);
            var json =
                SimpleJson.DeserializeObject<spo_page>(data, new NancySerializationStrategy(),
                    DateTimeStyles.AssumeLocal);
            Assert.NotNull(json);
        }

        [Fact]
        public void setTypeValue()
        {
            var set = new spo_settings();
            var svm = new SettingsViewModel();
            svm.settings_connstring = "test";
            foreach (var field in svm.GetType().GetFields())
            {
                if (field.Name.Contains("settings"))
                {
                    object val = null;
                   set.GetType().GetField(field.Name).SetValue(val, field.GetValue(null));
                    Assert.NotNull(val);
                }
            }
        }
        
        public class SettingsViewModel
        {
            public string site_domain;
            public string site_name;
            public string site_ssl;
            public string settings_dbtype;
            public string settings_connstring;
            public string settings_emailserver;
            public string settings_emailuser;
            public string settings_emailpsw;
            public string settings_emailport;
        }
        
    }
}