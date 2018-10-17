using PetaPoco;
using SerpoServer.Data;

namespace SerpoServer.Database
{
    public static class CallDb
    {
        public static IDatabase GetDb()
        {
            if (string.IsNullOrWhiteSpace(ConfigurationProvider.ConfigurationFile.connstring) || !Connection.DbExists())
                return new NullDatabase();

            return new Connection();
        }
    }
}