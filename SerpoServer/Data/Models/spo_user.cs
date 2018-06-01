using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_users")]
    [PrimaryKey("user_id")]
    public class spo_user
    {
        public int user_id;
        [Required]
        [MaxLength(40)]
        public string user_nickname;
        [Required]
        [EmailAddress]
        public string user_email;
        
        public string user_password;
        public string user_salt;
        
        public string user_avatar;
        public DateTime user_registerd;

    }
}