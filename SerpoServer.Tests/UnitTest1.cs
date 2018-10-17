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
                if (field.Name.Contains("settings"))
                {
                    object val = null;
                    set.GetType().GetField(field.Name).SetValue(val, field.GetValue(null));
                    Assert.NotNull(val);
                }
        }

        [Fact]
        public void jsonUpdate()
        {
            var crud = new spo_crud();
            crud.crud_struct = "[{\"Name\":\"id\",\"Type\":\"id\"},{\"Name\":\"name\",\"Type\":\"string\"}]";
            crud.crud_json =
                "[{\"id\": 1, \"name\": \"hello\"}, {\"id\": 1, \"name\": \"hello\"}, {\"id\": 3, \"name\": \"hello\"}]";
            var json = JArray.Parse(crud.crud_json);
            var jnew = JToken.Parse("{\"id\": 0, \"name\": \"he33llo\"}");
            var struc = JArray.Parse(crud.crud_struct);
            var item = struc.FirstOrDefault(f => f.Value<string>("Type") == "id");
            var keyName = item.Value<string>("Name");
            var newKeyVal = jnew.Value<string>(keyName);
            var jval = json.FirstOrDefault(t => t.Value<string>(keyName) == newKeyVal);
            if (jval == null)
                json.Add(jnew);
            else
                json[jval] = jnew;

            Assert.NotEmpty(json);
        }

        public void jsonDelete()
        {
            var crud = new spo_crud();
            crud.crud_struct = "[{\"Name\":\"id\",\"Type\":\"id\"},{\"Name\":\"name\",\"Type\":\"string\"}]";
            crud.crud_json =
                "[{\"id\": 1, \"name\": \"hello\"}, {\"id\": 1, \"name\": \"hello\"}, {\"id\": 3, \"name\": \"hello\"}]";
            var json = JArray.Parse(crud.crud_json);
            var primaryKey = 1;
            var struc = JArray.Parse(crud.crud_struct);
            var item = struc.FirstOrDefault(f => f.Value<string>("Type") == "id");
            var keyName = item.Value<string>("Name");
            var jval = json.FirstOrDefault(t => t.Value<dynamic>(keyName) == primaryKey);
            if (jval != null) json[jval] = null;
            Assert.NotEmpty(json);
        }


        [Fact]
        public void fileCheck()
        {
            var filePath = Path.Combine(new RootPathProvider().GetRootPath(), "test.jpg");
            var fileContent = File.ReadAllBytes(filePath);
            var compFile = CompressImage("test.jpg", fileContent).GetBuffer();
            var sFile = File.Create(filePath.Replace("test", "test2"));
            sFile.Write(compFile, 0, compFile.Length);
            sFile.Close();
            Assert.True(File.Exists(filePath.Replace("test", "test2")));
        }

        public MemoryStream CompressImage(string file, byte[] fileContent)
        {
            IImageEncoder format;
            if (Path.GetExtension(file) == ".jpg" || Path.GetExtension(file) == ".jpeg")
                format = new JpegEncoder();
            else if (Path.GetExtension(file) == ".png")
                format = new PngEncoder();
            else if (Path.GetExtension(file) == ".gif")
                format = new GifEncoder();
            else return null;
            using (var image = SixLabors.ImageSharp.Image.Load(fileContent))
            using (var outStream = new MemoryStream())
            {
                image.Mutate(c => c.Resize(200, 200));

                image.Save(outStream, format);
                return outStream;
            }
        }

        public class SettingsViewModel
        {
            public string settings_connstring;
            public string settings_dbtype;
            public string settings_emailport;
            public string settings_emailpsw;
            public string settings_emailserver;
            public string settings_emailuser;
            public string site_domain;
            public string site_name;
            public string site_ssl;
        }
    }
}