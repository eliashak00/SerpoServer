using PetaPoco;
using PetaPoco.Providers;

namespace SerpoServer.Data
{
    public class Connection
    {
        public IDatabase Get() => PetaPoco.DatabaseConfiguration.Build().UsingProvider<MySqlDatabaseProvider>().UsingConnectionString("")
            .Create();
    }
}