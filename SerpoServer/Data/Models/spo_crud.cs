﻿using PetaPoco;

namespace SerpoServer.Data.Models
{
    
    [TableName("spo_cruds")]
    [PrimaryKey("crud_id")]
    public class spo_crud
    {
        [Column("crud_id")]
        public int crud_id { get; set; }
        [Column("crud_table")]
        public string crud_table { get; set; }
        [Column("crud_json")]
        public string crud_json { get; set; }
    }
}