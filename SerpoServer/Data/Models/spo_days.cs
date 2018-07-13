using System;
using PetaPoco;

namespace SerpoServer.Data.Models
{    
    [TableName("spo_days")]
    [PrimaryKey("day_id")]
    public class spo_day
    {
        [Column("day_id")]
        public int day_id { get; set; }
        [Column("day_stat")]
        public spo_stats day_stat { get; set; }
        [Column("day_views")]
        public int day_views { get; set; }
        [Column("day_date")]
        public DateTime day_date { get; set; }
    }
}