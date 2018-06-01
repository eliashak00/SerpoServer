using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_sites")]
    [PrimaryKey("site_id")]
    public class spo_site
    {
        public int site_id;
        public string site_domain;
      
    }
}