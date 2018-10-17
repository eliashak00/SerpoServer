namespace SerpoServer.Tests
{
    public class DapperTest
    {
        [Fact]
        public void test()
        {
            DbConnection db = new MySqlConnection(ConfigurationProvider.ConfigurationFile.connstring);

            var row = db.Execute(
                "INSERT INTO spo_sites(site_name, site_domain, site_ssl) VALUES (@site_name, @site_domain, @site_ssl)",
                new {site_name = "ee", site_domain = "testtest", site_ssl = true});

            Assert.True(row == 1);
        }
    }
}