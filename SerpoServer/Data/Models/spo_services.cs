using System.Collections.Generic;
using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_services")]
    [PrimaryKey("service_id")]
    public class spo_service
    {
        [Column("service_id")]
        public int service_id { get; set; }
        [Column("service_name")]
        public string service_name { get; set; }
        [Column("service_script")]
        public string service_script { get; set; }
        [Ignore]
        public IEnumerable<spo_site> service_sites { get; set; }
    }
}