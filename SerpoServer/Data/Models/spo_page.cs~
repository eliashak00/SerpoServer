using System.ComponentModel.DataAnnotations;
using PetaPoco;
using SerpoServer.Data.Models.Enums;

namespace SerpoServer.Data.Models
{
    [TableName("spo_pages")]
    [PrimaryKey("page_id")]
    public class spo_page
    {
        [Column("page_id")]
        public int page_id { get; set; }
        
        [Column("page_site")] 
        public int page_site { get; set; }
        
        [Column("page_route")]
        public string page_route { get; set; }
        
        [Column("page_script")]
        public string page_script { get; set; }
        
        [Column("page_view")]
        public string page_view { get; set; }
        
        [Column("page_methods")]
        public RequestMethods page_methods { get; set; }

        [Column("page_crud")]
        public int page_crud { get; set; }
        
        [Column("page_response")]
        public ResponseMethods page_response { get; set; }
    }
}