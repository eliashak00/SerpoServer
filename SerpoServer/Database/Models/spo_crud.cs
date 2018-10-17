using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_cruds")]
    [PrimaryKey("crud_id")]
    public class spo_crud
    {
        [Column("crud_id")] public int crud_id { get; set; }

        [Column("crud_table")] public string crud_table { get; set; }

        [ResultColumn] public string crud_route { get; set; }

        [ResultColumn] public bool crud_haspsw { get; set; }

        [Column("crud_struct")] public string crud_struct { get; set; }

        [Column("crud_auth")] public bool crud_auth { get; set; }

        [Column("crud_password")] public string crud_password { get; set; }

        [Column("crud_json")] public string crud_json { get; set; }
    }
}