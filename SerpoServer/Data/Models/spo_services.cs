using System.Collections.Generic;
using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_services")]
    [PrimaryKey("service_id")]
    public class spo_service
    {
        public int service_id;
        public string service_name;
        public string service_script;
        public IEnumerable<spo_site> service_sites;
    }
}