using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_stats")]
    [PrimaryKey("stat_id")]
    public class spo_stats
    {
        [Column("stat_id")] public int stat_id { get; set; }

        [Column("stat_site")] public spo_site stat_site { get; set; }

        public int total_views { get; set; }
    }
}