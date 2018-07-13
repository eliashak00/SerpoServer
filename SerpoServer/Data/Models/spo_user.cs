using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_users")]
    [PrimaryKey("user_id")]
    public class spo_user
    {

        [Column("user_id")]
        public int user_id { get; set; }
        [Column("user_nick")]
        public string user_nick { get; set; }
        [Column("user_email")]
        public string user_email { get; set; }
        [Column("user_password")]
        public string user_password { get; set; }
        [Column("user_salt")]
        public string user_salt { get; set; }
        [Column("user_avatar")]
        public string user_avatar { get; set; }
        [Column("user_registerd")]
        public DateTime user_registerd { get; set; }

    }
}