using System.ComponentModel.DataAnnotations;
using PetaPoco;
using SerpoServer.Data.Models.Enums;

namespace SerpoServer.Data.Models
{
    [TableName("spo_pages")]
    [PrimaryKey("page_id")]
    public class spo_page
    {
        public int page_id;
        [Required]
        public int page_site;
        [Required]
        public string page_route;
        
        public string page_script;
        public string page_view;
        
        [Required]
        public RequestMethods page_methods;
        [Required]
        public ResponseMethods page_resposne;
    }
}