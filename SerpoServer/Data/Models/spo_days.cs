using System;
using PetaPoco;

namespace SerpoServer.Data.Models
{    
    [TableName("spo_days")]
    [PrimaryKey("day_id")]
    public class spo_day
    {
        public int day_id;
        public spo_stats day_stat;
        public int day_views;
        public DateTime day_date;
    }
}