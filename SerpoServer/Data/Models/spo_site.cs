using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_sites")]
    [PrimaryKey("site_id")]
    public class spo_site
    {
        [Column("site_id")]
        public int site_id { get; set; }
        [Column("site_domain")]
        public string site_domain { get; set; }
      
        [Column("site_name")]
        public string site_name { get; set; }
    }
}