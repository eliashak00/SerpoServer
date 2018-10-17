using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_days")]
    [PrimaryKey("day_id")]
    public class spo_day
    {
        [Column("day_id")] public int day_id { get; set; }

        [Column("day_views")] public int day_views { get; set; }

        [Column("day_site")] public int day_site { get; set; }

        [Column("day_date")] public System.DateTime day_date { get; set; }
    }
}