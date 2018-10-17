using PetaPoco;

namespace SerpoServer.Data.Models
{
    [TableName("spo_modules")]
    [PrimaryKey("module_id")]
    public class spo_module
    {
        public int module_id { get; set; }
        public string module_name { get; set; }
        public string module_js { get; set; }
        public int module_pos { get; set; }
        public int module_lat { get; set; }
        public bool module_active { get; set; }
    }
}