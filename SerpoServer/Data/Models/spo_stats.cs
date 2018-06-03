using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_stats")]
    [PrimaryKey("stat_id")]
    public class spo_stats
    {
        public int stat_id;
        public spo_site stat_site;
        public int total_views;
    }
}